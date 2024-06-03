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
        private Color selectedColor { get; set; }

        private string writenName { get; set; }
        private string selectedIcon { get; set; }
        private ListModel folder { get; set; }


        public Command SetColorCommand { get; }
        public Command OnCancelCommand { get; }
        public Command AddNewFolderCommand { get; }
        public INavigation Navigation { get; set; }

        public Color SelectedColor
        {
            get { return selectedColor; }
            set
            {
                if (selectedColor != value)
                {
                    selectedColor = value;
                    OnPropertyChanged();
                }
            }
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
            get { return selectedIcon; }
            set
            {
                if (selectedIcon != value)
                {
                    selectedIcon = value;
                    OnPropertyChanged();
                }
            }
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
            SelectedColor = Color.AliceBlue;
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
