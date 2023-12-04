using System.Windows;
using DocumentMargin.Margins;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace DocumentMargin.Margin
{
    internal class LengthMargin : BaseMargin
    {
        public override string MarginName => "Length Margin";
        private readonly ITextView2 _view;
        private bool _isDisposed;

        public LengthMargin(ITextView view)
        {
            _view = (ITextView2)view;
            _view.TextBuffer.PostChanged += OnTextBufferChanged;

            SetResourceReference(BackgroundProperty, EnvironmentColors.ScrollBarBackgroundBrushKey);
            SetResourceReference(ForegroundProperty, EnvironmentColors.ComboBoxFocusedTextBrushKey);
            FontSize = 11;
            Margin = new Thickness(0, 0, 0, 0);
            Padding = new Thickness(0, 1, 0, 0);

            SetValue();
        }

        private void OnTextBufferChanged(object sender, EventArgs e)
        {
            SetValue();
        }

        private void SetValue()
        {
            ITextSnapshot snapshot = _view.TextSnapshot;
            Content = $"Length: {snapshot.Length}    Lines: {snapshot.LineCount}";
        }

        public override void Dispose()
        {
            if (!_isDisposed)
            {
                GC.SuppressFinalize(this);
                _isDisposed = true;

                _view.TextBuffer.PostChanged -= OnTextBufferChanged;
            }
        }
    }
}
