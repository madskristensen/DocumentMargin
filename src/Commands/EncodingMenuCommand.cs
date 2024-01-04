using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EnvDTE;
using Microsoft.VisualStudio.Telemetry;

namespace DocumentMargin.Commands
{
    [Command(PackageIds.EncodingMenuDynamicStart)]
    internal partial class EncodingMenuCommand : BaseDynamicCommand<EncodingMenuCommand, Encoding>
    {
        private EncodingMenuCommandBridge _bridge;
        private List<Encoding> _encodings;

        protected override async Task InitializeCompletedAsync()
        {
            _bridge = await Package.GetServiceAsync<EncodingMenuCommandBridge, EncodingMenuCommandBridge>();
        }

        protected override IReadOnlyList<Encoding> GetItems()
        {
            if (_encodings == null)
            {
                _encodings = Encoding
                    .GetEncodings()
                    .Select(e => e.GetEncoding())
                    .Where(e => e.IsBrowserSave && e.CodePage != 65001 && !e.EncodingName.Contains("(Windows)") && !e.EncodingName.Contains("(DOS)"))
                    .ToList();

                _encodings.Add(new UTF8WithBomEncoding());
                _encodings.Add(new UTF8WithoutBomEncoding());
                _encodings = _encodings.OrderBy(e => e.EncodingName).ToList();
            }

            return _encodings;
        }

        protected override void BeforeQueryStatus(OleMenuCommand menuItem, EventArgs e, Encoding item)
        {
            var name = item.EncodingName;

            if (item.CodePage == 1200)
            {
                name = "Unicode (UTF-16)";
            }

            Encoding fileEncoding = _bridge.CurrentDocument?.Encoding;
            var isChecked = item.BodyName == fileEncoding.BodyName;

            if (item.CodePage == 65001 && item.CodePage == fileEncoding.CodePage)
            {
                _bridge.CurrentDocument.TextBuffer.Properties.TryGetProperty("hasbom", out bool hasBom);
                isChecked = (item is UTF8WithBomEncoding && hasBom) || (item is UTF8WithoutBomEncoding && !hasBom);
            }

            menuItem.Text = name;
            menuItem.Checked = isChecked;
        }

        protected override void Execute(OleMenuCmdEventArgs e, Encoding item)
        {
            if (_bridge.CurrentDocument is not null)
            {
                if (_bridge.CurrentDocument.IsDirty)
                {
                    _bridge.CurrentDocument.Save();
                }

                File.WriteAllText(_bridge.CurrentDocument.FilePath, _bridge.CurrentDocument.TextBuffer.CurrentSnapshot.GetText(), item);

                TelemetryEvent tel = Telemetry.CreateEvent("changeencoding");
                tel.Properties["encoding"] = item.EncodingName;

                Telemetry.TrackEvent(tel);
            }
        }
    }
}