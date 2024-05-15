﻿using App1.Models;
using App1.Views.Popups;
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
        public ObservableCollection<string> TagList { get; }
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
        public EditViewModel(INavigation navigation)
        {
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            TagList = new ObservableCollection<string>();
            LoadTagPopupCommand = new Command(ExecuteLoadTagPopup);
            FoldersPopupCommand = new Command(ExecuteFoldersPopup);
            PriorityPopupCommand = new Command(ExecutePriorityPopup);
            SelectedFolder = new ListModel();
            DatePopupCommand = new Command((arg) =>
            {
                var DatePickerDat = arg as DatePicker;
                DatePickerDat.IsEnabled = true;
                DatePickerDat.IsVisible = false;
                DatePickerDat.Focus();
            });
            Navigation = navigation;
            this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
            Assignment = new AssignmentModel();
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
        private async void ExecuteLoadTagPopup()
        {
            MessagingCenter.Unsubscribe<TagModel>(this, "TagChanged");
            MessagingCenter.Subscribe<TagModel>(this, "TagChanged",
                (sender) =>
                {
                    Assignment.Tag = sender.Name;
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
                });
            await Navigation.PushPopupAsync(new PriorityPopupPage());
        }
        private async void OnCancel()
        {
            await Navigation.PopAsync();
            MessagingCenter.Send(this, "PopupClosed");
        }
    }
}