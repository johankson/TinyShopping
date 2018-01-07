﻿namespace TinyShopping.Views
{
    using TinyMvvm.Forms;
    using TinyShopping.ViewModels;
    using Xamarin.Forms;

    public partial class ShoppingListView : ViewBase<ShoppingListViewModel>
    {
        public ShoppingListView()
        {
            this.InitializeComponent();
        }

        void Handle_Completed(object sender, System.EventArgs e)
        {
            var entry = sender as Entry;
            ViewModel.AddListFromName();
        }
    }
}
