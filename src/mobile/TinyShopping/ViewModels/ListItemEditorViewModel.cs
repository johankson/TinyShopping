﻿using System.Windows.Input;
﻿using System;
using System.Threading.Tasks;
using TinyMvvm;
using TinyPubSubLib;
using TinyShopping.ApplicationModels;
using TinyShopping.Core.Services;
using TinyShopping.Messaging;

namespace TinyShopping.ViewModels
{

    public class ListItemEditorViewModel : ShoppingBaseModel
    {
        private ShoppingService _shoppingService;

        public ListItemEditorViewModel(ShoppingService shoppingService)
        {
            _shoppingService = shoppingService;
        }

        public async override Task Initialize()
        {
            var item = NavigationParameter as Item;

            if (item == null)
            {
                item = new Item()
                {
                    Name = "No name"
                };
            }

            Item = item;
        }

        public Item Item { get; set; }

        public string Name { get; set; }

        public ICommand Save => new TinyCommand(async () =>
        {
            if (Item.Id == 0)
            {
                await _shoppingService.AddItem(Item);
            }
            else
            {
                await _shoppingService.UpdateItem(Item);
            }

            await TinyPubSub.PublishAsync(Channels.ShoppingListAdded);
            await Navigation.BackAsync();
        });
            
        
    }
}
