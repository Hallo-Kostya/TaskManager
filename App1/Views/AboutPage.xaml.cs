using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    public partial class AboutPage : ContentPage
    {
        private IPopupNavigation _popup { get; set; }
        private Page1 _modalPage;
        public AboutPage()
        {
            InitializeComponent();
            _popup = PopupNavigation.Instance;
            _modalPage = new Page1();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _popup.Popped += Popup_Popped;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _popup.Popped -= Popup_Popped;
        }
        async void OnButtonClicked(object sender, EventArgs args)
        {
            await _popup.PushAsync(_modalPage);
        }
        private async void Popup_Popped(object sender, Rg.Plugins.Popup.Events.PopupNavigationEventArgs e)
        {
            /* add your logic here, if necessary  */
        }
    }
}