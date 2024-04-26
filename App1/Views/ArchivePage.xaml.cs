using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App1.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ArchivePage : ContentPage
	{
        ArchiveViewModel archiveViewModel;
		public ArchivePage ()
		{
			InitializeComponent ();
            BindingContext = archiveViewModel = new ArchiveViewModel();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as ArchiveViewModel).ExecuteLoadArchive();
        }
    }
}