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
	public partial class FolderAddingPage : ContentPage
    {
		public FolderAddingPage ()
		{
			InitializeComponent ();
			BindingContext = new FolderAddingViewModel(Navigation);
		}
		public void SaveButton_Clicked()
		{
			if (string.IsNullOrWhiteSpace((BindingContext as FolderAddingViewModel).WritenName))
			{
				NullNameAlert.IsEnabled = true;
			}

        }
	}
}