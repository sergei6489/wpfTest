using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using ArcGISWpf.Ioc;
using ArcGISWpf.ViewModels.Layers;
using DevExpress.Mvvm;

namespace ArcGISWpf.ViewModels
{
    public class MapPanelViewModel : BaseViewModel
    {
        #region Events
        public delegate void changingLayersCollection( NotifyCollectionChangedAction type, IList list );
        public event changingLayersCollection ChangingLayersCollectionEvent;

        public event EventHandler ChangedProjection;

        public void ChangedProjectionProc()
        {
            if( ChangedProjection != null )
            {
                ChangedProjection( this, new EventArgs() );
            }
        }
        #endregion

        #region Property
        private MapViewModel baseViewModel;
        public MapViewModel BaseViewModel
        {
            get
            {
                return baseViewModel;
            }
            set
            {
                baseViewModel = value;
                NotifyPropertyChanged( "BaseViewModel" );
            }
        }

        private bool isClosed;
        public bool IsClosed
        {
            get
            {
                return isClosed;
            }
            set
            {
                isClosed = value;
                NotifyPropertyChanged( "IsClosed" );
            }
        }

        private Guid guidEditingLayer;
        public Guid GuidEditingLayer
        {
            get
            {
                return guidEditingLayer;
            }
            set
            {
                guidEditingLayer = value;
                NotifyPropertyChanged( "GuidEditingLayer" );
            }
        }

        #endregion

        #region Constructors
        public MapPanelViewModel( MapViewModel viewModel )
        {
            this.BaseViewModel = viewModel;
            Load();
        }

        public MapPanelViewModel()
        {
        }

        public void Load()
        {
            this.BaseViewModel.Layers.CollectionChanged += Layers_CollectionChanged;
            this.GuidEditingLayer = this.GuidEditingLayer;
        }

        private void Layers_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
        {
            if( e.Action == NotifyCollectionChangedAction.Remove )
            {
                // Вызвать событие изменение списка
                if( this.ChangingLayersCollectionEvent != null )
                {
                    ChangingLayersCollectionEvent( e.Action, e.OldItems );
                }
                // Убрать выбранный слой для показа семантики, отписаться от события изменения свойств слоя
                foreach( var obj in e.OldItems )
                {
                    if( GuidEditingLayer == ((BaseLayerViewModel)obj).Guid )
                    {
                        GuidEditingLayer = Guid.Empty;
                    }
                }
            }
            else if( e.Action == NotifyCollectionChangedAction.Add )
            {
                if( this.ChangingLayersCollectionEvent != null )
                {
                    ChangingLayersCollectionEvent( e.Action, e.NewItems );
                }
            }

        }

        #endregion

        #region Commands

        private DelegateCommand editMapPanelViewCommand;
        /// <summary>
        /// показать форму редактирования карты
        /// </summary>
        public DelegateCommand EditMapPanelViewCommand
        {
            get
            {
                if( editMapPanelViewCommand == null )
                {
                    editMapPanelViewCommand = new DelegateCommand( () =>
                    {
                        var mainViewModel = IocService.Get<MainViewModel>();
                        EditMapViewModel map = new EditMapViewModel( this, null );
                        mainViewModel.AddMapEventProc( map );
                    } );
                }
                return editMapPanelViewCommand;
            }
        }

        #region Добавление слоя(показать форму создания)
        private DelegateCommand addArcGISGlobalFeatureLayerViewCommand;
        /// <summary>
        /// Показать форму создания слоя "глобальные фигурки"
        /// </summary>
        public DelegateCommand AddArcGISGlobalFeatureLayerViewCommand
        {
            get
            {
                if( addArcGISGlobalFeatureLayerViewCommand == null )
                {
                    addArcGISGlobalFeatureLayerViewCommand = new DelegateCommand( () =>
                     {
                         ArcGISGlobalFeatureLayerViewModel data = new ArcGISGlobalFeatureLayerViewModel();
                         data.Guid = Guid.NewGuid();
                         data.Name = "Новый глобальный графический слой";
                         EditArcGISGlobalFeatureLayerViewModel model = new EditArcGISGlobalFeatureLayerViewModel( data, this.AddLayerCommand );
                         var mainViewModel = IocService.Get<MainViewModel>();
                         mainViewModel.AddEditLayerEventProc( model );
                     } );
                }
                return addArcGISGlobalFeatureLayerViewCommand;
            }
        }

        private DelegateCommand addArcGISGlobalTileLayerViewCommand;
        /// <summary>
        /// Показать форму создания слоя "глобальные тайлы"
        /// </summary>
        public DelegateCommand AddArcGISGlobalTileLayerViewCommand
        {
            get
            {
                if( addArcGISGlobalTileLayerViewCommand == null )
                {
                    addArcGISGlobalTileLayerViewCommand = new DelegateCommand( () =>
                    {
                        ArcGISGlobalTileLayerViewModel data = new ArcGISGlobalTileLayerViewModel();
                        data.Guid = Guid.NewGuid();
                        data.Name = "Новый глобальный тайловый слой";
                        EditArcGISGlobalTileLayerViewModel model = new EditArcGISGlobalTileLayerViewModel( data, this.AddLayerCommand );
                        var mainViewModel = IocService.Get<MainViewModel>();
                        mainViewModel.AddEditLayerEventProc( model );
                    } );
                }
                return addArcGISGlobalTileLayerViewCommand;
            }
        }

