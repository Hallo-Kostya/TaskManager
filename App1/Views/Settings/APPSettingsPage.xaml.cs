using App1.ViewModels;
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
    public partial class APPSettingsPage : ContentPage
    {
        public APPSettingsPage()
        {
            InitializeComponent();
            BindingContext = new APPSettingsViewModel(Navigation);
            if ((BindingContext as APPSettingsViewModel).IsArchiveCleaningEnabled == true)
            {
                NextClean.IsVisible = true;
                ArchiveClean.IsVisible = true;
            }
            else
            {
                NextClean.IsVisible = false;
                ArchiveClean.IsVisible = false;
            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        { 
            if ((BindingContext as APPSettingsViewModel).IsArchiveCleaningEnabled == false)
            {
                NextClean.IsVisible = false;
                ArchiveClean.IsVisible = false;
            }
            else
            {
                NextClean.IsVisible = true;
                ArchiveClean.IsVisible = true;
            }
        }
    }
}