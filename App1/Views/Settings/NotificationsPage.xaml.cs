using App1.ViewModels.Settings;
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
	public partial class NotificationsPage : ContentPage
	{
		public NotificationsPage ()
		{
			InitializeComponent ();
            BindingContext = new NotificationsViewModel(Navigation);
        }
    }
}