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
            BindingContext = assignmentViewModel = new AssignmentViewModel(Navigation);
            InitializeComponent();

            DateTime currentDate = DateTime.Now;
            int today = currentDate.Day;

            //labelMonth.Text = char.ToUpper(currentDate.ToString("MMMM", CultureInfo.CreateSpecificCulture("ru-RU"))[0]) + currentDate.ToString("MMMM", CultureInfo.CreateSpecificCulture("ru-RU")).Substring(1);
            DateTime startOfWeek = currentDate.AddDays((int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)currentDate.DayOfWeek);
            //SetDayButtonBackground(Day1, startOfWeek, today);
            //SetDayButtonBackground(Day2, startOfWeek.AddDays(1), today);
            //SetDayButtonBackground(Day3, startOfWeek.AddDays(2), today);
            //SetDayButtonBackground(Day4, startOfWeek.AddDays(3), today);
            //SetDayButtonBackground(Day5, startOfWeek.AddDays(4), today);
            //SetDayButtonBackground(Day6, startOfWeek.AddDays(5), today);
            //SetDayButtonBackground(Day7, startOfWeek.AddDays(6), today);

        }

        private void SetDayButtonBackground(Button dayButton, DateTime date, int today)
        {
            if (dayButton != null && today == date.Day)
            {
                dayButton.BackgroundColor = Color.FromHex("#952EAF");
            }
            dayButton.Text = date.Day.ToString();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            assignmentViewModel.OnAppearing();
        }
        //private void Button_Clicked(object sender, EventArgs e)
        //{
        //    Button clickedButton = (Button)sender;

        //    if (previousButton != null && previousButton != Day1)
        //        previousButton.BackgroundColor = Color.Black;

        //    clickedButton.BackgroundColor = Color.FromHex("#952EAF");

        //    previousButton = clickedButton;

        //    if (clickedButton != Day1)
        //        Day1.BackgroundColor = Color.Black;
        //}

        private void AddTask_Clicked(object sender, EventArgs e)
        {
            //var noTasks = this.FindByName<StackLayout>("NoTasks");
            //if (noTasks != null)
            //{
            //    noTasks.IsVisible = false;
            //}
        }

        private bool isExpanded = false;
        private void Vector_Clicked(object sender, EventArgs e)
        {
            // Переключаем изображение в зависимости от текущего состояния
            if (isExpanded)
            {
                VectorButton.Source = "vector_down";
            }
            else
            {
                VectorButton.Source = "vector_up";
            }

            // Переключаем видимость DoneTasks
            DoneTasks.IsVisible = !isExpanded;

            // Обновляем состояние кнопки
            isExpanded = !isExpanded;
        }
        private void TagList_Clicked(object sender, EventArgs e)
        {
            // Переключаем видимость DoneTasks
            TagLists.IsVisible = !isExpanded;

            // Обновляем состояние кнопки
            isExpanded = !isExpanded;
        }

    }
}