        private DelegateCommand addArcGISLocalFeatureLayerViewCommand;
        /// <summary>
        /// Показать форму создания слоя "глобальные фигурки"
        /// </summary>
        public DelegateCommand AddArcGISLocalFeatureLayerViewCommand
        {
            get
            {
                if( addArcGISLocalFeatureLayerViewCommand == null )
                {
                    addArcGISLocalFeatureLayerViewCommand = new DelegateCommand( () =>
                    {
                        ArcGISLocalFeatureLayerViewModel data = new ArcGISLocalFeatureLayerViewModel();
                        data.Guid = Guid.NewGuid();
                        data.Name = "Новый локальный графический слой";
                        EditArcGISLocalFeatureLayerViewModel model = new EditArcGISLocalFeatureLayerViewModel( data, this.AddLayerCommand );
                        var mainViewModel = IocService.Get<MainViewModel>();
                        mainViewModel.AddEditLayerEventProc( model );
                    } );
                }
                return addArcGISLocalFeatureLayerViewCommand;
            }
        }

        private DelegateCommand addArcGISLocalTileLayerViewCommand;
        /// <summary>
        /// Показать форму создания слоя "локальные тайлы"
        /// </summary>
        public DelegateCommand AddArcGISLocalTileLayerViewCommand
        {
            get
            {
                if( addArcGISLocalTileLayerViewCommand == null )
                {
                    addArcGISLocalTileLayerViewCommand = new DelegateCommand( () =>
                    {
                        ArcGISLocalTileLayerViewModel data = new ArcGISLocalTileLayerViewModel();
                        data.Guid = Guid.NewGuid();
                        data.Name = "Новый локальный тайловый слой";
                        EditArcGISLocalTileLayerViewModel model = new EditArcGISLocalTileLayerViewModel( data, this.AddLayerCommand );
                        var mainViewModel = IocService.Get<MainViewModel>();
                        mainViewModel.AddEditLayerEventProc( model );
                    } );
                }
                return addArcGISLocalTileLayerViewCommand;
            }
        }

        #endregion

        /// <summary>
        /// Добавить слой в коллекцию
        /// </summary>
        private DelegateCommand<BaseLayerViewModel> addLayerCommand;
        public DelegateCommand<BaseLayerViewModel> AddLayerCommand
        {
            get
            {
                if( addLayerCommand == null )
                {
                    addLayerCommand = new DelegateCommand<BaseLayerViewModel>( ( data ) =>
                        {
                            this.BaseViewModel.Layers.Add( data );
                        } );
                }
                return addLayerCommand;
            }
        }

        /// <summary>
        /// После сохранения слоя вызывается эта команда
        /// </summary>
        private DelegateCommand<BaseLayerViewModel> editionLayerSaved;
        public DelegateCommand<BaseLayerViewModel> EditionLayerSaved
        {
            get
            {
                if( editionLayerSaved == null )
                {
                    editionLayerSaved = new DelegateCommand<BaseLayerViewModel>( ( obj ) =>
                        {
                            var data = this.BaseViewModel.Layers.FirstOrDefault( m => m.Guid == obj.Guid );
                            int oldIndex = this.BaseViewModel.Layers.IndexOf( data );
                            this.BaseViewModel.Layers.Remove( data );
                            if( this.GuidEditingLayer == obj.Guid )
                            {
                                this.GuidEditingLayer = Guid.Empty;
                            }
                            this.BaseViewModel.Layers.Insert( oldIndex, data );

                        } );
                }
                return editionLayerSaved;
            }
        }

        /// <summary>
        /// Удалить слой
        /// </summary>
        private DelegateCommand<BaseLayerViewModel> removeLayerViewCommand;
        public DelegateCommand<BaseLayerViewModel> RemoveLayerViewCommand
        {
            get
            {
                if( removeLayerViewCommand == null )
                {
                    removeLayerViewCommand = new DelegateCommand<BaseLayerViewModel>( ( obj ) =>
                    {
                        this.BaseViewModel.Layers.Remove( obj );
                    } );
                }
                return removeLayerViewCommand;
            }
        }

        /// <summary>
        /// Редактировать слой в гриде
        /// </summary>
        private DelegateCommand<BaseLayerViewModel> editLayerViewCommand;
        public DelegateCommand<BaseLayerViewModel> EditLayerViewCommand
        {
            get
            {
                if( editLayerViewCommand == null )
                {
                    editLayerViewCommand = new DelegateCommand<BaseLayerViewModel>( ( layer ) =>
                        {
                            if( layer.GetType() == typeof( ArcGISLocalFeatureLayerViewModel ) || layer.GetType() == typeof( ArcGISGlobalFeatureLayerViewModel ) )
                            {
                                if( this.GuidEditingLayer == layer.Guid )
                                {
                                    this.GuidEditingLayer = Guid.Empty;
                                }
                                else
                                {
                                    this.GuidEditingLayer = layer.Guid;
                                }
                            }
                        } );
                }
                return editLayerViewCommand;
            }
        }
        #endregion
    }
}
