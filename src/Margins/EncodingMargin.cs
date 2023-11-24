using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

            ContextMenu ??= new ContextMenu();
            //Themes.SetUseVsTheme(ContextMenu, true);
            
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

        private IEnumerable<MenuItem> GetContextMenuItems()
        {
            foreach (EncodingInfo encodingInfo in Encoding.GetEncodings().OrderBy(e => e.DisplayName))
            {
                Encoding enc = encodingInfo.GetEncoding();

                if (enc.IsBrowserSave)
                {
                    yield return new MenuItem { 
                        Header = $"{enc.EncodingName} - Codepage {enc.CodePage}", 
                        IsChecked = enc == _doc.Encoding, 
                        Command = new DelegateCommand(() => { _doc.Encoding = enc; _doc.UpdateDirtyState(true, DateTime.Now); }) 
                    };
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
