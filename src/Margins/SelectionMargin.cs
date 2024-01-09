using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DocumentMargin.Margins;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Text.Editor;

namespace DocumentMargin.Margin
{
    internal class SelectionMargin : BaseMargin
    {
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
            Padding = new Thickness(5, 1, 0, 0);
            Visibility = Visibility.Collapsed;

            SetSelection();
            ToolTip = ""; // Initialize the tooltip
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            SetSelection();
        }

        private void SetSelection()
        {
            if (_view.Selection.IsEmpty)
            {
                Visibility = Visibility.Collapsed;
            }
            else
            {
                var length = _view.MultiSelectionBroker.AllSelections.Select(s => s.Extent.Length).Sum();
                Content = $"Sel: {length:#,#0}";

                if (_view.MultiSelectionBroker.AllSelections.Count > 1)
                {
                    Content += $" ({_view.MultiSelectionBroker.AllSelections.Count})";
                }

                Visibility = Visibility.Visible;
            }
        }

        protected override void OnToolTipOpening(ToolTipEventArgs e)
        {
            StringBuilder sb = new();

            for (var i = 0; i < _view.MultiSelectionBroker.AllSelections.Count; i++)
            {
                Microsoft.VisualStudio.Text.Selection selection = _view.MultiSelectionBroker.AllSelections[i];
                sb.AppendLine($"Selection {i + 1}:\t{selection.Extent.Length:#,#0}");
            }

            ToolTip = new ToolTip
            {
                Background = Background,
                Foreground = Foreground,
                Placement = System.Windows.Controls.Primitives.PlacementMode.Top,
                Content = sb.ToString().Trim(),
            };
        }

        public override void Dispose()
        {
            if (!_isDisposed)
            {
                GC.SuppressFinalize(this);
                _isDisposed = true;

                _view.Selection.SelectionChanged -= OnSelectionChanged;
            }
        }
    }
}
