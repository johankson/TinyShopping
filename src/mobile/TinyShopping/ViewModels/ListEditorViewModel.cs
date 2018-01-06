using System;
using System.Windows.Input;
using TinyMvvm;
using TinyNavigationHelper.Forms;
using TinyPubSubLib;
using TinyShopping.Core.Net.Models;
using TinyShopping.Core.services;
using TinyShopping.Messaging;

namespace TinyShopping.ViewModels
{
    public class ListEditorViewModel : ShoppingBaseModel
    {
        private ShoppingService _shoppingService;

        public ListEditorViewModel(ShoppingService shoppingService)
        {
            _shoppingService = shoppingService;
        }

        public string Name { get; set; }

        public ICommand Save 
        {
            get
            {
                return new TinyCommand(async () =>
                {
                    await _shoppingService.AddList(new ShoppingList()
                    {
                        Name = this.Name
                    });

                    await TinyPubSub.PublishAsync(Channels.ShoppingListAdded);
                    await Navigation.BackAsync(); 
                });
            }
        }
    }
}
