﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Text.Editor;

namespace DocumentMargin.Margin
{
    internal class InfoMargin : DockPanel, IWpfTextViewMargin
    {
        private readonly ITextView2 _view;

        public InfoMargin(ITextView view)
        {
            _view = (ITextView2)view;

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
            sb.AppendLine($"Length:\t\t{_view.TextSnapshot.Length:#,#0}");
            sb.AppendLine($"Lines:\t\t{_view.TextSnapshot.LineCount:#,#0}");
            sb.AppendLine($"Caret:\t\t{_view.Caret.Position.BufferPosition.Position:#,#0}");
            sb.AppendLine($"Language:\t{_view.TextBuffer.ContentType.DisplayName}");

            ((ToolTip)ToolTip).Content = sb.ToString().Trim();
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