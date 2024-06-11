using App1.Models;
using App1.Views;
using App1.Views.Popups;
using App1.Views.Popups.EditPopup;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using static App1.Models.AssignmentModel;


namespace App1.ViewModels
{
    public class EditViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command LoadTagPopupCommand { get; }
        public Command FoldersPopupCommand { get; }
        public Command PriorityPopupCommand { get; }
        public Command DatePopupCommand { get; }
        public Command DeleteTagCommand { get; }
        public Command NotificationPopupCommand { get; }

        public Command ChangeIsCompletedCommand { get; }
        public Command AddChildCommand { get; }
        public Command DeleteChildCommand { get; }

        public ObservableCollection<TagModel> TagList { get; }
        public ObservableCollection<AssignmentModel> ChildList { get; }
        public bool isFromPopup { get; set; }
        private EnumPriority selectedPriority { get; set; }
        public EnumPriority SelectedPriority
        {
            get { return selectedPriority; }
            set
            {
                if (selectedPriority != value)
                {
                    selectedPriority = value;
                    OnPropertyChanged();
                }
            }
        }
        private ListModel selectedFolder { get; set; }
        public ListModel SelectedFolder
        {
            get { return selectedFolder; }
            set
            {
                if (selectedFolder != value)
                {
                    selectedFolder = value;
                    OnPropertyChanged();
                }
            }
        }
        public Command HandleChangeIsCompletedCommand { get; }
        public Command DeleteCommand { get; }
        public EditViewModel(INavigation navigation)
        {
            AddChildCommand = new Command(AddChild);
            DeleteChildCommand = new Command<AssignmentModel>(DeleteChild);
            isFromPopup = false;
            HandleChangeIsCompletedCommand = new Command(HandleChangeIsCompleted);
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            TagList = new ObservableCollection<TagModel>();
            ChildList = new ObservableCollection<AssignmentModel>();
            LoadTagPopupCommand = new Command(ExecuteLoadTagPopup);
            FoldersPopupCommand = new Command(ExecuteFoldersPopup);
            PriorityPopupCommand = new Command(ExecutePriorityPopup);
            SelectedFolder = new ListModel();
            DeleteCommand = new Command(OnDelete);
            DeleteTagCommand = new Command<TagModel>(DeleteTag);
            DatePopupCommand = new Command((arg) =>
            {
                var DatePickerDat = arg as DatePicker;
                DatePickerDat.IsEnabled = true;
                DatePickerDat.IsVisible = false;
                DatePickerDat.Focus();
            });
            NotificationPopupCommand = new Command(ExecuteNotification);
            Navigation = navigation;
            this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
            Assignment = new AssignmentModel();
            ChangeIsCompletedCommand = new Command<AssignmentModel>(ChangeIsCompleted);
        }
        private async void OnSave()
        {
            var assignment = Assignment;
            if (string.IsNullOrEmpty(assignment.Name))
            {
                assignment.Name = "Без названия";
            }

            await App.AssignmentsDB.AddItemAsync(assignment);
            await Navigation.PopAsync();
        }
        private async void UpdateTags()
        {
            TagList.Clear();
            foreach (var tagId in Assignment.Tags)
            {
                var tag =await  App.AssignmentsDB.GetTagAsync(tagId.ID);
                if (tag != null)
                {
                    TagList.Add(tag);
                }
            }

        }
        private async void UpdateChilds()
        {
            ChildList.Clear();
            foreach (var childId in Assignment.Childs)
            {
                var child = await App.AssignmentsDB.GetItemtAsync(childId.ID);
                if (child != null)
                {
                    ChildList.Add(child);
                }
            }

        }

        private async void AddChild()
        {

            await Navigation.PushAsync(new AssignmentAddingPage(true));
            MessagingCenter.Subscribe<AssignmentModel>(this, "PopupChildClosed", (sender) =>
            {
                Assignment.AddChild(sender);
                UpdateChilds();
            });
        }
        private void DeleteChild(AssignmentModel assignment)
        {
            Assignment.RemoveChild(assignment);
            UpdateChilds();
        }


        private void DeleteTag(TagModel tag)
        {
            Assignment.RemoveTag(tag);
            UpdateTags();
        }
        private async void OnDelete()
        {
            var assignment = Assignment;
            assignment.IsDeleted = true;
            await App.AssignmentsDB.AddItemAsync(assignment);
            await Navigation.PopAsync();
        }
        private  void HandleChangeIsCompleted()
        {
            Assignment.IsCompleted = !Assignment.IsCompleted;
        }
        private async void ExecuteLoadTagPopup()
        {
            MessagingCenter.Unsubscribe<TagModel>(this, "TagChanged");
            MessagingCenter.Subscribe<TagModel>(this, "TagChanged",
                (sender) =>
                {
                    Assignment.AddTag(sender);
                    UpdateTags();
                });
            await Navigation.PushPopupAsync(new TagPopupPage());
        }

        private void ChangeIsCompleted(AssignmentModel assignment)
        {
            assignment.ChangeIsCompleted();
            UpdateChilds();
        }
        private async void ExecuteFoldersPopup()
        {
            MessagingCenter.Unsubscribe<ListModel>(this, "FolderChanged");
            MessagingCenter.Subscribe<ListModel>(this, "FolderChanged",
                (sender) =>
                {
                    Assignment.FolderName = sender.Name;
                    SelectedFolder = sender;
                });
            await Navigation.PushPopupAsync(new EditFoldersPopupPage());
        }

        private async void ExecutePriorityPopup()
        {
            MessagingCenter.Unsubscribe<AssignmentModel>(this, "PriorityChanged");
            MessagingCenter.Subscribe<AssignmentModel>(this, "PriorityChanged",
                (sender) =>
                {
                    Assignment.Priority = sender.Priority;
                });
            await Navigation.PushPopupAsync(new EditPriorityPopupPage());
        }

        private async void ExecuteNotification()
        {
            MessagingCenter.Unsubscribe<AssignmentModel>(this, "Date1Changed");
            MessagingCenter.Subscribe<AssignmentModel>(this, "Date1Changed",
                (sender) =>
                {
                    Assignment.ExecutionDate = sender.ExecutionDate;
                    Assignment.NotificationTime = sender.NotificationTime;
                    Assignment.HasNotification = sender.HasNotification;
                    Assignment.NotificationTimeMultiplier = sender.NotificationTimeMultiplier;
                });
            await Navigation.PushAsync(new DateSelectionPage(Assignment,false), false);
        }

        private async void OnCancel()
        {
            if (isFromPopup)
            {
                await Navigation.PopAsync(false);
                await Navigation.PushPopupAsync(new AssignmentAddingPage(Assignment), false);
            }
            await Navigation.PopAsync();
        }
    }
}
