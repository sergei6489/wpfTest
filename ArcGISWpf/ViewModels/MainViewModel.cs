using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ArcGISWpf.Service;
using ArcGISWpf.Ioc;
using ArcGISWpf.ViewModels.Layers;
using DevExpress.Xpf.Core;

namespace ArcGISWpf.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region События
        public delegate void AddEditLayerDelegate( object layer );
        /// <summary>
        /// Уведомление о новом слое
        /// </summary>
        public event AddEditLayerDelegate AddEditLayerEvent;

        public delegate void AddLayerMapDelegate( EditMapViewModel maplayer );
        /// <summary>
        /// Уведомление о новой карте
        /// </summary>
        public event AddLayerMapDelegate AddMapEvent;

        /// <summary>
        /// Вызвать событие добавление нового слоя к карте
        /// </summary>
        /// <param name="layer"></param>
        public void AddEditLayerEventProc( object layer )
        {
            if( AddEditLayerEvent != null )
            {
                AddEditLayerEvent( layer );
            }
        }

        /// <summary>
        /// Вызвать событие добавление  новой карты
        /// </summary>
        /// <param name="map"></param>
        public void AddMapEventProc( EditMapViewModel map )
        {
            if( AddMapEvent != null )
            {
                AddMapEvent( map );
            }
        }
        #endregion
        IConfigurationService service { get; set; }

        private string currentTheme;
        public string CurrentTheme
        {
            get
            {
                return currentTheme;
            }
            set
            {
                ThemeManager.ApplicationThemeName = value;
                currentTheme = value;
                NotifyPropertyChanged( "CurrentTheme" );
            }
        }

        private ObservableCollection<string> themeList;
        public ObservableCollection<string> ThemeList
        {
            get { return themeList; }
            set
            {
                themeList = value;
                NotifyPropertyChanged( "ThemeList" );
            }
        }


        private ObservableCollection<MapPanelViewModel> mapPanels;
        public ObservableCollection<MapPanelViewModel> MapPanels
        {
            get
            {
                return mapPanels;
            }
            set
            {
                mapPanels = value;
                NotifyPropertyChanged( "MapPanels" );
            }
        }

        public MainViewModel()
        {
            themeList = new ObservableCollection<string>() { "DeepBlue", "DXStyle", "LightGray", "MetropolisDark", "MetropolisLight",
                "Office2007Black", "Office2007Blue", "Office2007Silver", "Office2010Black", "Office2010Blue",
                "Office2010Silver", "Office2013","Office2013DarkGray","Office2013LightGray","Seven","VS2010","TouchlineDark","Office2016White","Office2016Black","Office2016Colorfull" };
            this.MapPanels = new ObservableCollection<MapPanelViewModel>();
            this.service = IocService.Get<IConfigurationService>();
        }

        #region Commands

        #region Работа с картами и слоями

        private DelegateCommand addMapViewCommand;
        /// <summary>
        /// Показать панель создания карты
        /// </summary>
        public DelegateCommand AddMapViewCommand
        {
            get
            {
                if( addMapViewCommand == null )
                {
                    addMapViewCommand = new DelegateCommand( () =>
                        {
                            //Создание новой карты
                            MapPanelViewModel mapViewModel = new MapPanelViewModel( new MapViewModel() { Guid = Guid.NewGuid(), Name = "Новая карта" } );
                            EditMapViewModel map = new EditMapViewModel( mapViewModel, AddMapPanelCommand );
                            AddMapEventProc( map );
                        } );
                }
                return addMapViewCommand;
            }
        }

        /// <summary>
        /// Добавить карту в коллекцию
        /// </summary>
        private DelegateCommand<MapViewModel> addMapPanelCommand;
        public DelegateCommand<MapViewModel> AddMapPanelCommand
        {
            get
            {
                if( addMapPanelCommand == null )
                {
                    addMapPanelCommand = new DelegateCommand<MapViewModel>( (map) =>
                    {
                        this.MapPanels.Add( new MapPanelViewModel(map) );
                    } );
                }
                return addMapPanelCommand;
            }
        }

        /// <summary>
        /// Удалить карту
        /// </summary>
        private DelegateCommand<MapPanelViewModel> removeMapPanelCommand;
        public DelegateCommand<MapPanelViewModel> RemoveMapPanelCommand
        {
            get
            {
                if( removeMapPanelCommand == null )
                {
                    removeMapPanelCommand = new DelegateCommand<MapPanelViewModel>( ( obj ) =>
                    {
                        this.MapPanels.Remove( obj );
                    } );
                }
                return removeMapPanelCommand;
            }
        }

        #endregion

        #region Состояние приложения
        private DelegateCommand saveStateCommand;
        public DelegateCommand SaveStateCommand
        {
            get
            {
                if( saveStateCommand == null )
                {
                    saveStateCommand = new DelegateCommand( () =>
                    {
                        service.SaveStateCommon( this );
                        service.SaveStateDockLayoutManager();
                    } );
                }
                return saveStateCommand;
            }
        }

        private DelegateCommand refreshStateCommand;
        public DelegateCommand RefreshStateCommand
        {
            get
            {
                if( refreshStateCommand == null )
                {
                    refreshStateCommand = new DelegateCommand( () =>
                        {
                            service.Refresh();
                            this.MapPanels.ToList().ForEach( n => n.IsClosed = true );
                            this.MapPanels.Clear();
                            service.Load(this);
                        } );
                }
                return refreshStateCommand;

            }
        }

        #endregion
        #endregion
    }
}
