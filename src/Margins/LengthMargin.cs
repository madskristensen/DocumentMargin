using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Editor.OptionsExtensionMethods;

namespace DocumentMargin.Margin
{
    internal class LengthMargin : TextBlock, IWpfTextViewMargin
    {
        public const string MarginName = "Length Margin";
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
            Padding = new Thickness(0, 3, 0, 0);

            SetValue();
        }

        private void OnTextBufferChanged(object sender, EventArgs e)
        {
            SetValue();
        }

        private void SetValue()
        {
            _ = ThreadHelper.JoinableTaskFactory.StartOnIdle(() =>
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                ITextSnapshot snapshot = _view.TextSnapshot;
                Text = $"Length: {snapshot.Length}    Lines: {snapshot.LineCount}";

            }, VsTaskRunContext.UIThreadIdlePriority);
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(MarginName);
            }
        }
        public FrameworkElement VisualElement
        {
            get
            {
                ThrowIfDisposed();
                return this;
            }
        }
        public double MarginSize
        {
            get
            {
                ThrowIfDisposed();
                return ActualHeight;
            }
        }

        public bool Enabled
        {
            get
            {
                ThrowIfDisposed();
                return _view.Options.IsLineNumberMarginEnabled(); ;
            }
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                GC.SuppressFinalize(this);
                _isDisposed = true;

                _view.TextBuffer.PostChanged -= OnTextBufferChanged;
            }
        }

        public ITextViewMargin GetTextViewMargin(string marginName)
        {
            return (marginName == MarginName) ? this : null;
        }
    }
}
