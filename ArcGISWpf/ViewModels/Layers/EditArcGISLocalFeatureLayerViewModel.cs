using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using ESRI.ArcGIS.Client.Local;
using Microsoft.Win32;

namespace ArcGISWpf.ViewModels.Layers
{
    public class EditArcGISLocalFeatureLayerViewModel :EditBaseLayerViewModel<ArcGISLocalFeatureLayerViewModel>
    {
        
        #region Properties
        private ObservableCollection<string> layersName;
        public ObservableCollection<string> LayersName
        {
            get
            {
                return layersName;
            }
            set
            {
                layersName = value;
                NotifyPropertyChanged( "LayersName" );
            }
        }
        #endregion

        public EditArcGISLocalFeatureLayerViewModel( ArcGISLocalFeatureLayerViewModel layer, DelegateCommand<BaseLayerViewModel> saveCommand )
          :base(layer,saveCommand)
        {
            this.GetLayersNameCommand.Execute( null );
        }

        #region Commands
        private DelegateCommand getLayersNameCommand;
        public DelegateCommand GetLayersNameCommand
        {
            get
            {
                if( getLayersNameCommand == null )
                {
                    getLayersNameCommand = new DelegateCommand( () =>
                    {
                        if( File.Exists( this.ViewModel.Path ) )
                        {
                            try
                            {
                                LocalMapService mapService = new LocalMapService(this.ViewModel.Path);
                                mapService.Start();
                                LayersName = new ObservableCollection<string>( mapService.MapLayers.Select( n => n.Name ).ToList() );
                                mapService.Stop();
                            }
                            catch( Exception ex )
                            {
                                // TO:DO логирование
                            }
                        }
                    } );
                }
                return getLayersNameCommand;
            }
        }


        public DelegateCommand OpenFileDialogCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    if (openFileDialog.ShowDialog() == true)
                    {
                        this.ViewModel.Path = openFileDialog.FileName;
                    }
                });
            }
        }
        #endregion

        public override void Validation()
        {
            StringBuilder errors = new StringBuilder();
            if( String.IsNullOrEmpty( this.ViewModel.Name ) )
            {
                errors.AppendLine( "Введите наименование" );
            }
            if( String.IsNullOrEmpty( this.ViewModel.LayerName) )
            {
                errors.AppendLine( "Введите(выберите из списка) имя слоя" );
            }
            if( !File.Exists(this.ViewModel.Path) )
            {
                errors.AppendLine( "Выберите файл mpk" );
            }
            this.Errors = errors.ToString();
        }

    }
}
