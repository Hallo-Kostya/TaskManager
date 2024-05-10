using App1.Models;
using App1.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class AssignmentViewModel : BaseAssignmentViewModel
    {

        
        private ObservableCollection<AssignmentModel> _assignments;
        public ObservableCollection<AssignmentModel> assignments
        {
            get => _assignments;
            set => SetProperty(ref _assignments, value);

        }
        private ObservableCollection<AssignmentModel> completedAssignments;
        public ObservableCollection<AssignmentModel> CompletedAssignments
        {
            get => completedAssignments;
            set => SetProperty(ref completedAssignments, value);

        }





        private TagModel selectedtag { get; set; }
        public TagModel SelectedTag
        {
            get { return selectedtag; }
            set
            {
                if (selectedtag != value)
                {
                    selectedtag = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<TagModel> tagList;
        public ObservableCollection<TagModel> TagList
        {
            get => tagList;
            set => SetProperty(ref tagList, value);
        }


        public Command LoadAssignmentCommand { get; }
        public Command LoadTagsCommand { get; }
        public Command AddAssignmentCommand { get; }
        public Command EditAssignmentCommand { get; }
        public Command DeleteAssignmentCommand { get; }
        public Command SearchCommand { get; }
        public Command ChangeIsCompletedCommand { get;}
        public INavigation Navigation { get; set; }
        




        public Command FilterByPriorityCommand { get; }
        public Command FilterByTagCommand { get; }




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
            FilterByPriorityCommand = new Command(OnFiltered);
            TagList = new ObservableCollection<TagModel>();
            //LoadTagsCommand = new Command(async () => await ExecuteLoadTagsCommand());
            FilterByTagCommand = new Command(OnTagFiltered);
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
                var a = (await App.AssignmentsDB.GetItemsAsync());
                var assList = a.Where(t => (t.IsDeleted==false) && (t.IsCompleted==false) );
                var completedList = a.Where(t => (t.IsDeleted==false) && (t.IsCompleted==true));///GetSortedByDate(DateTime date);
                assignments = new ObservableCollection<AssignmentModel>(assList);
                CompletedAssignments = new ObservableCollection<AssignmentModel>(completedList);
                TagList.Clear();
                var tags = (await App.AssignmentsDB.GetTagsAsync()).Where(x=>!string.IsNullOrWhiteSpace(x.Name)).Distinct().ToList();
                foreach (var tag in tags)
                    TagList.Add(tag);
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
        private async void OnTagFiltered()
        {
            try
            {
                if (SelectedTag.Name!="��� ������")
                {
                    var assList = (await App.AssignmentsDB.GetItemsAsync()).Where(t => (t.IsDeleted == false&& t.IsCompleted==false) && (t.Tag == SelectedTag.Name)); ///GetSortedByDate(DateTime date);
                    assignments = new ObservableCollection<AssignmentModel>(assList);
                }
                else
                {
                    await ExecuteLoadAssignmentCommand();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async void OnFiltered()
        {
            try
            {
                var assList = (await App.AssignmentsDB.GetItemsAsync()).Where(t => t.IsDeleted == false && t.IsCompleted==false).OrderByDescending(t => (int)t.Priority); ///GetSortedByDate(DateTime date);
                assignments = new ObservableCollection<AssignmentModel>(assList);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async void OnAddAssignment(object obj)
        {
            
            await Navigation.PushPopupAsync(new AssignmentAddingPage());
            MessagingCenter.Unsubscribe<AssignmentAddingViewModel>(this, "PopupClosed");
            MessagingCenter.Subscribe<AssignmentAddingViewModel>(this, "PopupClosed", async (sender) => await ExecuteLoadAssignmentCommand());
        }
        private async void OnSearchAssignment(object obj)
        {
            await Shell.Current.GoToAsync(nameof(SearchPage));
        }
        private async void OnEditAssignment(AssignmentModel assignment)
        {
            await Navigation.PushPopupAsync(new AssignmentAddingPage(assignment));
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
