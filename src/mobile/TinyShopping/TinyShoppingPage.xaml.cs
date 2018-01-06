using TinyShopping.ViewModels;
using Xamarin.Forms;

namespace TinyShopping
{
    public partial class TinyShoppingPage : ContentPage
    {
        public TinyShoppingPage()
        {
            InitializeComponent();

            var vm = new TinyShoppingListViewModel();
            vm.Navigation = this.Navigation;
            BindingContext = vm;
        }
    }
}
