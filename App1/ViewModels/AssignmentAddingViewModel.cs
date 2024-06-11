using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using App1.Data;
using App1.Models;
using App1.Services.Notifications;
using App1.Views;
using App1.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using static App1.Models.AssignmentModel;

namespace App1.ViewModels
{
    public class AssignmentAddingViewModel : BaseAssignmentViewModel
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command LoadTagPopupCommand { get; }
        public Command FoldersPopupCommand { get; }
        public Command PriorityPopupCommand { get; }
        public Command DatePopupCommand {  get; }
        public Command NotificationPopupCommand { get; }
        public Command OpenFullscreenCommand { get; }



        public Command DeleteTagCommand { get; }
        public ObservableCollection<TagModel> TagList { get; }
        public INavigation Navigation { get; set; }
        public Command BackgroundClickedCommand { get; }
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

        public bool TagLoaded { get; set; } = false;
        public bool IsChildAssignment { get; set; }


        public AssignmentAddingViewModel(INavigation navigation)
        {
            OpenFullscreenCommand = new Command(OpenFullscreen);
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            TagList = new ObservableCollection<TagModel>();
            LoadTagPopupCommand = new Command(ExecuteLoadTagPopup);
            FoldersPopupCommand = new Command(ExecuteFoldersPopup);
            PriorityPopupCommand = new Command(ExecutePriorityPopup);
            SelectedFolder = new ListModel();
            DeleteTagCommand = new Command<TagModel>(DeleteTag);
            DatePopupCommand = new Command((arg) =>
            {
                var DatePickerDate = arg as DatePicker;
                DatePickerDate.IsEnabled = true;
                DatePickerDate.IsVisible = false;
                DatePickerDate.Focus();
            });
            NotificationPopupCommand = new Command(ExecuteNotification);
            Navigation = navigation;
            this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
            Assignment = new AssignmentModel();
            BackgroundClickedCommand = new Command(OnBackgroundClicked);

        }
        private async void OnSave()
        {
            var assignment = Assignment;
            if (string.IsNullOrEmpty(assignment.Name))
            {
                assignment.Name = "Без названия";
            }
            if (!IsChildAssignment)
            {
                await App.AssignmentsDB.AddItemAsync(assignment);
                await Navigation.PopPopupAsync();
                MessagingCenter.Send(this, "PopupClosed");
            }
            else
            {
                await Navigation.PopPopupAsync();
                MessagingCenter.Send(this, "PopupChildClosed");
            }
        }
        private async void UpdateTags()
        {
            TagList.Clear();
            foreach (var tagId in Assignment.Tags)
            {
                var tag = await App.AssignmentsDB.GetTagAsync(tagId.ID);
                if (tag != null)
                {
                    TagList.Add(tag);
                }
            }
        }
        private void DeleteTag(TagModel tag)
        {
            Assignment.RemoveTag(tag);
            UpdateTags();
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

        private async void ExecuteFoldersPopup()
        {
            MessagingCenter.Unsubscribe<ListModel>(this, "FolderChanged");
            MessagingCenter.Subscribe<ListModel>(this, "FolderChanged",
                (sender) =>
                {
                    Assignment.FolderName = sender.Name;
                    SelectedFolder = sender;
                });
            await Navigation.PushPopupAsync(new FoldersPopupPage());
        }

        private async void ExecutePriorityPopup()
        {
            MessagingCenter.Unsubscribe<AssignmentModel>(this, "PriorityChanged");
            MessagingCenter.Subscribe<AssignmentModel>(this, "PriorityChanged",
                (sender) =>
                {
                    Assignment.Priority = sender.Priority;
                    SelectedPriority = sender.Priority;
                });
            await Navigation.PushPopupAsync(new PriorityPopupPage());
        }


        private async void ExecuteNotification()
        {
            MessagingCenter.Unsubscribe<AssignmentModel>(this, "DateChanged");
            MessagingCenter.Subscribe<AssignmentModel>(this, "DateChanged",
                (sender) =>
                {
                    Assignment.ExecutionDate = sender.ExecutionDate;
                    Assignment.NotificationTime = sender.NotificationTime;
                    Assignment.HasNotification = sender.HasNotification;
                    Assignment.NotificationTimeMultiplier = sender.NotificationTimeMultiplier;
                });
            await Navigation.PushAsync(new DateSelectionPage(Assignment,true),false);
            await Navigation.PopAllPopupAsync();
        }
        private async void OpenFullscreen()
        {
            var assignment = Assignment;
            await Navigation.PushAsync(new EditPage(assignment,true),false);
            await Navigation.PopAllPopupAsync(false);
        }
        private async void OnBackgroundClicked()
        {
            await Navigation.PopPopupAsync();
            MessagingCenter.Send(this, "PopupClosed");
        }
        private async void OnCancel()
        {
            await Navigation.PopPopupAsync();
            MessagingCenter.Send(this, "PopupClosed");
        }
    }
}
