using System;
using System.Collections.Generic;
using System.Linq;
using ArcGISWpf.ViewModels;
using DevExpress.Xpf.Docking;

namespace ArcGISWpf.Controls
{
    /// <summary>
    /// Interaction logic for EditMapPanelView.xaml
    /// </summary>
    public partial class EditMapPanelView : LayoutPanel
    {
        public EditMapPanelView( EditMapViewModel viewModel )
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
