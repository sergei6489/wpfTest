using ArcGISWpf.Service;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArcGISWpf.ViewModels;

namespace ArcGISWpf.Ioc
{
    public class CustomModule : NinjectModule
    {

        public override void Load()
        {
            Bind<IConfigurationService>().To<ConfigurationService>();
            Bind<MainViewModel>().To<MainViewModel>().InSingletonScope();
        }
    }
}
