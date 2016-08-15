using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcGISWpf.ViewModels.Layers
{
    public class ArcGISGlobalFeatureLayerViewModel : BaseLayerViewModel
    {
        #region Properties
        private string url;
        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                NotifyPropertyChanged( "Url" );
            }
        }

        #endregion

        public override object Clone()
        {
            ArcGISGlobalFeatureLayerViewModel data = new ArcGISGlobalFeatureLayerViewModel();
            data.Guid = this.Guid;
            data.IsEdit = this.IsEdit;
            data.IsVisible = this.IsVisible;
            data.Name = this.Name;
            data.Url = this.Url;
            return data;
        }

        public override void SaveChanges(BaseLayerViewModel obj)
        {
            ArcGISGlobalFeatureLayerViewModel data = (ArcGISGlobalFeatureLayerViewModel)obj;
            this.Guid = data.Guid;
            this.IsEdit = data.IsEdit;
            this.IsVisible = data.IsVisible;
            this.Name = data.Name;
            this.Url = data.Url;
        }
    }
}
