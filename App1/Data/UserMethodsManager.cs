using App1.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace App1.Data
{
    public  class UserMethodsManager
    {
        public UserMethodsManager() { }
        public void SetPreferencesForLevel(UserModel user)
        {   
            switch (user.Level)
            {
                case 1:
                    Preferences.Set("SomeSetting", "ValueForLevel0");
                    break;
                case 2:
                    Preferences.Set("SomeSetting", "ValueForLevel1");
                    break;
                case 3:
                    Preferences.Set("SomeSetting", "ValueForLevel2");
                    break;
                case 4:
                    Preferences.Set("SomeSetting", "ValueForLevel3");
                    break;
                case 5:
                    Preferences.Set("SomeSetting", "ValueForLevel4");
                    break;
                default:
                    break;
            }
        }
        public async Task UpdateLevel(UserModel user)
        {
            if (user.Exp >= 100)
            {
                user.Level = 2;
                user.RequiredExp = 250;
            }
            else if (user.Exp >= 250)
            {
                user.Level = 3;
                user.RequiredExp = 430;
            }
            else if (user.Exp >= 430)
            {
                user.Level = 4;
                user.RequiredExp = 600;
            }
            else if (user.Exp >= 600)
            {
                user.Level = 5;
                user.RequiredExp = 1000;
            }
            else
            {
                user.Level = 1;
                user.RequiredExp = 100;
            }
            await App.AssignmentsDB.AddUserAsync(user);
        }
    }
}
