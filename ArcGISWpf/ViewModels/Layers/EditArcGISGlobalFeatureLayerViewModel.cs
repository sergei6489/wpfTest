using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using ESRI.ArcGIS.Client.Local;

namespace ArcGISWpf.ViewModels.Layers
{
    public class EditArcGISGlobalFeatureLayerViewModel : EditBaseLayerViewModel<ArcGISGlobalFeatureLayerViewModel>
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

        public EditArcGISGlobalFeatureLayerViewModel( ArcGISGlobalFeatureLayerViewModel layer, DelegateCommand<BaseLayerViewModel> saveCommand )
          :base(layer,saveCommand)
        {
        }

        #region Commands
        #endregion

        public override void Validation()
        {
            StringBuilder errors = new StringBuilder();
            if( String.IsNullOrEmpty( this.ViewModel.Name ) )
            {
                errors.AppendLine( "Введите наименование" );
            }
            if( !RemoteFileExists( this.ViewModel.Url) )
            {
                errors.AppendLine( "Указанный url недоступен" );
            }
            this.Errors = errors.ToString();
        }


        private bool RemoteFileExists( string url )
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create( url ) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }

    }
}
