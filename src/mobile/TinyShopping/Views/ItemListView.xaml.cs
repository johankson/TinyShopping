using System;
using System.Collections.Generic;
using TinyMvvm.Forms;
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

        public bool ShowSearchBar => true;

        public ISearchHandler SearchHandler => ViewModel;

        public bool LargeTile => true;

        void Handle_Completed(object sender, System.EventArgs e)
        {
            ViewModel.AddItemFromName();
        }
    }
}
