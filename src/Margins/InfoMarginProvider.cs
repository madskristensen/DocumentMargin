using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace DocumentMargin.Margin
{
    [Export(typeof(IWpfTextViewMarginProvider))]
    [Name(nameof(InfoMargin))]
    [MarginContainer(PredefinedMarginNames.BottomRightCorner)]
    [Order(After = nameof(EncodingMargin))]
    [ContentType(StandardContentTypeNames.Text)]
    [TextViewRole(PredefinedTextViewRoles.Zoomable)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal class InfoMarginProvider : IWpfTextViewMarginProvider
    {
        [Import]
        internal IViewTagAggregatorFactoryService _tagAggregator = null;

        public IWpfTextViewMargin CreateMargin(IWpfTextViewHost wpfTextViewHost, IWpfTextViewMargin marginContainer)
        {
            ITagAggregator<IClassificationTag> tagAggregator = _tagAggregator.CreateTagAggregator<IClassificationTag>(wpfTextViewHost.TextView);
            return new InfoMargin(wpfTextViewHost.TextView, tagAggregator);
        }
    }
}
