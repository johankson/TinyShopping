namespace TinyShopping.Views
{
    using System;
    using TinyMvvm.Forms;
    using TinyShopping.ApplicationModels;
    using TinyShopping.ViewModels;
    using Xamarin.Forms;

    public partial class ShoppingListView : ViewBase<ShoppingListViewModel>, ISearchControllerPage
    {
        public bool ShowSearchBar => true;

        public ISearchHandler SearchHandler => ViewModel;

        public bool LargeTile => true;

        public ShoppingListView()
        {
            this.InitializeComponent();
            MainListView.ItemSelected += (sender, e) => MainListView.SelectedItem = null;
        }

    }

    public interface ICustomTitleView
    {
        bool LargeTile { get; }
    }

    public interface ISearchControllerPage : ICustomTitleView
    {
        bool ShowSearchBar { get; }
        ISearchHandler SearchHandler { get; }
    }

    public interface ISearchHandler
    {
        void Search(string value);
        void Clear();
        void AddItem();
    }
}
