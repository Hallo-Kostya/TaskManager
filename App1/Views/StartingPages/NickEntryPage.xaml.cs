using App1.ViewModels.StartingPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views.StartingPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NickEntryPage : ContentPage
    {
        public NickEntryPage()
        {
            InitializeComponent();
            BindingContext = new NickEntryPageViewModel(Navigation);
            AnimateElements();
        }
        private async void AnimateElements()
        {
            await WelcomeLabel.FadeTo(1, 1000); 
            await Task.Delay(3000); 

            

            await IntroductionLabel.FadeTo(1, 1000); 
            await Task.Delay(5000); 
            await NameLabel.FadeTo(1, 1000);
            await Task.Delay(1000);
            await NicknameEntry.FadeTo(1, 1000);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((NickEntryPageViewModel)BindingContext).PropertyChanged += NickEntryPage_PropertyChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ((NickEntryPageViewModel)BindingContext).PropertyChanged -= NickEntryPage_PropertyChanged;
        }

        private async void NickEntryPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NickEntryPageViewModel.IsNicknameNotEmpty))
            {
                if (((NickEntryPageViewModel)BindingContext).IsNicknameNotEmpty)
                {
                    SaveButton.IsVisible = true;
                    await SaveButton.FadeTo(1, 1000); 
                }
                else
                {
                    SaveButton.IsVisible = false;
                    await SaveButton.FadeTo(0, 1000); 
                }
            }
        }
    }
}
