using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Editor.OptionsExtensionMethods;

namespace DocumentMargin.Margin
{
    internal class SelectionMargin : TextBlock, IWpfTextViewMargin
    {
        public const string MarginName = "Selection Margin";
        private readonly ITextView2 _view;
        private bool _isDisposed;

        public SelectionMargin(ITextView view)
        {
            _view = (ITextView2)view;
            _view.Selection.SelectionChanged += OnSelectionChanged;

            SetResourceReference(BackgroundProperty, EnvironmentColors.ScrollBarBackgroundBrushKey);
            SetResourceReference(ForegroundProperty, EnvironmentColors.ComboBoxFocusedTextBrushKey);
            FontSize = 11;
            Margin = new Thickness(0, 0, 0, 0);
            Padding = new Thickness(9, 1, 9, 0);
            Visibility = Visibility.Collapsed;

            SetSelection();
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            SetSelection();
        }

        private void SetSelection()
        {
            var length = _view.MultiSelectionBroker.AllSelections.Select(s => s.Extent.Length).Sum();

            if (length > 0)
            {
                Text = $"Sel: {length}";
                Visibility = Visibility.Visible;
            }
            else
            {
                Visibility = Visibility.Collapsed;
            }
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

                _view.Selection.SelectionChanged -= OnSelectionChanged;
            }
        }

        public ITextViewMargin GetTextViewMargin(string marginName)
        {
            return (marginName == MarginName) ? this : null;
        }
    }
}
