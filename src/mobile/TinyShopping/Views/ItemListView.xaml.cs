using System;
using System.Collections.Generic;
using TinyMvvm.Forms;
using TinyShopping.ViewModels;
using Xamarin.Forms;

namespace TinyShopping.Views
{
    public partial class ItemListView : ViewBase<ItemListViewModel>
    {
        public ItemListView()
        {
            InitializeComponent();
        }

        void Handle_Completed(object sender, System.EventArgs e)
        {
            ViewModel.AddItemFromName();
        }
    }
}
