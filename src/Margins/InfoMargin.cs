using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace DocumentMargin.Margin
{
    internal class InfoMargin : DockPanel, IWpfTextViewMargin
    {
        private readonly ITextView2 _view;
        private readonly ITagAggregator<IClassificationTag> _tagAggregator;

        public InfoMargin(ITextView view, ITagAggregator<IClassificationTag> tagAggregator)
        {
            _view = (ITextView2)view;
            _tagAggregator = tagAggregator;

            SetResourceReference(BackgroundProperty, EnvironmentColors.ScrollBarBackgroundBrushKey);

            Children.Add(new CrispImage()
            {
                Moniker = KnownMonikers.StatusInformationOutlineNoColor,
                Width = 14,
                Height = 14,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(4, 0, 5, 0),
            });

            ToolTip = new ToolTip()
            {
                Background = Background,
                Foreground = FindResource(EnvironmentColors.ToolWindowTextBrushKey) as Brush,
                Placement = System.Windows.Controls.Primitives.PlacementMode.Top,
            };
        }

        public FrameworkElement VisualElement => this;

        public double MarginSize => ActualHeight;

        public bool Enabled => true;

        protected override void OnToolTipOpening(ToolTipEventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Document");
            sb.AppendLine($"   Length:\t{_view.TextSnapshot.Length:#,#0}");
            sb.AppendLine($"   Lines:\t\t{_view.TextSnapshot.LineCount:#,#0}");
            sb.AppendLine($"   Language:\t{_view.TextBuffer.ContentType.DisplayName}");

            sb.AppendLine();
            sb.AppendLine("Caret");
            sb.AppendLine($"   Position:\t{_view.Caret.Position.BufferPosition.Position:#,#0}");
            sb.AppendLine($"   Classification:\t{GetClassificationName()}");

            ((ToolTip)ToolTip).Content = sb.ToString().Trim();
        }

        private string GetClassificationName()
        {
            var name = "None";

            if (_view.TextSnapshot.Length == 0)
            {
                return name;
            }

            int position = _view.Caret.Position.BufferPosition;

            if (position >= _view.TextSnapshot.Length)
            {
                position--;

            }

            var caretSpan = new SnapshotSpan(_view.TextSnapshot, position, 1);
            IMappingTagSpan<IClassificationTag> tagSpan = _tagAggregator.GetTags(caretSpan).FirstOrDefault();

            if (tagSpan != null)
            {
                name = tagSpan.Tag.ClassificationType.Classification;

                if (name.Contains(" - "))
                {
                    var index = name.IndexOf(" - ", StringComparison.Ordinal);
                    name = name.Substring(0, index).Trim();
                }
            }

            return name;
        }

        public ITextViewMargin GetTextViewMargin(string marginName)
        {
            return (marginName == GetType().Name) ? this : null;
        }

        public void Dispose()
        {

        }
    }
}
