using System;
using TinyMvvm.IoC;
using TinyShopping.iOS.Renderers;
using TinyShopping.Views;
using TinyTranslation.Forms;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(TilePageRenderer))]

namespace TinyShopping.iOS.Renderers
{
    public class TilePageRenderer : PageRenderer
    {
        private readonly TranslationHelper _transService;

        public TilePageRenderer()
        {
            _transService = Resolver.Resolve<TranslationHelper>();
        }


        public override void WillMoveToParentViewController(UIKit.UIViewController parent)
        {
            base.WillMoveToParentViewController(parent);
            if (Element is ISearchControllerPage searchView)
            {
                var searchController = new UISearchController(searchResultsController: null)
                {
                    HidesNavigationBarDuringPresentation = false,
                    DimsBackgroundDuringPresentation = false,
                    //ObscuresBackgroundDuringPresentation = true
                };

                searchController.SearchBar.SearchBarStyle = UISearchBarStyle.Prominent;
                searchController.SearchBar.Placeholder = _transService.Translate("Search or add");
                parent.NavigationItem.SearchController = searchController;
                searchController.SearchBar.ShowsSearchResultsButton = true;
                searchController.SearchBar.ShowsCancelButton = false;
                var tf = searchController.SearchBar.ValueForKey(new Foundation.NSString("_searchField")) as UITextField;

                if (tf != null)
                {
                    tf.ClearButtonMode = UITextFieldViewMode.Never;
                    tf.ReturnKeyType = UIReturnKeyType.Send;
                }

                searchController.SearchBar.SearchButtonClicked += (sender, e) =>
                {
                    searchView.SearchHandler.AddItem();
                    searchController.SearchBar.Text = String.Empty;
                };

                searchController.SearchBar.TextChanged += (sender, e) =>
                {
                    searchView.SearchHandler.Search(e.SearchText);
                };

                searchController.SearchBar.CancelButtonClicked += (sender, e) =>
                {
                    searchView.SearchHandler.Clear();
                };
            }
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
    }
}
