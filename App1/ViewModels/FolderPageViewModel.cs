using App1.Data;
using App1.Models;
using App1.Views;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            FilterByPriorityCommand = new Command(OnFiltered);
            TagList = new ObservableCollection<TagModel>();
            //LoadTagsCommand = new Command(async () => await ExecuteLoadTagsCommand());
            FilterByTagCommand = new Command(OnTagFiltered);
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
                var a = (await App.AssignmentsDB.GetItemsAsync());
                var assList = a.Where(t => (t.IsDeleted == false) && (t.IsCompleted == false) && (t.FolderName==Folder.Name));
                var completedList = a.Where(t => (t.IsDeleted == false) && (t.IsCompleted == true)&&(t.FolderName==Folder.Name));///GetSortedByDate(DateTime date);
                assignments = new ObservableCollection<AssignmentModel>(assList);
                CompletedAssignments = new ObservableCollection<AssignmentModel>(completedList);
                TagList.Clear();
                var tags = (await App.AssignmentsDB.GetTagsAsync()).Where(x => !string.IsNullOrWhiteSpace(x.Name)).Distinct().ToList();
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
        private async void OnTagFiltered()
        {
            try
            {
                if (SelectedTag.Name != "Все Задачи")
                {
                    var assList = (await App.AssignmentsDB.GetItemsAsync()).Where(t => (t.IsDeleted == false && t.IsCompleted == false) && (t.Tag == SelectedTag.Name) && (t.FolderName == Folder.Name)); ///GetSortedByDate(DateTime date);
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
                var assList = (await App.AssignmentsDB.GetItemsAsync()).Where(t => t.IsDeleted == false && t.IsCompleted == false && t.FolderName == Folder.Name).OrderByDescending(t => (int)t.Priority); ///GetSortedByDate(DateTime date);
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
