using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Telemetry;

namespace DocumentMargin.Commands
{
    [Command(PackageIds.EncodingMenuDynamicStart)]
    internal class EncodingMenuCommand : BaseDynamicCommand<EncodingMenuCommand, Encoding>
    {
        private EncodingMenuCommandBridge _bridge;
        private IReadOnlyList<Encoding> _encodings;

        protected override async Task InitializeCompletedAsync()
        {
            _bridge = await Package.GetServiceAsync<EncodingMenuCommandBridge, EncodingMenuCommandBridge>();
        }

        protected override IReadOnlyList<Encoding> GetItems()
        {
            return _encodings ??= Encoding
                .GetEncodings()
                .OrderBy(e => e.DisplayName)
                .Select(e => e.GetEncoding())
                .Where(e => e.IsBrowserSave)
                .ToList();
        }

        protected override void BeforeQueryStatus(OleMenuCommand menuItem, EventArgs e, Encoding item)
        {
            menuItem.Text = $"{item.EncodingName} - Codepage {item.CodePage}";
            menuItem.Checked = item == _bridge.CurrentDocument?.Encoding;
        }

        protected override void Execute(OleMenuCmdEventArgs e, Encoding item)
        {
            if (_bridge.CurrentDocument is not null)
            {
                _bridge.CurrentDocument.Encoding = item;
                _bridge.CurrentDocument.UpdateDirtyState(true, DateTime.Now);

                TelemetryEvent tel = Telemetry.CreateEvent("changeencoding");
                tel.Properties["encoding"] = item.EncodingName;

                Telemetry.TrackEvent(tel);
            }
        }
    }
}