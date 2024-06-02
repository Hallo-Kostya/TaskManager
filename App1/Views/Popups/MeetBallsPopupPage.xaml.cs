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
    public partial class MeetBallsPopupPage : PopupPage
    {
        public MeetBallsPopupPage()
        {
            InitializeComponent();
            BindingContext = new MeetBallsPopupViewModel(Navigation);
        }
    }
}