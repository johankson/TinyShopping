using TinyShopping.Core.services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TinyPubSubLib;
using TinyShopping.Messaging;
<<<<<<< HEAD
using TinyShopping.ApplicationModels;
=======
using System.Windows.Input;
>>>>>>> ed1a8be5822fe2b740f9cfae7a004f6ea427668f

namespace TinyShopping.ViewModels
{

    public class ShoppingListViewModel : ShoppingBaseModel
    {
        private ShoppingService _shoppingService;
		
        public ShoppingListViewModel(ShoppingService shoppingService) 
        {
            _shoppingService = shoppingService;
        }

        public async override Task OnFirstAppear()
        {
            await LoadData();
        }

        [TinySubscribe(Channels.ShoppingListAdded)]
        [TinySubscribe(Channels.ShoppingListDeleted)]
        public async Task LoadData()
        {
            ShoppingLists = new ObservableCollection<ShoppingList>(await _shoppingService.GetShoppingLists());
        }

        public ObservableCollection<ShoppingList> ShoppingLists { get; set; }

        public ShoppingList SelectedItem
        {
            set
            {
                if(value == null)
                {
                    return;
                }
            }
        }

        public ICommand Delete
        {
            get
            {
                return new TinyCommand<ShoppingList>(async (shoppingList) =>
                {
                    ShoppingLists.Remove(shoppingList);
                    await _shoppingService.Delete(shoppingList);
                });
            }
        }

        public ICommand Edit
        {
            get
            {
                return new TinyCommand<ShoppingList>(async (shoppingList) =>
                {
                    await Navigation.NavigateToAsync("ListEditorView", shoppingList);
                });
            }
        }
    }
}
