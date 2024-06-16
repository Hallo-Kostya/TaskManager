using App1.Models;
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
	public partial class СreateTagPage : ContentPage
	{
		public СreateTagPage ()
		{
			InitializeComponent ();
            BindingContext = new СreateTagViewModel(Navigation);
        }
        public СreateTagPage(AssignmentModel assignment, bool _isFromPopUp)
        {
            InitializeComponent();
            BindingContext = new СreateTagViewModel(Navigation);
            (BindingContext as СreateTagViewModel).Assignment = assignment;
            (BindingContext as СreateTagViewModel).IsFromPopup= _isFromPopUp;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            ConfirmLayout.IsVisible = true;
            NullNameTagAlert.IsVisible = false;
        }

        private void Confirm_Clicked_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((BindingContext as СreateTagViewModel).Tag.Name))
            {
                NullNameTagAlert.IsVisible = true;
            }
            else
            {
                ConfirmLayout.IsVisible = false;
                NullNameTagAlert.IsVisible = false;
            }
        }
    }
}