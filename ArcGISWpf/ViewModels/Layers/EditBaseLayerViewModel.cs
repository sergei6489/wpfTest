using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using ArcGISWpf.ViewModels.Layers;
using DevExpress.Mvvm;
using ESRI.ArcGIS.Client.Local;
using Microsoft.Win32;

namespace ArcGISWpf.ViewModels
{
    /// <summary>
    /// Редактирование слоя
    /// </summary>
    public abstract class EditBaseLayerViewModel<T>: BaseViewModel where T : BaseLayerViewModel
    {
        #region Properties

        private bool isClosed;
        public bool IsClosed
        {
            get { return isClosed; }
            set
            {
                this.isClosed = value;
                NotifyPropertyChanged( "IsClosed" );
            }
        }

        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                this.isVisible = value;
                NotifyPropertyChanged( "IsVisible" );
            }
        }

        private string errors;
        public string Errors
        {
            get
            {
                return errors;
            }
            set
            {
                errors = value;
                NotifyPropertyChanged( "Errors" );
            }
        }

        private T baseViewModel;
        public T BaseViewModel
        {
            get
            {
                return baseViewModel;
            }
            set
            {
                baseViewModel = value;
                NotifyPropertyChanged("BaseViewModel");
            }
        }

        private T viewModel;
        public T ViewModel
        {
            get
            {
                return viewModel;
            }
            set
            {
                viewModel = value;
                NotifyPropertyChanged("ViewModel");
            }
        }
        #endregion

        private DelegateCommand<BaseLayerViewModel> saveCommand;
        public EditBaseLayerViewModel(BaseLayerViewModel layer, DelegateCommand<BaseLayerViewModel> saveCommand )
        {
            this.BaseViewModel =(T) layer;
            this.saveCommand = saveCommand;
            this.RefreshLayerCommand.Execute( null );
        }

        #region Commands

        private DelegateCommand saveLayerCommand;
        public DelegateCommand SaveLayerCommand
        {
            get
            {
                if( saveLayerCommand == null )
                {
                    saveLayerCommand = new DelegateCommand( (  ) =>
                    {
                        Validation();          
                        if( errors==null || errors.Length == 0 )
                        {
                            #region Сохранить изменения
                            SaveChanges();

                            #endregion
                            this.Errors = null;
                            if( saveCommand != null )
                            {
                                this.saveCommand.Execute( (T) this.ViewModel.Clone() );
                            }
                            this.IsClosed = true;
                        }
                    } );
                }
                return saveLayerCommand;
            }
        }

        private DelegateCommand cancelLayerCommand;
        public DelegateCommand CancelLayerCommand
        {
            get
            {
                if( cancelLayerCommand == null )
                {
                    cancelLayerCommand = new DelegateCommand( () => { this.IsClosed = true; } );
                }
                return cancelLayerCommand;
            }
        }

        private DelegateCommand refreshLayerCommand;
        public DelegateCommand RefreshLayerCommand
        {
            get
            {
                if( refreshLayerCommand == null )
                {
                    refreshLayerCommand = new DelegateCommand( () =>
                    {
                        Refresh();
                    } );
                }
                return refreshLayerCommand;
            }
        }

        #endregion

        public void SaveChanges()
        {
            this.BaseViewModel.SaveChanges(this.ViewModel);
        }
        protected void Refresh()
        {
            this.ViewModel = (T)this.BaseViewModel.Clone();
        }
        public abstract void Validation();
    }
}
