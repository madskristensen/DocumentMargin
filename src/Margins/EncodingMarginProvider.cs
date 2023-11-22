using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace DocumentMargin.Margin
{
    [Export(typeof(IWpfTextViewMarginProvider))]
    [Name(EncodingMargin.MarginName)]
    [MarginContainer(PredefinedMarginNames.BottomRightCorner)]
    [Order(After = PredefinedMarginNames.LineEndingMargin)]
    [ContentType(StandardContentTypeNames.Text)]
    [TextViewRole(PredefinedTextViewRoles.Zoomable)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [DeferCreation(OptionName = DefaultTextViewHostOptions.EditingStateMarginOptionName)]
    internal class EncodingMarginProvider : IWpfTextViewMarginProvider
    {
        [Import]
        public ITextDocumentFactoryService _documentService = null;

        public IWpfTextViewMargin CreateMargin(IWpfTextViewHost wpfTextViewHost, IWpfTextViewMargin marginContainer)
        {
            ITextDocument doc = wpfTextViewHost.TextView.TextBuffer.GetTextDocument();

            return doc != null ? new EncodingMargin(doc) : null;
        }
    }
}
