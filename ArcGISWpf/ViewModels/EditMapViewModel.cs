using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;

namespace ArcGISWpf.ViewModels
{
    public class EditMapViewModel:MapViewModel
    {
        #region Properties
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
        #endregion

        public MapViewModel baseMap;
        public MapViewModel BaseMap
        {
            get
            {
                return baseMap;
            }
            set
            {
                baseMap = value;
                NotifyPropertyChanged( "BaseMap" );
            }
        }

        private MapPanelViewModel mapViewModel;
        private DelegateCommand<MapViewModel> saveCommand;
        public EditMapViewModel( MapPanelViewModel map, DelegateCommand<MapViewModel> saveCommand )
        {
            this.mapViewModel = map;
            this.BaseMap = map.BaseViewModel;
            this.saveCommand = saveCommand;
            RefreshMapCommand.Execute(null);
            //TO:DO  копирование коллекции Layers
        }

        #region Commands
        private DelegateCommand cancelMapCommand;
        public DelegateCommand CancelMapCommand
        {
            get
            {
                if( cancelMapCommand == null )
                {
                    cancelMapCommand = new DelegateCommand( () => { this.IsClosed = true; } );
                }
                return cancelMapCommand;
            }
        }

        private DelegateCommand refreshMapCommand;
        public DelegateCommand RefreshMapCommand
        {
            get
            {
                if( refreshMapCommand == null )
                {
                    refreshMapCommand = new DelegateCommand( () =>
                        {
                            this.Guid = baseMap.Guid;
                            this.Name = baseMap.Name;
                            this.WKID = baseMap.WKID;
                            this.X1 = baseMap.X1;
                            this.X2 = baseMap.X2;
                            this.Y1 = baseMap.Y1;
                            this.Y2 = baseMap.Y2;
                        } );
                }
                return refreshMapCommand;
            }
        }

        private DelegateCommand saveMapCommand;
        public DelegateCommand SaveMapCommand
        {
            get
            {
                if( saveMapCommand == null )
                {
                    saveMapCommand = new DelegateCommand( ( ) =>
                        {
                            #region Сохранить изменения
                            BaseMap.Guid = this.Guid;
                            BaseMap.Name = this.Name;
                            BaseMap.WKID = this.WKID;
                            BaseMap.X1 = this.X1;
                            BaseMap.X2 = this.X2;
                            BaseMap.Y1 = this.Y1;
                            BaseMap.Y2 = this.Y2;
                            #endregion
                            if( saveCommand != null )
                            {
                                saveCommand.Execute( BaseMap );
                                this.ChangeProjectionCommand.Execute( null );
                            }
                            this.IsClosed = true;
                        } );
                }
                return saveMapCommand;
            }
        }

        private DelegateCommand changeProjectionCommand;
        public DelegateCommand ChangeProjectionCommand
        {
            get
            {
                if( changeProjectionCommand == null )
                {
                    changeProjectionCommand = new DelegateCommand( () =>
                    {
                        mapViewModel.ChangedProjectionProc();
                    } );
                }
                return changeProjectionCommand;
            }
        }
        #endregion
    }
}
