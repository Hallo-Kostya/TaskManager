using App1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App1.ViewModels.Settings
{
    public class ProfileViewModel : BaseAssignmentViewModel
    {
        public Command OnCancelCommand { get; }
        public INavigation Navigation { get; set; }
        private UserModel _user;
        public UserModel User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }
        private int _overDueCount;
        public int OverDueCount
        {
            get => _overDueCount;
            set
            {
                SetProperty(ref _overDueCount, value);

            }
        }
        private int _doneCount;
        public int DoneCount
        {
            get => _doneCount;
            set
            {
                SetProperty(ref _doneCount, value);

            }
        }

        public ProfileViewModel(INavigation navigation)
        {
            OnCancelCommand = new Command(OnCancel);
            Navigation = navigation;
            
            Task.Run(async () => await LoadUser());
            MessagingCenter.Subscribe<object>(this, "UpdateOverdue", (sender) =>
            {
                OverDueCount += 1;
                OnPropertyChanged(nameof(OverDueCount));
            });
            MessagingCenter.Subscribe<object>(this, "UpdateDone", (sender) =>
            {
                DoneCount += 1;
                OnPropertyChanged(nameof(DoneCount));
            });
        }
        
        async Task LoadUser()
        {
            int userId = Preferences.Get("CurrentUserID", -1);
            if (userId!= -1)
            {
                var user = await App.AssignmentsDB.GetUserAsync(userId);
                user.AllOverDue += OverDueCount;
                user.OverDueForWeek += OverDueCount;
                user.DoneForWeek += DoneCount;
                user.DoneAll += DoneCount;
                await App.AssignmentsDB.AddUserAsync(user);
                User= await App.AssignmentsDB.GetUserAsync(userId);
                DoneCount = 0;
                OverDueCount = 0;
                OnPropertyChanged(nameof(DoneCount));
                OnPropertyChanged(nameof(OverDueCount));
            }
        }
        public async void OnCancel()
        {
            await Navigation.PopAsync();
        }
    }
}
