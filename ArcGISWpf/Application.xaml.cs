
using ArcGISWpf.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace ArcGISWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IocService.InitializeIocService(new CustomModule());
            var mainWindow = IocService.Get<MainWindow>();
            mainWindow.Show();
        }
    }
}
