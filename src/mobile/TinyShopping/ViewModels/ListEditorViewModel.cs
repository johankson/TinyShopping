using System;
using System.Windows.Input;
using TinyMvvm;
using TinyNavigationHelper.Forms;
using TinyPubSubLib;
using TinyShopping.Core.Net.Models;
using TinyShopping.Core.services;
using TinyShopping.Messaging;
using Xamarin.Forms;

namespace TinyShopping.ViewModels
{
    public class ListEditorViewModel : ShoppingBaseModel
    {
        private ShoppingService _shoppingService;

        public ListEditorViewModel(ShoppingService shoppingService)
        {
            _shoppingService = shoppingService;
            ShoppingList = new ShoppingList() {
                Name = "No name"
            };
        }



        public ShoppingList ShoppingList { get; set; }

        public string Name { get; set; }

        public ICommand Save
        {
            get
            {
                return new TinyCommand(async () =>
                {
                    if (ShoppingList.Id == 0)
                        await _shoppingService.AddList(ShoppingList);

                    await TinyPubSub.PublishAsync(Channels.ShoppingListAdded);
                    await Navigation.BackAsync();
                });
            }
        }
    }
}
