using ArcGISWpf.Service;
using ArcGISWpf.ViewModels;
using System.Collections.Generic;
using System.Windows;
using ArcGISWpf.Controls;
using System;
using System.Linq;
using ArcGISWpf.Ioc;
using DevExpress.Xpf.Grid;
using System.Collections.Specialized;
using ArcGISWpf.ViewModels.Layers;
using DevExpress.Xpf.Docking;

namespace ArcGISWpf
{

    public partial class MainWindow : Window
    {
        public MainViewModel mainViewModel { get; set; }
        public IConfigurationService service;

        public MainWindow(IConfigurationService service)
        {
            InitializeComponent();
             
            this.service = service;
            mainViewModel = IocService.Get<MainViewModel>();
            mainViewModel.AddEditLayerEvent += mainViewModel_AddLayerEvent;
            mainViewModel.AddMapEvent += mainViewModel_AddMapEvent;
            mainViewModel.MapPanels.CollectionChanged+=MapPanels_CollectionChanged;
            service.Load(mainViewModel);
            this.DataContext = mainViewModel;
        }


        /// <summary>
        /// Изменение в коллекции карт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapPanels_CollectionChanged( object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e )
        {
           switch (e.Action)
           {
               case NotifyCollectionChangedAction.Add:
                   foreach( var item in e.NewItems )
                   {
                       var panel = new MapLayoutPanel( (MapPanelViewModel)item );
                       this.DocumentGroups.Add( panel );
                       if( ((MapPanelViewModel)item).BaseViewModel.Layers.Count > 0 )
                       {
                           panel.ResetLayers();
                       }
                   }
                   break;
               case NotifyCollectionChangedAction.Remove:
                   foreach( var item in e.OldItems )
                   {
                       this.DocumentGroups.Remove( this.DocumentGroups.Items.FirstOrDefault( n => n.Name == ((MapPanelViewModel)item).BaseViewModel.Name.ToString().Replace(" ","") ) );
                   }
                   break;
           }
        }

        #region Открытие панелей создания/редактирования карт,слоев
        /// <summary>
        /// Открыть панель редактирования/создания новой карты
        /// </summary>
        /// <param name="maplayer"></param>
        private void mainViewModel_AddMapEvent( EditMapViewModel maplayer )
        {
            DocumentGroups.Add(new EditMapPanelView(maplayer));
        }

        /// <summary>
        /// Открыть новую панель редактирования/создания слоя
        /// </summary>
        /// <param name="layer"></param>
        private void mainViewModel_AddLayerEvent( object layer )
        {
            if (layer.GetType() == typeof(EditArcGISGlobalFeatureLayerViewModel))
            {
                AddPanel(new EditArcGISGlobalFeatureLayerView((EditArcGISGlobalFeatureLayerViewModel) layer));
            }
            else if (layer.GetType() == typeof(EditArcGISGlobalTileLayerViewModel))
            {
                AddPanel(new EditArcGISGlobalTileLayerView((EditArcGISGlobalTileLayerViewModel)layer));
            }
            else if (layer.GetType() == typeof(EditArcGISLocalFeatureLayerViewModel))
            {
                AddPanel(new EditArcGISLocalFeatureLayerView((EditArcGISLocalFeatureLayerViewModel)layer));
            }
            else if (layer.GetType() == typeof(EditArcGISLocalTileLayerViewModel))
            {
                AddPanel(new EditArcGISLocalTileLayerView((EditArcGISLocalTileLayerViewModel)layer));
            }
        }

        private void AddPanel(LayoutPanel panel )
        {
            DocumentGroups.Add(panel);
            panel.IsActive = true;
        }
        #endregion

        #region Drag n Drop
        private void TreeListDragDropManager_Drop( object sender, DevExpress.Xpf.Grid.DragDrop.TreeListDropEventArgs e )
        {       
            TreeListView view = e.Manager.View as TreeListView;
            var node = view.GetNodeByRowHandle( e.HitInfo.RowHandle );
            if( node.ActualLevel != 1 && e.DropTargetType != DropTargetType.InsertRowsIntoNode || (node.ActualLevel == 1 && e.DropTargetType == DropTargetType.InsertRowsIntoNode) )
            {
                e.Handled = true;
            }
        }


        private void TreeListDragDropManager_StartDrag( object sender, DevExpress.Xpf.Grid.DragDrop.TreeListStartDragEventArgs e )
        {
            if( e.Node.Level == 0 )
            {
                e.CanDrag = false;
            }
        }
        #endregion
    }
}
