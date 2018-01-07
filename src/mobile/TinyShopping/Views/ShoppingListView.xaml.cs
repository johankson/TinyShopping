﻿namespace TinyShopping.Views
{
    using TinyMvvm.Forms;
    using TinyShopping.ApplicationModels;
    using TinyShopping.ViewModels;
    using Xamarin.Forms;

    public partial class ShoppingListView : ViewBase<ShoppingListViewModel>
    {
        public ShoppingListView()
        {
            this.InitializeComponent();
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            ViewModel.OpenList(e.SelectedItem as ShoppingList);
        }

        void Handle_Completed(object sender, System.EventArgs e)
        {
            var entry = sender as Entry;
            ViewModel.AddListFromName();
        }
    }
}
