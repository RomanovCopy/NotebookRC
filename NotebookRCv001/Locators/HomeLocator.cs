using Autofac;

using NotebookRCv001.MyControls;
using NotebookRCv001.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookRCv001.Locators
{
    public class HomeLocator
    {
        public HomeViewModel homeViewModel => App.container.Resolve<HomeViewModel>();
        public MenuHome menuHome => App.container.Resolve<MenuHome>();
        public MenuHomeViewModel menuHomeViewModel => App.container.Resolve<MenuHomeViewModel>();
        public RichTextBox richTextBox => App.container.Resolve<RichTextBox>();
        ButtonsClearAndClose buttonsClearAndClose => App.container.Resolve<ButtonsClearAndClose>();
    }
}
