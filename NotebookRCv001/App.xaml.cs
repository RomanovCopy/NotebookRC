using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Autofac;

using NotebookRCv001.DI;
using NotebookRCv001.Views;

namespace NotebookRCv001
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IContainer  container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var startup = new Startup();
            container = (IContainer)startup.ConfigureServices();

            // Разрешение и запуск главного окна
            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }
    }
}
