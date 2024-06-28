using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Essentials;

namespace App1.Models
{
    public class UserModel : BaseModel
    {
        
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string IconPath { get; set; }
        private int _exp = 0;
        private int _level = 1;
        private int _requiredExp = 100;
        private DateTime _lastLaunchDate = DateTime.MinValue;
        public DateTime LastLaunchDate
        {
            get => _lastLaunchDate;
            set
            {
                if (_lastLaunchDate != value)
                {
                    _lastLaunchDate = value;
                    OnPropertyChanged(nameof(LastLaunchDate));
                }
            }
        }
        public int DayStreak { get; set; }
        public int AllOverDue { get; set; }
        public int OverDueForWeek {  get; set; }
        public int DoneForWeek {  get; set; }
        public int DoneAll { get; set; }

        public int Level
        {
            get => _level;
            set
            {
                if (_level != value)
                {
                    _level = value;
                    OnPropertyChanged(nameof(Level));
                    SetPreferencesForLevel(Level);
                }
            }
        }
 
        public int RequiredExp
        {
            get => _requiredExp;
            set
            {
                if (_requiredExp != value)
                {
                    _requiredExp = value;
                    OnPropertyChanged(nameof(RequiredExp));
                }
            }
        }
        public string ExpDisplay => $"{Exp}/{RequiredExp}";
        public int Exp
        {
            get => _exp;
            set
            {
                if (_exp != value)
                {
                    _exp =value;
                    
                    OnPropertyChanged(nameof(Exp));
                    UpdateLevel();
                }
            }
        }
        private void SetPreferencesForLevel(int level)
        {
            switch (level)
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
        private void UpdateLevel()
        {
            if (Exp >= 100)
            {
                Level = 2;
                RequiredExp = 250;
            }
            else if (Exp >= 250)
            {
                Level = 3;
                RequiredExp = 430;
            }
            else if (Exp >= 430)
            {
                Level = 4;
                RequiredExp = 600;
            }
            else if (Exp >= 600)
            {
                Level = 5;
                RequiredExp = 1000;
            }
            else
            {
                Level = 1;
                RequiredExp = 100;
            }
            
            OnPropertyChanged(nameof(Level));
            OnPropertyChanged(nameof(RequiredExp));
        }
        

      
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}