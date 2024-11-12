using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using System.Windows;
using NotebookRCv001.Views;
using NotebookRCv001.ViewModels;
using NotebookRCv001.Converters;
using NotebookRCv001.MyControls.HomeSideMenu;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.MyControls;

namespace NotebookRCv001.DI
{
    public class Startup
    {
        public IContainer ConfigureServices()
        {
            var builder = new ContainerBuilder();

            //регистрация главного окна
            builder.RegisterType<MainWindow>().SingleInstance();

            //регистрация страниц
            builder.RegisterType<MainWindowViewModel>().SingleInstance();
            builder.RegisterType<SizeLocationConverter>().SingleInstance();
            builder.RegisterType<Languages>().SingleInstance();
            builder.RegisterType<SideMenu>().SingleInstance();
            builder.RegisterType<ToolBarStatus>().SingleInstance();

            builder.RegisterType<Home>().SingleInstance();
            builder.RegisterType<HomeViewModel>().SingleInstance();
            builder.RegisterType<MenuHome>().SingleInstance();
            builder.RegisterType<MenuHomeViewModel>().SingleInstance();
            builder.RegisterType<ButtonsClearAndClose>().SingleInstance();
            builder.RegisterType<RichTextBox>().SingleInstance();

            return builder.Build();
        }
    }
}
