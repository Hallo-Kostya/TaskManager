using App1.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class APPSettingsPage : ContentPage
    {
        public int CleaningInterval;
        public APPSettingsPage()
        {
            InitializeComponent();
            BindingContext = new APPSettingsViewModel(Navigation);
            if ((BindingContext as APPSettingsViewModel).IsArchiveCleaningEnabled == true)
            {
                //NextClean.IsVisible = true;
                ArchiveClean.IsVisible = true;
            }
            else
            {
                //NextClean.IsVisible = false;
                ArchiveClean.IsVisible = false;
            }
            CleaningInterval= Preferences.Get("CleaningInterval", 24);
            if (CleaningInterval > 720)
            {
                CustomTimeLayout.IsVisible = true;
            }
            else
            {
                CustomTimeLayout.IsVisible = false;
            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        { 
            if ((BindingContext as APPSettingsViewModel).IsArchiveCleaningEnabled == false)
            {
                //NextClean.IsVisible = false;
                ArchiveClean.IsVisible = false;
            }
            else
            {
                //NextClean.IsVisible = true;
                ArchiveClean.IsVisible = true;
            }
        }
        private void ButtonOwnTime_Clicked(object sender, EventArgs e)
        {
            CustomTimeLayout.IsVisible = true;
            ButtonOwnTime.IsVisible = false;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            CustomTimeLayout.IsVisible = false;
        }

        private void ImageButton_Clicked_1(object sender, EventArgs e)
        {
            CustomTimeLayout.IsVisible = false;
            ButtonOwnTime.IsVisible = true;
        }
    }
}