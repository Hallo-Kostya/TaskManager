using App1.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Notification2PopupPage : PopupPage
    {
        public Notification2PopupPage()
        {
            InitializeComponent();
            BindingContext = new Notification2PopupViewModel(Navigation);
        }
    }
}