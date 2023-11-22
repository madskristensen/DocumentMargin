using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace DocumentMargin.Margin
{
    [Export(typeof(IWpfTextViewMarginProvider))]
    [Name(SelectionMargin.MarginName)]
    [MarginContainer(PredefinedMarginNames.BottomRightCorner)]
    [Order(After = PredefinedMarginNames.ChrMargin)]
    [ContentType(StandardContentTypeNames.Text)]
    [TextViewRole(PredefinedTextViewRoles.Zoomable)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [DeferCreation(OptionName = DefaultTextViewHostOptions.EditingStateMarginOptionName)]
    internal class SelectionMarginProvider : IWpfTextViewMarginProvider
    {
        [Import]
        public ITextDocumentFactoryService _documentService = null;

        public IWpfTextViewMargin CreateMargin(IWpfTextViewHost wpfTextViewHost, IWpfTextViewMargin marginContainer)
        {
            return new SelectionMargin(wpfTextViewHost.TextView);
        }
    }
}
