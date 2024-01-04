using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DocumentMargin.Commands;
using DocumentMargin.Margins;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;

namespace DocumentMargin.Margin
{
    internal class EncodingMargin : BaseMargin
    {
        private readonly ITextDocument _doc;
        private readonly JoinableTaskFactory _jtf;
        private bool _isDisposed;

        public EncodingMargin(ITextDocument doc, JoinableTaskFactory jtf)
        {
            _doc = doc;
            _jtf = jtf;
            doc.EncodingChanged += OnEncodingChanged;

            SetColors();
            FontSize = 11;
            Margin = new Thickness(0, 0, 0, 0);
            Padding = new Thickness(9, 0, 9, 0);
            BorderThickness = new Thickness(1);

            MouseUp += OnMouseUp;
            MouseEnter += SetColors;
            MouseLeave += SetColors;

            SetEncodingAsync(_doc.Encoding).FireAndForget();
        }

        public override string MarginName => "Encoding Margin";

        private void SetColors(object sender = null, MouseEventArgs e = null)
        {
            if (IsMouseOver)
            {
                SetResourceReference(BackgroundProperty, EnvironmentColors.CommandBarMouseOverBackgroundGradientBrushKey);
                SetResourceReference(ForegroundProperty, EnvironmentColors.CommandBarTextHoverBrushKey);
                SetResourceReference(BorderBrushProperty, EnvironmentColors.CommandBarHoverOverSelectedIconBorderBrushKey);
            }
            else
            {
                SetResourceReference(BackgroundProperty, EnvironmentColors.ScrollBarBackgroundBrushKey);
                SetResourceReference(ForegroundProperty, EnvironmentColors.ToolWindowTextBrushKey);
                SetResourceReference(BorderBrushProperty, EnvironmentColors.ScrollBarBackgroundBrushKey);

            }
        }

        [SuppressMessage("Usage", "VSTHRD102:Implement internal logic asynchronously", Justification = "Event handler.")]
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _jtf.Run(async () =>
            {
                EncodingMenuCommandBridge bridge = await VS.GetRequiredServiceAsync<EncodingMenuCommandBridge, EncodingMenuCommandBridge>();
                Point point = PointToScreen(e.GetPosition(this));
                await bridge.ShowAsync(_doc, (int)point.X, (int)point.Y);
            });
        }

        private void OnEncodingChanged(object sender, EncodingChangedEventArgs e)
        {
            SetEncodingAsync(e.NewEncoding).FireAndForget();
        }

        private async Task SetEncodingAsync(Encoding encoding)
        {
            var name = encoding.BodyName.Replace("iso", "ISO").Replace("utf", "UTF");

            if (encoding.CodePage == 1200)
            {
                name = "UTF-16";
            }

            else if (encoding.CodePage == 65001 && await HasBomAsync(_doc))
            {
                name = "UTF-8 with BOM";
            }

            await _jtf.SwitchToMainThreadAsync();

            Content = name;
        }

        public static async Task<bool> HasBomAsync(ITextDocument document)
        {
            using (Stream fs = new FileStream(document.FilePath, FileMode.Open))
            {
                var bits = new byte[3];
                await fs.ReadAsync(bits, 0, 3);

                var hasBom = bits[0] == 0xEF && bits[1] == 0xBB && bits[2] == 0xBF;

                document.TextBuffer.Properties["hasbom"] = hasBom;

                return hasBom;
            }
        }

        public override void Dispose()
        {
            if (!_isDisposed)
            {
                GC.SuppressFinalize(this);
                _isDisposed = true;

                _doc.EncodingChanged -= OnEncodingChanged;
                MouseUp -= OnMouseUp;
                MouseEnter -= SetColors;
                MouseLeave -= SetColors;
            }
        }
    }
}
