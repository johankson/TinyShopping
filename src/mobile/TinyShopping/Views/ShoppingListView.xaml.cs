namespace TinyShopping.Views
{
    using TinyMvvm.Forms;
    using TinyShopping.ApplicationModels;
    using TinyShopping.ViewModels;
    using Xamarin.Forms;

    public partial class ShoppingListView : ViewBase<ShoppingListViewModel>, ICustomTitleView
    {
        public View TitleView { get; set; }

        public ShoppingListView()
        {
            this.InitializeComponent();

            MainListView.ItemSelected += (sender, e) => MainListView.SelectedItem = null;
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            //   ViewModel.OpenList(e.SelectedItem as ShoppingList);
        }

        void Handle_Completed(object sender, System.EventArgs e)
        {
            var entry = sender as Entry;
            ViewModel.AddListFromName();
        }
    }

    internal interface ICustomTitleView
    {
        View TitleView { get; set; }
    }
}
