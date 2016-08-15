using ArcGISWpf.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Windows;
using DevExpress.Xpf.Docking;

namespace ArcGISWpf.Service
{
    public class ConfigurationService : IConfigurationService
    {
        protected string Path = Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData );
        protected string MapPanelsFileName = "MapPanels.xml";
        protected string DockLayoutManagerFileName = "DockLayoutManager.xml";

        public void Refresh()
        {
            var file = System.IO.Path.Combine( Path, MapPanelsFileName );
            File.Delete( file );
            file = System.IO.Path.Combine( Path, DockLayoutManagerFileName );
            File.Delete( file );
        }

        public void SaveStateCommon( MainViewModel viewModel )
        {
            var file = System.IO.Path.Combine( Path, MapPanelsFileName );
            XmlSerializer serializer = new XmlSerializer( viewModel.GetType() );
            File.Delete(file);
            using( FileStream stream = File.Open( file, FileMode.CreateNew ) )
            {
                serializer.Serialize( stream, viewModel );
            }
        }

        public MainViewModel LoadStateCommon()
        {
            var file = System.IO.Path.Combine( Path, MapPanelsFileName );
            XmlSerializer serializer = new XmlSerializer( typeof( MainViewModel ) );
            if( File.Exists( file ) )
            {
                using( FileStream stream = File.Open( file, FileMode.Open ) )
                {
                    return (MainViewModel)serializer.Deserialize( stream );
                }
            }
            else
            {
                return null;
            }
        }

        public void SaveStateDockLayoutManager( )
        {
            var file = System.IO.Path.Combine( Path, DockLayoutManagerFileName );
            ((MainWindow)Application.Current.MainWindow).Manager.SaveLayoutToXml( file );
        }

        public void RestoreStateDockLayoutManager()
        {
            var file = System.IO.Path.Combine( Path, DockLayoutManagerFileName );
            if( File.Exists( file ) )
            {
                ((MainWindow)Application.Current.MainWindow).Manager.RestoreLayoutFromXml( file );
            }
        }

        public void Load(MainViewModel viewModel)
        {
            try
            {
                var restored = LoadStateCommon();
                restored.MapPanels.ToList().ForEach((n) =>
                {
                    viewModel.MapPanels.Add(n);
                    n.Load();
                });
                viewModel.CurrentTheme = restored.CurrentTheme;
                if (viewModel.MapPanels.Count == 0)
                {
                    var defaultPanel = new MapPanelViewModel(new MapViewModel() { Guid = Guid.NewGuid(), Name = "Карта" });
                    viewModel.MapPanels.Add(defaultPanel);
                }
                RestoreStateDockLayoutManager();
            }
            catch (Exception ex)
            {
                // TO:DO логирование
            }
        }
    }
}
