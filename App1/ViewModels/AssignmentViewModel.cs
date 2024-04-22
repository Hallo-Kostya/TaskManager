using App1.Models;
using App1.Services;
using App1.Views;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class AssignmentViewModel : BaseAssignmentViewModel
    {

        public Command LoadAssignmentCommand { get; }
        public ObservableCollection<AssignmentModel> assignments { get; set; }
        public Command AddAssignmentCommand { get; }
        public Command EditAssignmentCommand { get; }
        public Command DeleteAssignmentCommand { get; }
        public ILookup<string, AssignmentModel> GroupByIsCompleted { get; set; }
        public Command ChangeIsCompletedCommand { get; set; }
        public INavigation Navigation { get; set; }





        //private IDateService _dateService;
        //private DayModel _selectedDay;
        //public ObservableCollection<DayModel> DaysList { get; set; }
        //public ObservableCollection<AssignmentModel> TaskList { get; set; }
        //public LayoutState TaskListState { get; set; }
        //public string Name { get; set; }
        //public WeekModel Week { get; set; }
        //public string Filter { get; set; }
        //public ICommand DayCommand { get; set; }
        //public ICommand PreviousWeekCommand { get; set; }
        //public ICommand NextWeekCommand { get; set; }
        //public DateService dateService { get; set; }


        public AssignmentViewModel(INavigation _navigation)
        {
            LoadAssignmentCommand = new Command(async () => await ExecuteLoadAssignmentCommand());
            assignments = new ObservableCollection<AssignmentModel>();
            AddAssignmentCommand = new Command(OnAddAssignment);
            EditAssignmentCommand = new Command<AssignmentModel>(OnEditAssignment);
            Navigation = _navigation;
            DeleteAssignmentCommand = new Command<AssignmentModel>(OnDeleteAssignment);
            ChangeIsCompletedCommand = new Command<AssignmentModel>(HandleChangeIsCompleted);
            GetGroupByIsCompleted().ContinueWith(t =>
            {
                GroupByIsCompleted = t.Result;
            });
            Assignment = new AssignmentModel();

            //PreviousWeekCommand = new Command<DateTime>(PreviousWeekCommandHandler);
            //NextWeekCommand = new Command<DateTime>(NextWeekCommandHandler);
            //DayCommand = new Command<DayModel>(DayCommandHandler);

        }

        public  void OnAppearing()
        {
            IsBusy = true;
        }
        public async Task RefreshList()
        {
            GroupByIsCompleted = await GetGroupByIsCompleted();
        }



        //private void DayCommandHandler(DayModel day)
        //{
        //    SetActiveDay(day);
        //    ///CreateQueryForTasks(day.Date);
        //}
        //private void PreviousWeekCommandHandler(DateTime startDate)
        //{
        //    Week = _dateService.GetWeek(startDate.AddDays(-1));
        //    DaysList = new ObservableCollection<DayModel>(_dateService.GetDayList(Week.StartDay, Week.LastDay));
        //    SetActiveDay();
        //}
        //private void NextWeekCommandHandler(DateTime lastDate)
        //{
        //    Week = _dateService.GetWeek(lastDate.AddDays(1));
        //    DaysList = new ObservableCollection<DayModel>(_dateService.GetDayList(Week.StartDay, Week.LastDay));
        //    SetActiveDay();
        //}
       
        //private void SetActiveDay(DayModel day = null)
        //{
        //    ResetActiveDay();
        //    if (day != null)
        //    {
        //        _selectedDay = day;
        //        day.State = DayStateEnum.Active;
        //    }
        //    else
        //    {
        //        var selectedDate = DaysList.FirstOrDefault(d => d.Date == _selectedDay.Date);
        //        if (selectedDate != null)
        //        {
        //            selectedDate.State = DayStateEnum.Active;
        //        }
        //    }
        //}

        //private void ResetActiveDay()
        //{
        //    var selectedDay = DaysList?.FirstOrDefault(d => d.State.Equals(DayStateEnum.Active));
        //    if (selectedDay != null)
        //    {
        //        selectedDay.State = selectedDay.Date < DateTime.Now.Date ? DayStateEnum.Past : DayStateEnum.Normal;
        //    }
        //}

        private async Task<ILookup<string, AssignmentModel>> GetGroupByIsCompleted()
        {
            return (await App.AssignmentsDB.GetItemsAsync())
                                .OrderBy(t => t.IsCompleted)
                                .ToLookup(t => t.IsCompleted ? "Completed" : "Active");
        }

        async Task ExecuteLoadAssignmentCommand()
        {
            try
            {
                IsBusy = true;
                assignments.Clear();
                var assList = await App.AssignmentsDB.GetItemsAsync(); ///GetSortedByDate(DateTime date);
                foreach (var ass in assList)
                {
                    assignments.Add(ass);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void HandleChangeIsCompleted(AssignmentModel assignment)
        {
            if (assignment == null)
            {
                return;
            }
            assignment.IsCompleted = !assignment.IsCompleted;
            await App.AssignmentsDB.AddItemAsync(assignment);
            await ExecuteLoadAssignmentCommand();
        }
        private async void OnAddAssignment(object obj)
        {
            await Shell.Current.GoToAsync(nameof(AssignmentAddingPage));
        }
        private async void OnEditAssignment(AssignmentModel assignment)
        {
            await Navigation.PushAsync(new AssignmentAddingPage(assignment));
        }

        private async void OnDeleteAssignment(AssignmentModel assignment)
        {
            if (assignment == null)
            {
                return;
            }
            await App.AssignmentsDB.DeleteItemAsync(assignment.ID);
            await ExecuteLoadAssignmentCommand();
        }
    }
}
