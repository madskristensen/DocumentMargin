global using Community.VisualStudio.Toolkit;

global using Microsoft.VisualStudio.Shell;

global using System;

global using Task = System.Threading.Tasks.Task;

using DocumentMargin.Commands;
using System.Runtime.InteropServices;
using System.Threading;

namespace DocumentMargin
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideService(typeof(EncodingMenuCommandBridge), IsAsyncQueryable = true)]
    [Guid(PackageGuids.DocumentMarginString)]
    public sealed class DocumentMarginPackage : ToolkitPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            AddService(
                typeof(EncodingMenuCommandBridge),
                (_, _, _) => Task.FromResult<object>(new EncodingMenuCommandBridge(this)),
                true
            );

            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await this.RegisterCommandsAsync();
        }
    }
}