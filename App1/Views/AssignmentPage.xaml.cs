using App1.Models;
using App1.Services;
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
    public partial class AssignmentPage : ContentPage
    {
        AssignmentViewModel assignmentViewModel;
        Button previousButton;
        public DateTime tempDate;
        public AssignmentPage()
        {
            InitializeComponent();
            BindingContext = assignmentViewModel = new AssignmentViewModel(Navigation);
            DayWeek1.BackgroundColor = Color.FromHex("#952EAF");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            assignmentViewModel.OnAppearing();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            tempDate=DateTime.Now; ///заглушка
            Button clickedButton = (Button)sender;

            if (previousButton != null && previousButton != DayWeek1)
                previousButton.BackgroundColor = Color.Black;

            clickedButton.BackgroundColor = Color.FromHex("#952EAF");

            previousButton = clickedButton;

            if (clickedButton != DayWeek1)
                DayWeek1.BackgroundColor = Color.Black;
            await Navigation.PushAsync(new AssignmentPage());
        }

        private void AddTask_Clicked(object sender, EventArgs e)
        {
            var noTasks = this.FindByName<StackLayout>("NoTasks");
            if (noTasks != null)
            {
                noTasks.IsVisible = false;
            }
        }
    }
}