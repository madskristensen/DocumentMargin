using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            Padding = new Thickness(3, 1, 20, 0);
            MinWidth = 50;

            SetSelection();
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            SetSelection();
        }

        private void SetSelection()
        {
            if (_view.Selection.IsEmpty)
            {
                Content = $"Sel: 0";
                ToolTip = null;
            }
            else
            {
                var length = _view.MultiSelectionBroker.AllSelections.Select(s => s.Extent.Length).Sum();
                Content = $"Sel: {length:#,#0}";

                if (_view.MultiSelectionBroker.AllSelections.Count > 1)
                {
                    Content += $" ({_view.MultiSelectionBroker.AllSelections.Count})";
                }

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
