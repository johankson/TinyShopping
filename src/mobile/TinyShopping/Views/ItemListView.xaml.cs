using System;
using System.Collections.Generic;
using TinyMvvm.Forms;
using TinyShopping.ApplicationModels;
using TinyShopping.ViewModels;
using Xamarin.Forms;

namespace TinyShopping.Views
{
    public partial class ItemListView : ViewBase<ItemListViewModel>, ISearchControllerPage
    {
        public ItemListView()
        {
            InitializeComponent();
        }

        void Handle_OnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var cell = sender as SwitchCell;
            ViewModel.Changed.Execute(cell.BindingContext as Item);
        }

        public bool ShowSearchBar => true;

        public ISearchHandler SearchHandler => ViewModel;

        public bool LargeTile => true;

    }
}
