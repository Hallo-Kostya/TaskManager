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
	public partial class FolderAddingPage : ContentPage
    {
		public FolderAddingPage ()
		{
			InitializeComponent ();
			BindingContext = new FolderAddingViewModel(Navigation);
		}
        public FolderAddingPage(ListModel folder)
        {
            InitializeComponent();
            BindingContext = new FolderAddingViewModel(Navigation);
            if (folder != null )
            {
                (BindingContext as FolderAddingViewModel).Folder = folder;
                (BindingContext as FolderAddingViewModel).SelectedIcon = folder.Icon;
                (BindingContext as FolderAddingViewModel).SelectedColor = Color.FromHex(folder.Color);
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace((BindingContext as FolderAddingViewModel).WritenName))
            {
                NullNameAlert.IsVisible = true;
            }
        }
    }
}