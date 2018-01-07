<<<<<<< HEAD
﻿using System.Windows.Input;
=======
﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
>>>>>>> ed1a8be5822fe2b740f9cfae7a004f6ea427668f
using TinyMvvm;
using TinyPubSubLib;
using TinyShopping.ApplicationModels;
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

<<<<<<< HEAD
=======
        public async override Task Initialize()
        {
            var shoppingList = NavigationParameter as ShoppingList;

            if (shoppingList == null)
            {
                shoppingList = new ShoppingList()
                {
                    Name = "No name"
                };
            }

            ShoppingList = shoppingList;
        }

>>>>>>> ed1a8be5822fe2b740f9cfae7a004f6ea427668f
        public ShoppingList ShoppingList { get; set; }

        public string Name { get; set; }

        public ICommand Save
        {
            get
            {
                return new TinyCommand(async () =>
                {
                    if (ShoppingList.Id == 0)
                    {
                        await _shoppingService.AddList(ShoppingList);
                    }
                    else
                    {
                        await _shoppingService.UpdateList(ShoppingList);
                    }

                    await TinyPubSub.PublishAsync(Channels.ShoppingListAdded);
                    await Navigation.BackAsync();
                });
            }
        }
    }
}
