using ArcGISWpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArcGISWpf.Service
{
    public interface IConfigurationService
    {
         void Refresh();
         void SaveStateDockLayoutManager( );
         void RestoreStateDockLayoutManager();
         void SaveStateCommon( MainViewModel viewModel );
         MainViewModel LoadStateCommon();
         void Load(MainViewModel viewModel);
    }
}
