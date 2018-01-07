using System;
using System.Collections.Generic;
using Xamarin.Forms;
using TinyMvvm.Forms;
using TinyShopping.ViewModels;

namespace TinyShopping.Views
{
    public partial class ListItemEditorView : ViewBase<ListItemEditorViewModel>
    {
        public ListItemEditorView()
        {
            InitializeComponent();
        }
    }
}
