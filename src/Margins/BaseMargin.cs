using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Text.Editor;

namespace DocumentMargin.Margins
{
    internal abstract class BaseMargin : TextBlock, IWpfTextViewMargin
    {
        public abstract string MarginName { get; }

        public FrameworkElement VisualElement => this;

        public double MarginSize => ActualHeight;

        public bool Enabled => true;

        public abstract void Dispose();

        public ITextViewMargin GetTextViewMargin(string marginName)
        {
            return (marginName == MarginName) ? this : null;
        }
    }
}
