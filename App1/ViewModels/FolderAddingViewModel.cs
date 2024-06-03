using App1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace App1.ViewModels
{
    public class FolderAddingViewModel: BaseAssignmentViewModel
    {

        private string writenName { get; set; }
        private ListModel folder { get; set; }


        public Command SetColorCommand { get; }
        public Command OnCancelCommand { get; }
        public Command AddNewFolderCommand { get; }
        public INavigation Navigation { get; set; }

        private Color _selectedColor;
        private string _selectedIcon;

        public Color SelectedColor
        {
            get => _selectedColor;
            set => SetProperty(ref _selectedColor, value);
        }


        public string WritenName
        {
            get { return writenName; }
            set
            {
                if (writenName != value)
                {
                    writenName = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SelectedIcon
        {
            get => _selectedIcon;
            set => SetProperty(ref _selectedIcon, value);
        }

        public ListModel Folder
        {
            get { return folder; }
            set
            {
                if (folder != value)
                {
                    folder = value;
                    OnPropertyChanged();
                }
            }
        }
        public FolderAddingViewModel(INavigation navigation) 
        {
            Navigation = navigation;
            AddNewFolderCommand = new Command(OnAdded);
            OnCancelCommand = new Command(OnCancel);
            SetColorCommand = new Command<string>(SetColor);
            Folder = new ListModel();
            SelectedIcon = "folders";
            SelectedColor = Color.Default;
        }
       
        private async void OnCancel()
        {
            await Navigation.PopAsync();
        }

        private void SetColor(string  color)
        {
            SelectedColor = Color.FromHex(color);
        }
        private async void OnAdded()
        {
            if (!string.IsNullOrWhiteSpace(WritenName))
            {
                //Folder.Color = SelectedColor;
                Folder.Icon = SelectedIcon;
                Folder.Name = WritenName;
                Folder.Count = 0;
                await App.AssignmentsDB.AddListAsync(Folder);
                await Navigation.PopAsync();
                MessagingCenter.Send(this, "FolderClosed");
            }
        }
    }
}
