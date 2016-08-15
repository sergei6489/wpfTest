using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcGISWpf.ViewModels.Layers
{
    public class ArcGISLocalFeatureLayerViewModel : BaseLayerViewModel
    {
        #region Properties
        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                NotifyPropertyChanged( "Path" );
            }
        }

        private string layerName;
        public string LayerName
        {
            get { return layerName; }
            set
            {
                layerName = value;
                NotifyPropertyChanged( "LayerName" );
            }
        }
        #endregion

        public override object Clone()
        {
            ArcGISLocalFeatureLayerViewModel data = new ArcGISLocalFeatureLayerViewModel();
            data.Guid = this.Guid;
            data.IsEdit = this.IsEdit;
            data.IsVisible = this.IsVisible;
            data.Name = this.Name;
            data.Path = this.Path;
            data.LayerName = this.LayerName;
            return data;
        }

        public override void SaveChanges(BaseLayerViewModel obj)
        {
            ArcGISLocalFeatureLayerViewModel data = (ArcGISLocalFeatureLayerViewModel) obj;
            this.Guid = data.Guid;
            this.IsEdit = data.IsEdit;
            this.IsVisible = data.IsVisible;
            this.Name = data.Name;
            this.Path = data.Path;
            this.LayerName = data.LayerName;
        }
    }
}
