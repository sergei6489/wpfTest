using ArcGISWpf.ViewModels;
using System.Windows;
using System.Windows.Controls;
using ESRI.ArcGIS.Client.Local;
using System.ComponentModel;
using DevExpress.Xpf.Docking;
using System.Linq;
using System;
using ESRI.ArcGIS.Client.Tasks;
using ESRI.ArcGIS.Client;
using System.Collections.Specialized;
using System.Windows.Data;
using ArcGISWpf.ViewModels.Layers;
using System.Collections;

namespace ArcGISWpf.Controls
{
    /// <summary>
    /// Interaction logic for MapLayoutPanel.xaml
    /// </summary>
    public partial class MapLayoutPanel : LayoutPanel
    {

        public MapPanelViewModel ViewModel { get; set; }

        public MapLayoutPanel( MapPanelViewModel viewModel )
        {
            InitializeComponent();
            MyDataGrid.Map = MyMap;
            this.Name = getByGuid( viewModel.BaseViewModel.Guid );
            this.ViewModel = viewModel;
            DataContext = ViewModel;
            LocalGeometryService.GetServiceAsync( serviceCallback =>
            {
                MyEditorWidget.GeometryServiceUrl = serviceCallback.UrlGeometryService;
            } );
            this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            this.ViewModel.ChangedProjection += ViewModel_ChangedProjection;
            viewModel.ChangingLayersCollectionEvent += viewModel_ChangingLayersCollectionEvent;
            // Применить проекцию
            this.ViewModel_ChangedProjection( this, new EventArgs() );
        }

        /// <summary>
        /// Обновить проекцию карты
        /// </summary>
        private void ViewModel_ChangedProjection( object sender, EventArgs e )
        {
            if( ViewModel.BaseViewModel.WKID.HasValue && ViewModel.BaseViewModel.X1.HasValue &&
             ViewModel.BaseViewModel.X2.HasValue &&
             ViewModel.BaseViewModel.Y1.HasValue &&
             ViewModel.BaseViewModel.Y2.HasValue )
            {
                if( ViewModel.BaseViewModel.WKID.HasValue )
                {
                    var temp = MyMap.Layers.ToList();
                    MyMap.Layers.Clear();
                    ESRI.ArcGIS.Client.Geometry.SpatialReference forcedSpatialReference = new ESRI.ArcGIS.Client.Geometry.SpatialReference( ViewModel.BaseViewModel.WKID.Value );
                    ESRI.ArcGIS.Client.Geometry.Envelope forcedEnvelope = new ESRI.ArcGIS.Client.Geometry.Envelope( ViewModel.BaseViewModel.X1.Value, ViewModel.BaseViewModel.Y1.Value, ViewModel.BaseViewModel.X2.Value, ViewModel.BaseViewModel.Y2.Value );
                    forcedEnvelope.SpatialReference = forcedSpatialReference;
                    MyMap.Extent = forcedEnvelope;
                    temp.ForEach( ( obj ) => MyMap.Layers.Add( obj ) );
                }
            }
        }

