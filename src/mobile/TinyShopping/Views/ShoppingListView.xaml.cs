using System;
using System.Collections.Generic;
using TinyShopping.ViewModels;
using Xamarin.Forms;

namespace TinyShopping.Views
{
    public partial class ShoppingListView : ContentPage
    {
        public ShoppingListView()
        {
            InitializeComponent();

            BindingContext = new ShoppingListViewModel();
        }
    }
}
