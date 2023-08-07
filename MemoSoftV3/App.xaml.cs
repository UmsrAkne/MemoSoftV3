using System.Windows;
using MemoSoftV3.ViewModels;
using MemoSoftV3.Views;
using Prism.Ioc;

namespace MemoSoftV3
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<GroupEditPage, GroupEditPageViewModel>();
            containerRegistry.RegisterDialog<TagEditPage, TagEditPageViewModel>();
        }
    }
}