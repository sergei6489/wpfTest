using ArcGISWpf.ViewModels.Layers;
using DevExpress.Xpf.Docking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArcGISWpf.Controls
{
    /// <summary>
    /// Логика взаимодействия для EditArcGISLocalTileLayerView.xaml
    /// </summary>
    public partial class EditArcGISLocalTileLayerView : LayoutPanel
    {
        public EditArcGISLocalTileLayerView(EditArcGISLocalTileLayerViewModel model)
        {
            InitializeComponent();
            this.DataContext = model;
        }
    }
}
