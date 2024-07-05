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
        private int _daystreak = 1;
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
        public int DayStreak
        {
            get => _daystreak;
            set
            {
                if (_daystreak != value)
                {
                    _daystreak = value;
                    OnPropertyChanged(nameof(DayStreak));
                }
            }
        }

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
                    //SetPreferencesForLevel(Level);
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
                    //UpdateLevel();
                }
            }
        }
        
        

      
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}