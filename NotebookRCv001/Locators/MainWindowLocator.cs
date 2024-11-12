using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using NotebookRCv001.Converters;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.MyControls;
using NotebookRCv001.MyControls.HomeSideMenu;
using NotebookRCv001.ViewModels;

namespace NotebookRCv001.Locators
{
    public class MainWindowLocator
    {
        public MainWindowViewModel mainWindowViewModel => App.container.Resolve<MainWindowViewModel>();

        public SideMenu sideMenu => App.container.Resolve<SideMenu>();

        public ToolBarStatus toolBarStatus => App.container.Resolve<ToolBarStatus>();
        public static SizeLocationConverter sizeLocationConverter => App.container.Resolve<SizeLocationConverter>();


    }
}
