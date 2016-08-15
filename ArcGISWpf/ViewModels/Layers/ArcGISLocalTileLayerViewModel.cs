using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcGISWpf.ViewModels.Layers
{
    public class ArcGISLocalTileLayerViewModel : BaseLayerViewModel
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
        #endregion

        public override object Clone()
        {
            ArcGISLocalTileLayerViewModel data = new ArcGISLocalTileLayerViewModel();
            data.Guid = this.Guid;
            data.IsEdit = this.IsEdit;
            data.IsVisible = this.IsVisible;
            data.Name = this.Name;
            data.Path = this.Path;
            return data;
        }

        public override void SaveChanges(BaseLayerViewModel obj)
        {
            ArcGISLocalTileLayerViewModel data = (ArcGISLocalTileLayerViewModel) obj;
            this.Guid = data.Guid;
            this.IsEdit = data.IsEdit;
            this.IsVisible = data.IsVisible;
            this.Name = data.Name;
            this.Path = data.Path;
        }
    }
}
