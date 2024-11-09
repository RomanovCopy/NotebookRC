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

namespace NotebookRCv001.DI
{
    public class Startup
    {
        public IContainer ConfigureServices()
        {
            var builder = new ContainerBuilder();

            //регистрация главного окна
            builder.RegisterType<MainWindow>().AsSelf();

            //регистрация страниц
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            builder.RegisterType<Home>().AsSelf();
            builder.RegisterType<HomeViewModel>().AsSelf();

            return builder.Build();
        }
    }
}
