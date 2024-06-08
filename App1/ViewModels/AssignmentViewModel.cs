using App1.Data;
using App1.Models;
using App1.Services.Notifications;
using App1.Views;
using App1.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

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
        private ListModel _selectedfolder;
        public ListModel SelectedFolder
        {
            get { return _selectedfolder; }
            set { _selectedfolder = value; OnPropertyChanged(); }
        }

        private ObservableCollection<AssignmentModel> completedAssignments;
        public ObservableCollection<AssignmentModel> CompletedAssignments
        {
            get => completedAssignments;
            set => SetProperty(ref completedAssignments, value);
        }
       


        INotificationManager notificationManager;

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


        public Command LoadAssignmentCommand { get; }
        public Command LoadTagsCommand { get; }
        public Command AddAssignmentCommand { get; }
        public Command EditAssignmentCommand { get; }
        public Command DeleteAssignmentCommand { get; }
        public Command SearchCommand { get; }
        public Command ChangeIsCompletedCommand { get; }
        public Command TagSelectPopupCommand { get; }
        public Command MeetBallsPopupCommand { get; }

        public INavigation Navigation { get; set; }





  
        public Command FilterByTagCommand { get; }




        
        public AssignmentViewModel(INavigation _navigation)
        {
            LoadAssignmentCommand = new Command(async () => await ExecuteLoadAssignmentCommand());
            AddAssignmentCommand = new Command(OnAddAssignment);
            EditAssignmentCommand = new Command<AssignmentModel>(OnEditAssignment);
            Navigation = _navigation;
            DeleteAssignmentCommand = new Command<AssignmentModel>(OnDeleteAssignment);
            ChangeIsCompletedCommand = new Command<AssignmentModel>(HandleChangeIsCompleted);
            SearchCommand = new Command(OnSearchAssignment);
            SelectedTag = new TagModel();
            SelectedTag.Name = "без тега";
            notificationManager = DependencyService.Get<INotificationManager>();
           
            SelectedFolder = new ListModel();
            SelectedFolder.Name = "Мои дела";
            isFilteredByTag = Preferences.Get("IsFilteredByTag", false);
            isFilteredByPriority = Preferences.Get("IsFilteredByPriority", false);
            isFilteredByDate = Preferences.Get("IsFilteredByDate", false);
            TagSelectPopupCommand = new Command(ExecuteTagSelectPopup);
            MeetBallsPopupCommand = new Command(ExecuteMeetBallsPopup);
            MessagingCenter.Subscribe<ListModel>(this, "UpdatePage", async (sender) =>
            {
                SelectedFolder = sender;
                await ExecuteLoadAssignmentCommand();
            });
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

                if (SelectedFolder.Name != "Мои дела")
                {
                    filteredAssignments = filteredAssignments.Where(t => t.FolderName == SelectedFolder.Name);
                }

                // Фильтрация по тегу
                if (IsFilteredByTag && SelectedTag.Name != "без тега")
                {
                    filteredAssignments = filteredAssignments.Where(t => t.Tag == SelectedTag.Name);
                }

                // Применение сортировки
                if (IsFilteredByPriority)
                {
                    filteredAssignments = filteredAssignments.OrderByDescending(x => (int)x.Priority);
                }
                else if (IsFilteredByDate)
                {
                    filteredAssignments = filteredAssignments
                        .OrderBy(x => x.ExecutionDate.Date)
                        .ThenBy(x => x.ExecutionDate.TimeOfDay);
                }

                var groupedAssignments = filteredAssignments
                    .Where(t => t.IsCompleted == false)
                    .ToList();

                assignments = new ObservableCollection<AssignmentModel>(groupedAssignments);

                var completedList = filteredAssignments
                    .Where(t => t.IsCompleted == true)
                    .ToList();

                CompletedAssignments = new ObservableCollection<AssignmentModel>(completedList);
                //var groupedAssignments = filteredAssignments
                //    .OrderBy(x => x.ExecutionDate.Date) // Сортировка по дате выполнения
                //    .ThenBy(x => x.ExecutionDate.TimeOfDay) // Сортировка по времени выполнения
                //    .GroupBy(x => x.ExecutionDate.Date) // Группировка по дате
                //    .SelectMany(group => group.OrderByDescending(x => (int)x.Priority)) // Сортировка внутри группы по приоритету и разворачивание в единую последовательность
                //    .ToList();

                //assignments = new ObservableCollection<AssignmentModel>(groupedAssignments);
            }
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
        private async void OnAddAssignment()
        {
            if (SelectedFolder.Name != "Мои дела")
            {
                var fold = SelectedFolder;
                await Navigation.PushPopupAsync(new AssignmentAddingPage(fold));
            }
            else
            {
                await Navigation.PushPopupAsync(new AssignmentAddingPage());
            }

            MessagingCenter.Unsubscribe<AssignmentAddingViewModel>(this, "PopupClosed");
            MessagingCenter.Subscribe<AssignmentAddingViewModel>(this, "PopupClosed", async (sender) => await ExecuteLoadAssignmentCommand());
        }
        private async void OnSearchAssignment(object obj)
        {
            await Shell.Current.GoToAsync(nameof(SearchPage));
        }
        private async void OnEditAssignment(AssignmentModel assignment)
        {

            await Navigation.PushAsync(new EditPage(assignment, false));
        }

        private async void OnDeleteAssignment(AssignmentModel assignment)
        {
            if (assignment == null)
            {
                return;
            }
            assignment.IsDeleted = true;

            if (assignment.HasNotification)
            {
                notificationManager.CancelNotification(assignment.ID);  // Отмена уведомления по идентификатору задачи
            }
            await App.AssignmentsDB.AddItemAsync(assignment);
            await ExecuteLoadAssignmentCommand();
        }

        private async void ExecuteTagSelectPopup()
        {
            await Navigation.PushPopupAsync(new TagSelectPopupPage());
            MessagingCenter.Unsubscribe<TagModel>(this, "TagSelected");
            MessagingCenter.Subscribe<TagModel>(this, "TagSelected", async (sender) =>
            {
                try
                {
                    SelectedTag = sender;

                    if (sender.Name != "без тега")
                    {
                            IsFilteredByTag = true;
                    }
                    else
                    {
                        IsFilteredByTag = false;
                    }
                    await ExecuteLoadAssignmentCommand();
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }

        private async void ExecuteMeetBallsPopup()
        {
            await Navigation.PushPopupAsync(new MeetBallsPopupPage());
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
