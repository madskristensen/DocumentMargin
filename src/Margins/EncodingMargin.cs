using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Windows;
using System.Windows.Input;
using DocumentMargin.Commands;
using DocumentMargin.Margins;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Text;

namespace DocumentMargin.Margin
{
    internal class EncodingMargin : BaseMargin
    {
        private readonly ITextDocument _doc;
        private bool _isDisposed;

        public EncodingMargin(ITextDocument doc)
        {
            _doc = doc;
            doc.EncodingChanged += OnEncodingChanged;

            SetColors();
            FontSize = 11;
            Margin = new Thickness(0, 0, 0, 0);
            Padding = new Thickness(9, 1, 9, 0);

            MouseUp += OnMouseUp;
            MouseEnter += SetColors;
            MouseLeave += SetColors;

            SetEncoding(_doc.Encoding);
        }

        public override string MarginName => "Encoding Margin";

        private void SetColors(object sender = null, MouseEventArgs e = null)
        {
            if (IsMouseDirectlyOver)
            {
                SetResourceReference(BackgroundProperty, EnvironmentColors.CommandBarMouseOverBackgroundGradientBrushKey);
                SetResourceReference(ForegroundProperty, EnvironmentColors.CommandBarTextHoverBrushKey);
            }
            else
            {
                SetResourceReference(BackgroundProperty, EnvironmentColors.ScrollBarBackgroundBrushKey);
                SetResourceReference(ForegroundProperty, EnvironmentColors.ToolWindowTextBrushKey);
            }
        }

        [SuppressMessage("Usage", "VSTHRD102:Implement internal logic asynchronously", Justification = "Event handler.")]
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                EncodingMenuCommandBridge bridge = await VS.GetRequiredServiceAsync<EncodingMenuCommandBridge, EncodingMenuCommandBridge>();
                Point point = PointToScreen(e.GetPosition(this));
                await bridge.ShowAsync(_doc, (int)point.X, (int)point.Y);
            });
        }

        private void OnEncodingChanged(object sender, EncodingChangedEventArgs e)
        {
            SetEncoding(e.NewEncoding);
        }

        private void SetEncoding(Encoding encoding)
        {
            Text = encoding.EncodingName;
        }

        public override void Dispose()
        {
            if (!_isDisposed)
            {
                GC.SuppressFinalize(this);
                _isDisposed = true;

                _doc.EncodingChanged -= OnEncodingChanged;
                MouseUp -= OnMouseUp;
            }
        }
    }
}
