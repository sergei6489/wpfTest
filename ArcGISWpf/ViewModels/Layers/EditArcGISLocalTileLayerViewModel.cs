using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using ESRI.ArcGIS.Client.Local;
using Microsoft.Win32;

namespace ArcGISWpf.ViewModels.Layers
{
    public class EditArcGISLocalTileLayerViewModel:EditBaseLayerViewModel<ArcGISLocalTileLayerViewModel>
    {
        
        #region Properties
        #endregion

        public EditArcGISLocalTileLayerViewModel( ArcGISLocalTileLayerViewModel layer, DelegateCommand<BaseLayerViewModel> saveCommand )
          :base(layer,saveCommand)
        {
            BaseViewModel = layer;
        }

        public override void Validation()
        {
            StringBuilder errors = new StringBuilder();
            if( String.IsNullOrEmpty(this.ViewModel.Name) )
            {
                errors.AppendLine( "Введите наименование" );
            }
            if( !File.Exists( this.ViewModel.Path ) )
            {
                errors.AppendLine( "Выберите файл tpk" );
            }
            this.Errors = errors.ToString();
        }

        #region Commands
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
    }
}
