using App1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateFolderPage : ContentPage
    {
        public CreateFolderPage()
        {
            InitializeComponent();
            BindingContext = new СreateFolderViewModel(Navigation);
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            FolderConfirmLayout.IsVisible = true;
        }
        private void Confirm_Clicked_1(object sender, EventArgs e)
        {
            FolderConfirmLayout.IsVisible = false;
        }
    }
}