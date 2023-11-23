using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace DocumentMargin.Margin
{
    internal class EncodingMargin : TextBlock, IWpfTextViewMargin
    {
        public const string MarginName = "Encoding Margin";
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
            ContextMenu ??= new ContextMenu();

            SetEncoding(_doc.Encoding);
        }

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

        private IEnumerable<MenuItem> GetContextMenuItems()
        {
            foreach (EncodingInfo encodingInfo in Encoding.GetEncodings().OrderBy(e => e.DisplayName))
            {
                Encoding enc = encodingInfo.GetEncoding();

                if (enc.IsBrowserSave)
                {
                    yield return new MenuItem { Header = $"{enc.EncodingName} - Codepage {enc.CodePage}", Command = new DelegateCommand(() => { _doc.Encoding = enc; _doc.UpdateDirtyState(true, DateTime.Now); }) };
                }
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ContextMenu.ItemsSource = GetContextMenuItems();
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
            return (marginName == MarginName) ? this : null;
        }
    }
}
