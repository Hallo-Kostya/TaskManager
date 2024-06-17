using App1.Models;
using App1.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FolderConfirmationPage : PopupPage
    {
        public FolderConfirmationPage(ListModel folder)
        {
            InitializeComponent();
            BindingContext = new FolderConfirmationViewModel(Navigation);
            if (folder != null)
            {
                (BindingContext as FolderConfirmationViewModel).SelectedFolder = folder;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            RelocateTasks.IsVisible = true;
            RelocateTitle.IsVisible = true;
        }
    }
}