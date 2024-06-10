using App1.Data;
using App1.Models;
using App1.Views;
using App1.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class FolderPageViewModel: BaseAssignmentViewModel
    {
        public INavigation Navigation { get; }
        public Command LoadAssignmentCommand { get; }
        public Command LoadTagsCommand { get; }
        public Command AddAssignmentCommand { get; }
        public Command EditAssignmentCommand { get; }
        public Command DeleteAssignmentCommand { get; }
        public Command SearchCommand { get; }
        public Command ChangeIsCompletedCommand { get; }
        public Command FilterByPriorityCommand { get; }
        public Command FilterByTagCommand { get; }
        private string groupedBy;
        public string GroupedBy
        {
            get => groupedBy;
            set
            {
                SetProperty(ref groupedBy, value);
                Preferences.Set("GroupedBy", value); // Сохраняем состояние в Preferences
            }
        }
        private bool isFilteredByTag;
        public bool IsFilteredByTag
        {
            get => isFilteredByTag;
            set
            {
                SetProperty(ref isFilteredByTag, value);
                Preferences.Set("IsFilteredByTag", value); // Сохраняем состояние в Preferences
            }
        }
        private bool isFilteredByPriority;
        public bool IsFilteredByPriority
        {
            get => isFilteredByPriority;
            set
            {
                SetProperty(ref isFilteredByPriority, value);
                Preferences.Set("IsFilteredByPriority", value); // Сохраняем состояние в Preferences
            }
        }
        private bool isFilteredByDate;
        public bool IsFilteredByDate
        {
            get => isFilteredByDate;
            set
            {
                SetProperty(ref isFilteredByDate, value);
                Preferences.Set("IsFilteredByDate", value); // Сохраняем состояние в Preferences
            }
        }
        private ListModel _folder;
        public ListModel Folder
        {
            get { return _folder; }
            set { _folder = value; OnPropertyChanged(); }
        }
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
        public FolderPageViewModel(INavigation navigation)
        {
            LoadAssignmentCommand=new Command(async () => await ExecuteLoadAssignmentCommand());
            AddAssignmentCommand = new Command(OnAddAssignment);
            EditAssignmentCommand = new Command<AssignmentModel>(OnEditAssignment);
            Navigation = navigation;
            Folder = new ListModel();
            DeleteAssignmentCommand = new Command<AssignmentModel>(OnDeleteAssignment);
            ChangeIsCompletedCommand = new Command<AssignmentModel>(HandleChangeIsCompleted);
            SearchCommand = new Command(OnSearchAssignment);
            TagList = new ObservableCollection<TagModel>();
            //LoadTagsCommand = new Command(async () => await ExecuteLoadTagsCommand());
            groupedBy = Preferences.Get("GroupedBy", "None");
            isFilteredByTag = Preferences.Get("IsFilteredByTag", false);
            isFilteredByPriority = Preferences.Get("IsFilteredByPriority", false);
            isFilteredByDate = Preferences.Get("IsFilteredByDate", false);
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
        async Task ExecuteLoadAssignmentCommand()
        {
            IsBusy = true;
            try
            {
                var a = await App.AssignmentsDB.GetItemsAsync();
                foreach (var assignment in a)
                {
                    assignment.CheckIfOverdue();
                }

                // Начальная фильтрация по папке
                IEnumerable<AssignmentModel> filteredAssignments = a.Where(t => t.IsDeleted == false);

                if (Folder.Name != "Мои дела")
                {
                    filteredAssignments = filteredAssignments.Where(t => t.FolderName == Folder.Name);
                }

                // Фильтрация по тегу
                if (IsFilteredByTag && SelectedTag.Name != "без тега")
                {
                    filteredAssignments = filteredAssignments.Where(t => t.Tags.Any(tag => tag == SelectedTag.ID));
                }

                // Применение сортировки
                //if (IsFilteredByPriority)
                //{
                //    filteredAssignments = filteredAssignments.OrderByDescending(x => (int)x.Priority);
                //}
                //else if (IsFilteredByDate)
                //{
                //    filteredAssignments = filteredAssignments
                //        .OrderBy(x => x.ExecutionDate.Date)
                //        .ThenBy(x => x.ExecutionDate.TimeOfDay);
                //}

                //var groupedAssignments = filteredAssignments
                //    .Where(t => t.IsCompleted == false)
                //    .ToList();

                //assignments = new ObservableCollection<AssignmentModel>(groupedAssignments);

                //var completedList = filteredAssignments
                //    .Where(t => t.IsCompleted == true)
                //    .ToList();

                //CompletedAssignments = new ObservableCollection<AssignmentModel>(completedList);
                IEnumerable<IGrouping<object, AssignmentModel>> groupedAssignments;
                switch (GroupedBy)
                {
                    case "Date":
                        groupedAssignments = filteredAssignments
                            .GroupBy(x => (object)x.ExecutionDate.Date)
                            .OrderByDescending(group => group.Key);
                        break;
                    case "Priority":
                        groupedAssignments = filteredAssignments
                            .GroupBy(x => (object)(int)x.Priority)
                            .OrderByDescending(group => group.Key);
                        break;
                    case "Tag":
                        groupedAssignments = filteredAssignments
                            .SelectMany(x => x.Tags.Select(tagId => new { TagId = tagId, Assignment = x }))
                            .GroupBy(x => (object)App.AssignmentsDB.GetTagAsync(x.TagId).Result.Name, x => x.Assignment)
                            .OrderByDescending(group => group.Key);
                        break;
                    default:
                        groupedAssignments = filteredAssignments
                            .GroupBy(x => (object)null); // Нет группировки
                        break;
                }

                var sortedAssignments = groupedAssignments
                   .SelectMany(group =>
                   {
                       var sortedGroup = group.AsEnumerable();
                       if (IsFilteredByPriority)
                       {
                           sortedGroup = sortedGroup.OrderByDescending(x => (int)x.Priority);
                       }
                       else if (IsFilteredByDate)
                       {
                           sortedGroup = sortedGroup
                               .OrderBy(x => x.ExecutionDate.Date)
                               .ThenBy(x => x.ExecutionDate.TimeOfDay);
                       }
                       return sortedGroup;
                   })
                    .ToList();
                assignments = new ObservableCollection<AssignmentModel>(sortedAssignments.Where(t => t.IsCompleted == false));
                CompletedAssignments = new ObservableCollection<AssignmentModel>(sortedAssignments.Where(t => t.IsCompleted == true));
            }


            //    .OrderBy(x => x.ExecutionDate.Date) // Сортировка по дате выполнения
            //    .ThenBy(x => x.ExecutionDate.TimeOfDay) // Сортировка по времени выполнения
            //    .GroupBy(x => x.ExecutionDate.Date) // Группировка по дате
            //    .SelectMany(group => group.OrderByDescending(x => (int)x.Priority)) // Сортировка внутри группы по приоритету и разворачивание в единую последовательность
            //    .ToList();

            //assignments = new ObservableCollection<AssignmentModel>(groupedAssignments);

            catch (Exception ex)
            {
                // Обработка исключений
                Console.WriteLine($"Error in ExecuteLoadAssignmentCommand: {ex.Message}");
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
            IsBusy = true;
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
        private async void ExecuteMeetBallsPopup()
        {
            await Navigation.PushPopupAsync(new MeetBallsPopupPage());
            MessagingCenter.Unsubscribe<FilterSelectPopupViewModel, string>(this, "GroupSelected");
            MessagingCenter.Subscribe<FilterSelectPopupViewModel, string>(this, "GroupSelected", async (sender, arg) => {
                switch (arg)
                {
                    case "Tag":
                        GroupedBy = "Tag";
                        break;
                    case "Date":
                        GroupedBy = "Date";
                        break;
                    case "Priority":
                        GroupedBy = "Priority";
                        break;
                    default:
                        GroupedBy = "None";
                        break;
                }
                await ExecuteLoadAssignmentCommand();
            });
            MessagingCenter.Unsubscribe<FilterSelectPopupViewModel>(this, "FilterByPrioritySelected");
            MessagingCenter.Subscribe<FilterSelectPopupViewModel>(this, "FilterByPrioritySelected", async (sender) => {
                IsFilteredByPriority = true;
                IsFilteredByDate = false;
                await ExecuteLoadAssignmentCommand();
            });
            MessagingCenter.Unsubscribe<FilterSelectPopupViewModel>(this, "FilterByDateSelected");
            MessagingCenter.Subscribe<FilterSelectPopupViewModel>(this, "FilterByDateSelected", async (sender) => {
                IsFilteredByDate = true;
                IsFilteredByPriority = false;
                await ExecuteLoadAssignmentCommand();
            });
            MessagingCenter.Unsubscribe<FilterSelectPopupViewModel>(this, "DefaultFilterSelected");
            MessagingCenter.Subscribe<FilterSelectPopupViewModel>(this, "DefaultFilterSelected", async (sender) => {
                IsFilteredByDate = false;
                IsFilteredByPriority = false;
                await ExecuteLoadAssignmentCommand();
            });

        }

    }
}
