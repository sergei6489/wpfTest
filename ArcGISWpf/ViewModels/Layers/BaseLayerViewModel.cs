
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ArcGISWpf.Ioc;
using System.Xml.Serialization;

namespace ArcGISWpf.ViewModels.Layers
{

    [XmlInclude(typeof(ArcGISGlobalFeatureLayerViewModel))]
    [XmlInclude(typeof(ArcGISGlobalTileLayerViewModel))]
    [XmlInclude(typeof(ArcGISLocalFeatureLayerViewModel))]
    [XmlInclude(typeof(ArcGISLocalTileLayerViewModel))]
    public abstract class BaseLayerViewModel : BaseViewModel
    {
        public BaseLayerViewModel()
        {
        }

        #region Properties
        private Guid guid;
        public Guid Guid
        {
            get { return guid; }
            set
            {
                this.guid = value;
                NotifyPropertyChanged( "Guid" );
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

        private bool isVisible=true;
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                NotifyPropertyChanged( "IsVisible" );
            }
        }

        #region 
        private bool isEdit;
        public bool IsEdit
        {
            get
            {
                return isEdit;
            }
            set
            {
                isEdit = value;
                NotifyPropertyChanged( "IsEdit" );
            }
        }
        #endregion

        #endregion

        #region Commands

        private DelegateCommand<BaseLayerViewModel> editLayerViewCommand;
        /// <summary>
        /// Показать форму редактирования слоя
        /// </summary>
        public DelegateCommand<BaseLayerViewModel> EditLayerViewCommand
        {
            get
            {
                if( editLayerViewCommand == null )
                {
                    editLayerViewCommand = new DelegateCommand<BaseLayerViewModel> ( ( data ) =>
                    {                       
                        var mainViewModel = IocService.Get<MainViewModel>();
                        //TO:DO MapPanelViewModel надо уведомить через делегат команду, надо рассмотреть вариант как получить mapPanel более красивым способом
                        MapPanelViewModel mapPanel=null;
                        foreach( var map in mainViewModel.MapPanels )
                        {
                            if( map.BaseViewModel.Layers.FirstOrDefault( n => n.Guid == data.Guid )!=null )
                            {
                                mapPanel = map;
                            }                    
                        }
                        if (data.GetType() == typeof(ArcGISGlobalFeatureLayerViewModel))
                        {
                            mainViewModel.AddEditLayerEventProc( new EditArcGISGlobalFeatureLayerViewModel( (ArcGISGlobalFeatureLayerViewModel)data, mapPanel.EditionLayerSaved) );
                        }
                        else if (data.GetType() == typeof(ArcGISGlobalTileLayerViewModel))
                        {
                            mainViewModel.AddEditLayerEventProc( new EditArcGISGlobalTileLayerViewModel( (ArcGISGlobalTileLayerViewModel)data, mapPanel.EditionLayerSaved ) );
                        }
                        else if (data.GetType() == typeof(ArcGISLocalFeatureLayerViewModel))
                        {
                            mainViewModel.AddEditLayerEventProc( new EditArcGISLocalFeatureLayerViewModel( (ArcGISLocalFeatureLayerViewModel)data, mapPanel.EditionLayerSaved ) );
                        }
                        else if (data.GetType() == typeof(ArcGISLocalTileLayerViewModel))
                        {
                            mainViewModel.AddEditLayerEventProc( new EditArcGISLocalTileLayerViewModel( (ArcGISLocalTileLayerViewModel)data, mapPanel.EditionLayerSaved ) );
                        }                           
                    } );
                }
                return editLayerViewCommand;
            }
        }

        #endregion

        public abstract object Clone();

        public abstract void SaveChanges(BaseLayerViewModel obj);
    }
}
