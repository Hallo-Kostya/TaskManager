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
        public Notification2PopupPage(AssignmentModel assignment)
        {
            InitializeComponent();
            BindingContext = new Notification2PopupViewModel(Navigation);
            if (assignment != null)
            {
                (BindingContext as Notification1PopupViewModel).Assignment = assignment;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}