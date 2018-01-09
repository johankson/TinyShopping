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
        private bool _isInitializing = true;

        public ItemListView()
        {
            InitializeComponent();
            _isInitializing = false;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (this.ViewModel != null)
            {
                this.ViewModel.ScrollTo = (Item obj) => MainListView.ScrollTo(obj, ScrollToPosition.MakeVisible, false);
                this.ViewModel.PlayTickAnimation = () =>
                {
                    animationView.IsVisible = true;
                    animationView.Opacity = 1;

                    var animation = new Animation((d) => animationView.Progress = (float)d, 0.2, 1);
                    animationView.Animate("anka", animation, rate: 60, length: 1000, finished: (arg1, arg2) => 
                                           animationView.FadeTo(0)
                                         );
                };
            }
        }

        public bool ShowSearchBar => true;

        public ISearchHandler SearchHandler => ViewModel;

        public bool LargeTile => true;
    }
}
