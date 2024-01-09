using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
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
        public IWpfTextViewMargin CreateMargin(IWpfTextViewHost wpfTextViewHost, IWpfTextViewMargin marginContainer)
        {
            return new InfoMargin(wpfTextViewHost.TextView);
        }
    }
}
