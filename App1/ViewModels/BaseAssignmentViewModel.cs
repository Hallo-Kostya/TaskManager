using App1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.CommunityToolkit.UI.Views;

namespace App1.ViewModels
{
    public class BaseAssignmentViewModel: INotifyPropertyChanged
    {
        
        public string Title { get; set; }
        public LayoutState MainState { get; set; }
        private string selectedtag { get; set; }
        public string SelectedTag
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
        private AssignmentModel _assignment;
        public AssignmentModel Assignment
        {
            get { return _assignment; }
            set { _assignment = value; OnPropertyChanged(); }
        }
        bool isBusy=false;
        public bool IsBusy
        {
            get { return isBusy; }
            set 
            {
                SetProperty(ref isBusy, value);
            }
        }


        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;
            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;
            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
