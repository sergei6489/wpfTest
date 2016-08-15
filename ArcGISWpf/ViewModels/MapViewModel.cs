using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGISWpf.ViewModels.Layers;

namespace ArcGISWpf.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        #region Properties
        private Guid guid;
        public Guid Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                NotifyPropertyChanged( "Name" );
            }
        }

        private int? wkid;
        public int? WKID
        {
            get
            {
                return wkid;
            }
            set
            {
                wkid = value;
                NotifyPropertyChanged( "WKID" );
            }
        }

        private int? x1;
        public int? X1
        {
            get
            {
                return x1;
            }
            set
            {
                x1 = value;
                NotifyPropertyChanged("X1");
            }
        }

        private int? x2;
        public int? X2
        {
            get
            {
                return x2;
            }
            set
            {
                x2 = value;
                NotifyPropertyChanged( "X2" );
            }
        }

        private int? y1;
        public int? Y1
        {
            get
            {
                return y1;
            }
            set
            {
                y1 = value;
                NotifyPropertyChanged( "Y1" );
            }
        }

        private int? y2;
        public int? Y2
        {
            get
            {
                return y2;
            }
            set
            {
                y2 = value;
                NotifyPropertyChanged( "Y2" );
            }
        }

        private ObservableCollection<BaseLayerViewModel> layers = new ObservableCollection<BaseLayerViewModel>();
        public ObservableCollection<BaseLayerViewModel> Layers
        {
            get
            {
                return layers;
            }
            set
            {
                layers = value;
                NotifyPropertyChanged( "Layers" );
            }
        #endregion
        }
    }
}
