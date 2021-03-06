﻿using TinyCache;
using TinyNavigationHelper.Abstraction;
using Xamarin.Forms;

namespace TinyShopping
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Bootstrapper.Initialize(this);

            NavigationHelper.Current.SetRootView("ShoppingListView", true);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