        private string getByGuid( Guid guid )
        {
            string result = String.Empty;
            string[] unique = new string[] { "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "a", "s", "d", "f", "g", "h", "j", "k", "l", "z", "x", "c", "v", "b", "n", "m" };
            string GuidString = Convert.ToBase64String( guid.ToByteArray() );
            foreach( var t in GuidString )
            {
                if( unique.Any( n => n.ToLower() == t.ToString().ToLower() ) )
                {
                    result += t.ToString();
                }
            }
            return result;
        }

        private void viewModel_ChangingLayersCollectionEvent( NotifyCollectionChangedAction type, IList list )
        {
            switch( type )
            {
                case NotifyCollectionChangedAction.Add:
                    ResetLayers();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach( var item in list )
                    {
                        RemoveLayerEvent( (BaseLayerViewModel)item );
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ResetLayers();
                    break;
            }
        }

        /// <summary>
        /// Обработка редактирования 
        /// </summary>
        private void ViewModel_PropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            if( e.PropertyName.Equals( "GuidEditingLayer" ) )
            {
                if( ViewModel.GuidEditingLayer == Guid.Empty )
                {
                    MyDataGrid.GraphicsLayer = null;
                    MyDataGrid.ItemsSource = null;
                }
                else
                {
                    MyDataGrid.GraphicsLayer = MyMap.Layers.FirstOrDefault( n => n.ID == ViewModel.GuidEditingLayer.ToString() ) as GraphicsLayer;
                }
            }
        }

        #region Обработка (появления,удаления,перемещение - слоя)
        #region Перемещение/вставка слоя
        public void ResetLayers()
        {
            Layer layer;
            int index;
            for( int n = 0; n < ViewModel.BaseViewModel.Layers.Count; n++ )
            {
                layer = MyMap.Layers.FirstOrDefault( m => m.ID == ViewModel.BaseViewModel.Layers[n].Guid.ToString() );
                //Вставка нового слоя
                if( layer == null )
                {
                    CreateLayer( ViewModel.BaseViewModel.Layers[n], n );
                }
            }
        }
        #endregion

        #region Вставка нового слоя
        private void CreateLayer( BaseLayerViewModel layer, int position )
        {
            try
            {
                if( MyMap.Layers[layer.Guid.ToString()] == null )
                {
                    if( layer.GetType() == typeof( ArcGISLocalFeatureLayerViewModel ) )
                    {
                        ArcGISLocalFeatureLayer layerView = new ArcGISLocalFeatureLayer( ((ArcGISLocalFeatureLayerViewModel)layer).Path, ((ArcGISLocalFeatureLayerViewModel)layer).LayerName );
                        BindVisibility( layerView, layer );
                        layerView.Editable = true;
                        layerView.OutFields = new OutFields() { "*" };
                        layerView.ID = layer.Guid.ToString();
                        AddLayer( layerView, position );
                        if( MyEditorWidget.LayerIDs != null )
                        {
                            var list = MyEditorWidget.LayerIDs.ToList();
                            list.Add( layer.Guid.ToString() );
                            MyEditorWidget.LayerIDs = list.ToArray();
                        }
                        else
                        {
                            MyEditorWidget.LayerIDs = new string[] { layerView.ID };
                        }
                    }
                    else if( layer.GetType() == typeof( ArcGISLocalTileLayerViewModel ) )
                    {
                        ArcGISLocalTiledLayer layerView = new ArcGISLocalTiledLayer( ((ArcGISLocalTileLayerViewModel)layer).Path );
                        layerView.ID = layer.Guid.ToString();
                        BindVisibility( layerView, layer );
                        AddLayer( layerView, position );
                    }
                    else if( layer.GetType() == typeof( ArcGISGlobalFeatureLayerViewModel ) )
                    {
                        FeatureLayer layerView = new FeatureLayer();
                        layerView.Url = ((ArcGISGlobalFeatureLayerViewModel)layer).Url;
                        BindVisibility( layerView, layer );
                        //layerView.Mode = FeatureLayer.QueryMode.OnDemand;
                        layerView.DisableClientCaching = true;
                        layerView.OutFields = new OutFields() { "*" };
                        layerView.ID = layer.Guid.ToString();
                        AddLayer( layerView, position );
                        if( MyEditorWidget.LayerIDs != null )
                        {
                            var list = MyEditorWidget.LayerIDs.ToList();
                            list.Add( layer.Guid.ToString() );
                            MyEditorWidget.LayerIDs = list.ToArray();
                        }
                        else
                        {
                            MyEditorWidget.LayerIDs = new string[] { layerView.ID };
                        }
                    }
                    else if( layer.GetType() == typeof( ArcGISGlobalTileLayerViewModel ) )
                    {
                        ArcGISTiledMapServiceLayer layerView = new ArcGISTiledMapServiceLayer();

                        layerView.Url = ((ArcGISGlobalTileLayerViewModel)layer).Url;
                        layerView.ID = layer.Guid.ToString();
                        BindVisibility( layerView, layer );
                        AddLayer( layerView, position );
                    }

                }
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.Message );
            }
        }

        private void BindVisibility( Layer layer, BaseLayerViewModel viewModel )
        {
            Binding bind = new Binding();
            bind.Source = viewModel;
            bind.Path = new PropertyPath( "IsVisible" );
            bind.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding( layer, Layer.VisibleProperty, bind );
        }

        #endregion

        /// <summary>
        /// Удаление слоя
        /// </summary>
        private void RemoveLayerEvent( BaseLayerViewModel layer )
        {
            var arcLayer = MyMap.Layers[layer.Guid.ToString()];
            BindingOperations.ClearAllBindings( arcLayer );
            MyMap.Layers.Remove( arcLayer );
            if( layer.GetType() == typeof( ArcGISLocalFeatureLayerViewModel ) || layer.GetType() == typeof( ArcGISGlobalFeatureLayerViewModel ) )
            {
                if( MyEditorWidget.LayerIDs != null )
                {
                    var list = MyEditorWidget.LayerIDs.ToList();
                    list.Remove( layer.Guid.ToString() );
                    MyEditorWidget.LayerIDs = list.ToArray();
                }
            }
        }
        #endregion

        #region Вспомогательные
        /// <summary>
        /// Вставка слоя в позицию
        /// </summary>
        /// <param name="layer">Слой</param>
        /// <param name="position"> позиция для вставки</param>
        private void AddLayer( Layer layer, int position )
        {
            MyMap.Layers.Insert( position, layer );
        }
        #endregion
    }
}
