using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace DocumentMargin.Margin
{
    [Export(typeof(IWpfTextViewMarginProvider))]
    [Name(nameof(SelectionMargin))]
    [MarginContainer(PredefinedMarginNames.BottomRightCorner)]
    [Order(Before = nameof(LengthMargin))]
    [Order(Before = PredefinedMarginNames.RowMargin)]
    [ContentType(StandardContentTypeNames.Text)]
    [TextViewRole(PredefinedTextViewRoles.Zoomable)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [DeferCreation(OptionName = DefaultTextViewHostOptions.EditingStateMarginOptionName)]
    internal class SelectionMarginProvider : IWpfTextViewMarginProvider
    {
        public IWpfTextViewMargin CreateMargin(IWpfTextViewHost wpfTextViewHost, IWpfTextViewMargin marginContainer)
        {
            // Disable selection (MULTI/BOX) from showing up in the editor margin
            wpfTextViewHost.TextView.Options.SetOptionValue(DefaultTextViewHostOptions.SelectionStateMarginOptionId, false);

            return new SelectionMargin(wpfTextViewHost.TextView);
        }
    }
}
