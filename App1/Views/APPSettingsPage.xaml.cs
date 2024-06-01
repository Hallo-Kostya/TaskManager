using App1.ViewModels;
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
    public partial class APPSettingsPage : ContentPage
    {
        public APPSettingsPage()
        {
            InitializeComponent();
            BindingContext = new APPSettingsViewModel(Navigation);
        }
    }
}