using App1.Models;
using App1.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class AssignmentViewModel : BaseAssignmentViewModel
    {

        public Command LoadAssignmentCommand { get; }
        private ObservableCollection<AssignmentModel> _assignments;
        public ObservableCollection<AssignmentModel> assignments
        {
            get => _assignments;
            set => SetProperty(ref _assignments, value);

        }
        public Command AddAssignmentCommand { get; }
        public Command EditAssignmentCommand { get; }
        public Command DeleteAssignmentCommand { get; }
        public Command SearchCommand { get; }
        public ILookup<string, AssignmentModel> GroupByIsCompleted { get; set; }
        public Command ChangeIsCompletedCommand { get; set; }
        public INavigation Navigation { get; set; }
        public Command ToArchiveCommand { get; }

        public Command FilterByPriorityCommand { get; }





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
            AddAssignmentCommand = new Command(OnAddAssignment);
            EditAssignmentCommand = new Command<AssignmentModel>(OnEditAssignment);
            Navigation = _navigation;
            DeleteAssignmentCommand = new Command<AssignmentModel>(OnDeleteAssignment);
            ChangeIsCompletedCommand = new Command<AssignmentModel>(HandleChangeIsCompleted);
            SearchCommand = new Command(OnSearchAssignment);
            ToArchiveCommand = new Command(OnArchive);
            FilterByPriorityCommand = new Command(OnFiltered);

            //PreviousWeekCommand = new Command<DateTime>(PreviousWeekCommandHandler);
            //NextWeekCommand = new Command<DateTime>(NextWeekCommandHandler);
            //DayCommand = new Command<DayModel>(DayCommandHandler);

        }

        public  void OnAppearing()
        {
            IsBusy = true;
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

        async Task ExecuteLoadAssignmentCommand()
        {
            IsBusy = true;
            try
            {
                var assList = (await App.AssignmentsDB.GetItemsAsync()).Where(t => t.IsDeleted == false).OrderBy(t => t.IsCompleted); ///GetSortedByDate(DateTime date);
                assignments = new ObservableCollection<AssignmentModel>(assList);
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
        private async  void HandleChangeIsCompleted(AssignmentModel assignment)
        {
            if (assignment == null)
            {
                return;
            }
            assignment.IsCompleted = !assignment.IsCompleted;
            await App.AssignmentsDB.AddItemAsync(assignment);
            IsBusy = true;
        }
        private async void OnFiltered()
        {
            try
            {
                var assList = (await App.AssignmentsDB.GetItemsAsync()).Where(t => t.IsDeleted == false).OrderBy(t => t.IsCompleted).ThenByDescending(t => (int)t.Priority); ///GetSortedByDate(DateTime date);
                assignments = new ObservableCollection<AssignmentModel>(assList);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async void OnAddAssignment(object obj)
        {
            await Shell.Current.GoToAsync(nameof(AssignmentAddingPage));
        }
        private async void OnArchive(object obj)
        {
            await Shell.Current.GoToAsync(nameof(ArchivePage));
        }
        private async void OnSearchAssignment(object obj)
        {
            await Shell.Current.GoToAsync(nameof(SearchPage));
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
            assignment.IsDeleted = true;
            await App.AssignmentsDB.AddItemAsync(assignment);
            await ExecuteLoadAssignmentCommand();
        }

    }

}
