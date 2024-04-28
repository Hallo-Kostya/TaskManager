using App1.Models;
using App1.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public AssignmentPage()
        {
            InitializeComponent();
            BindingContext = assignmentViewModel = new AssignmentViewModel(Navigation);
            Day1.BackgroundColor = Color.FromHex("#952EAF");
            DateTime currentDate = DateTime.Now;

            labelMonth.Text = char.ToUpper(currentDate.ToString("MMMM", CultureInfo.CreateSpecificCulture("ru-RU"))[0]) + currentDate.ToString("MMMM", CultureInfo.CreateSpecificCulture("ru-RU")).Substring(1);
            SetDayOfWeekText(DayWeek1, currentDate);
            SetDayOfWeekText(DayWeek2, currentDate.AddDays(1));
            SetDayOfWeekText(DayWeek3, currentDate.AddDays(2));
            SetDayOfWeekText(DayWeek4, currentDate.AddDays(3));
            SetDayOfWeekText(DayWeek5, currentDate.AddDays(4));
            SetDayOfWeekText(DayWeek6, currentDate.AddDays(5));
            SetDayOfWeekText(DayWeek7, currentDate.AddDays(6));
            Day1.Text = currentDate.ToString("dd");
            Day2.Text = currentDate.AddDays(1).ToString("dd");
            Day3.Text = currentDate.AddDays(2).ToString("dd");
            Day4.Text = currentDate.AddDays(3).ToString("dd");
            Day5.Text = currentDate.AddDays(4).ToString("dd");
            Day6.Text = currentDate.AddDays(5).ToString("dd");
            Day7.Text = currentDate.AddDays(6).ToString("dd");
        }

        private void SetDayOfWeekText(Label label, DateTime date)
        {
            label.Text = char.ToUpper(date.ToString("ddd", CultureInfo.CreateSpecificCulture("ru-RU"))[0]) + date.ToString("ddd", CultureInfo.CreateSpecificCulture("ru-RU")).Substring(1);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            assignmentViewModel.OnAppearing();
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            if (previousButton != null && previousButton != Day1)
                previousButton.BackgroundColor = Color.Black;

            clickedButton.BackgroundColor = Color.FromHex("#952EAF");

            previousButton = clickedButton;

            if (clickedButton != Day1)
                Day1.BackgroundColor = Color.Black;
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