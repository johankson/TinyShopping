using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TinyShopping.ViewModels
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ShoppingBaseModel : TinyMvvm.ViewModelBase
    {
        private bool hasLoaded = false;

        public async virtual Task OnFirstAppear() {
            
        }

        public async override Task OnAppearing()
        {
            if (!hasLoaded) {
                hasLoaded = true;
                await OnFirstAppear();
            }
            await base.OnAppearing();
        }
    }
}

