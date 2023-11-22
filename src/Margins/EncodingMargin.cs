using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace DocumentMargin.Margin
{
    internal class EncodingMargin : TextBlock, IWpfTextViewMargin
    {
        public const string MarginName = "Document Margin";
        private readonly ITextDocument _doc;
        private bool _isDisposed;

        public EncodingMargin(ITextDocument doc)
        {
            _doc = doc;
            doc.EncodingChanged += OnEncodingChanged;

            SetResourceReference(BackgroundProperty, EnvironmentColors.ScrollBarBackgroundBrushKey);
            SetResourceReference(ForegroundProperty, EnvironmentColors.ComboBoxFocusedTextBrushKey);
            Padding = new Thickness(5, 3, 5, 0);

            SetEncoding(_doc.Encoding);

            ContextMenu = new ContextMenu();
            MouseUp += OnMouseUp;

            foreach (EncodingInfo encodingInfo in Encoding.GetEncodings().OrderBy(e => e.DisplayName))
            {
                Encoding enc = encodingInfo.GetEncoding();

                if (enc.IsBrowserSave)
                {
                    _ = ContextMenu.Items.Add(new MenuItem { Header = $"{enc.EncodingName} - Codepage {enc.CodePage}", Command = new DelegateCommand(() => { _doc.Encoding = enc; _doc.UpdateDirtyState(true, DateTime.Now); }) });
                }
            }
        }

        private void OnMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ContextMenu.IsOpen = true;
        }

        private void OnEncodingChanged(object sender, EncodingChangedEventArgs e)
        {
            SetEncoding(e.NewEncoding);
        }

        private void SetEncoding(Encoding encoding)
        {
            Text = encoding.EncodingName;
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
                return true;
            }
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                GC.SuppressFinalize(this);
                _isDisposed = true;

                _doc.EncodingChanged -= OnEncodingChanged;
                MouseUp -= OnMouseUp;
            }
        }

        public ITextViewMargin GetTextViewMargin(string marginName)
        {
            return (marginName == MarginName) ? (IWpfTextViewMargin)this : null;
        }
    }
}
