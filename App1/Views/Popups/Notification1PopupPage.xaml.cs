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
    public partial class Notification1PopupPage : PopupPage
    {
        public Notification1PopupPage()
        {
            InitializeComponent();
            BindingContext = new Notification1PopupViewModel(Navigation);
        }
    }
}