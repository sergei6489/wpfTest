using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcGISWpf.Ioc
{
   public static class IocService
    {
       private static IKernel kernel { get; set; }
       public static void InitializeIocService(params INinjectModule[] modules)
       {
           kernel = new StandardKernel(modules);
       }

       public static T Get<T>()
       {
           return kernel.Get<T>();
       }
    }
}
