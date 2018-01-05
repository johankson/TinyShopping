using TinyShopping.ViewModels;
using Xamarin.Forms;

namespace TinyShopping
{
    public partial class TinyShoppingPage : ContentPage
    {
        public TinyShoppingPage()
        {
            InitializeComponent();
            BindingContext = new TinyShoppingListViewModel();
        }
    }
}
