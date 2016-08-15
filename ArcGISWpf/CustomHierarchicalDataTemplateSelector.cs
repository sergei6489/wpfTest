using ArcGISWpf.ViewModels;
using DevExpress.Xpf.Grid.TreeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ArcGISWpf.ViewModels.Layers;

namespace ArcGISWpf
{
    public class CustomHierarchicalDataTemplateSelector : DataTemplateSelector
    {

        public HierarchicalDataTemplate MapDataTemplate { get; set; }
        public HierarchicalDataTemplate ArcGISLocalFeatureTemplate { get; set; }
        public HierarchicalDataTemplate ArcGISGlobalFeatureTemplate { get; set; }
        public HierarchicalDataTemplate ArcGISLocalTileTemplate { get; set; }
        public HierarchicalDataTemplate ArcGISGlobalTileTemplate { get; set; }

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var rowData = item as TreeListRowData;
            var element = rowData.Row;
            if (element is MapPanelViewModel)
                return MapDataTemplate;
            if( element.GetType() == typeof( ArcGISLocalFeatureLayerViewModel ) )
                return ArcGISLocalFeatureTemplate;
            if (element.GetType() == typeof(ArcGISGlobalFeatureLayerViewModel))
                return ArcGISGlobalFeatureTemplate;
            if( element.GetType() == typeof( ArcGISLocalTileLayerViewModel ) )
                return ArcGISLocalTileTemplate;
            if( element.GetType() == typeof( ArcGISGlobalTileLayerViewModel ) )
                return ArcGISGlobalTileTemplate;
            return null;
        }
    }
}
