using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;

namespace DocumentMargin.Commands
{
    internal class EncodingMenuCommandBridge
    {
        private const uint _showOptions = (uint)(__VSSHOWCONTEXTMENUOPTS2.VSCTXMENU_PLACETOP | __VSSHOWCONTEXTMENUOPTS2.VSCTXMENU_RIGHTALIGN);

        public ITextDocument CurrentDocument { get; private set; }

        public async Task ShowAsync(ITextDocument document, int x, int y)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            CurrentDocument = document;

            try
            {
                IVsUIShell shell = await VS.Services.GetUIShellAsync();
                POINTS[] locationPoints = new[] { new POINTS() { x = (short)x, y = (short)y } };
                _ = shell.ShowContextMenu(_showOptions, PackageGuids.DocumentMargin, PackageIds.EncodingMenu, locationPoints, pCmdTrgtActive: null);

                Telemetry.TrackUserTask("openencodingmenu");
            }
            finally
            {
                CurrentDocument = null;
            }
        }
    }
}
