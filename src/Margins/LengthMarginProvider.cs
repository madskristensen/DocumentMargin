using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace DocumentMargin.Margin
{
    [Export(typeof(IWpfTextViewMarginProvider))]
    [Name(nameof(LengthMargin))]
    [MarginContainer(PredefinedMarginNames.BottomRightCorner)]
    [Order(Before = PredefinedMarginNames.RowMargin)]
    [ContentType(StandardContentTypeNames.Text)]
    [TextViewRole(PredefinedTextViewRoles.Zoomable)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [DeferCreation(OptionName = DefaultTextViewHostOptions.EditingStateMarginOptionName)]
    internal class LengthMarginProvider : IWpfTextViewMarginProvider
    {
        public IWpfTextViewMargin CreateMargin(IWpfTextViewHost wpfTextViewHost, IWpfTextViewMargin marginContainer)
        {
            return new LengthMargin(wpfTextViewHost.TextView);
        }
    }
}
