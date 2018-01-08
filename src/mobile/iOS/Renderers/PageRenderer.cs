using System;
using TinyShopping.iOS.Renderers;
using TinyShopping.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(TilePageRenderer))]

namespace TinyShopping.iOS.Renderers
{
    public class TilePageRenderer : PageRenderer
    {
        public override void WillMoveToParentViewController(UIKit.UIViewController parent)
        {
            base.WillMoveToParentViewController(parent);
            if (Element is ISearchControllerPage searchView)
            {
                var searchController = new UISearchController(searchResultsController: null)
                {
                    HidesNavigationBarDuringPresentation = true,
                    DimsBackgroundDuringPresentation = false,
                    //ObscuresBackgroundDuringPresentation = true
                };
                searchController.SearchBar.SearchBarStyle = UISearchBarStyle.Prominent;
                searchController.SearchBar.Placeholder = "Search or add";
                parent.NavigationItem.SearchController = searchController;
                searchController.SearchBar.ShowsSearchResultsButton = true;
                var tf = searchController.SearchBar.ValueForKey(new Foundation.NSString("_searchField")) as UITextField;
                if (tf != null)
                {
                    tf.ClearButtonMode = UITextFieldViewMode.Never;
                    tf.ReturnKeyType = UIReturnKeyType.Send;
                }
                searchController.SearchBar.SearchButtonClicked += (sender, e) => {
                    searchView.SearchHandler.AddItem();
                };
               
                searchController.SearchBar.TextChanged += (sender, e) =>
                {
                    searchView.SearchHandler.Search(e.SearchText);
                };
                searchController.SearchBar.CancelButtonClicked += (sender, e) => {
                    searchView.SearchHandler.Clear();
                };
            }

            // parent.NavigationItem.TitleView = view.Subviews[0];
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            UpdateTiles();
        }
        public override void ViewWillAppear(bool animated)
        {
            UpdateTiles();
            base.ViewWillAppear(animated);

        }

        private void UpdateTiles()
        {
            if (Element is ICustomTitleView tv)
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0) && NavigationController != null && NavigationController.NavigationBar != null)
                {
                    NavigationController.NavigationBar.PrefersLargeTitles = tv.LargeTile;
                }
            }
        }

        //public override void ViewDidDisappear(bool animated)
        //{
            

        //    if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0) && NavigationController != null && NavigationController.NavigationBar != null)
        //    {
        //        NavigationController.NavigationBar.PrefersLargeTitles = false;
        //    }
        //    base.ViewDidDisappear(animated);
        //}
    }
}
