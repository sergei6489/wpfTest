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
    /// Логика взаимодействия для EditArcGISGlobalFeatureLayerView.xaml
    /// </summary>
    public partial class EditArcGISGlobalFeatureLayerView : LayoutPanel
    {
        public EditArcGISGlobalFeatureLayerView(EditArcGISGlobalFeatureLayerViewModel model)
        {
            InitializeComponent();
            this.DataContext = model;
        }
    }
}
