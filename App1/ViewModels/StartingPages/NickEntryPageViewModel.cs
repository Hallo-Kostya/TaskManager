﻿using App1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App1.ViewModels.StartingPages
{
    public class NickEntryPageViewModel: BaseAssignmentViewModel
    {
        INavigation Navigation { get; set; }
        public Command SaveUserName { get; }
        private string _nickname;
        public string Nickname
        {
            get => _nickname;
            set
            {
                if (_nickname != value)
                {
                    _nickname = value;
                    OnPropertyChanged(nameof(Nickname));
                    OnPropertyChanged(nameof(IsNicknameNotEmpty));
                }
            }
        }

        public bool IsNicknameNotEmpty => !string.IsNullOrEmpty(Nickname);
        

        private UserModel _user;
        public UserModel User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }
        public NickEntryPageViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            SaveUserName = new Command(SaveUser);
        }
        private async void SaveUser()
        {
            if (!string.IsNullOrWhiteSpace(Nickname))
            {
                User.Name = Nickname;
                await App.AssignmentsDB.AddUserAsync(User);
                Preferences.Set("IsFirstLaunch", false);
                Preferences.Set("CurrentUserNickname", Nickname);
                Application.Current.MainPage = new AppShell();
            }
        }

    }
}