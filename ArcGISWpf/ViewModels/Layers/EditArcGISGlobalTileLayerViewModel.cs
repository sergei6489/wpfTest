using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArcGISWpf.ViewModels.Layers
{
    public class EditArcGISGlobalTileLayerViewModel: EditBaseLayerViewModel<ArcGISGlobalTileLayerViewModel>
    {
        #region Properties
        private string layerName;
        public string LayerName
        {
            get
            {
                return layerName;
            }
            set
            {
                layerName = value;
                NotifyPropertyChanged("LayerName");
            }
        }

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
                NotifyPropertyChanged("LayersName");
            }
        }
        #endregion

        public EditArcGISGlobalTileLayerViewModel(ArcGISGlobalTileLayerViewModel layer, DelegateCommand<BaseLayerViewModel> saveCommand)
          :base(layer,saveCommand)
        {
        }

        #region Commands

        #endregion

        public override void Validation()
        {
            StringBuilder errors = new StringBuilder();
            if (String.IsNullOrEmpty(this.ViewModel.Name))
            {
                errors.AppendLine("Введите наименование");
            }

            if (!RemoteFileExists(this.ViewModel.Url))
            {
                errors.AppendLine("Указанный url недоступен");
            }
            this.Errors = errors.ToString();
        }

        private bool RemoteFileExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return true;
            }
            catch(Exception ex)
            {
                //Any exception will returns false.
                return false;
            }
        }
    }
}

