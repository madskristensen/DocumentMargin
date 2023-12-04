using System.ComponentModel.Design;
using Microsoft.VisualStudio.Text;

namespace DocumentMargin.Commands
{
    internal class EncodingMenuCommandBridge
    {
        private readonly IAsyncServiceProvider _serviceProvider;

        public EncodingMenuCommandBridge(IAsyncServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ITextDocument CurrentDocument { get; private set; }

        public async Task ShowAsync(ITextDocument document, int x, int y)
        {
            CurrentDocument = document;

            try
            {
                IMenuCommandService commandService = await _serviceProvider.GetServiceAsync<IMenuCommandService, IMenuCommandService>();
                commandService.ShowContextMenu(new CommandID(PackageGuids.DocumentMargin, PackageIds.EncodingMenu), x, y);
            }
            finally
            {
                CurrentDocument = null;
            }
        }
    }
}
