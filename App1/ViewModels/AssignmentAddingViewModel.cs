using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using App1.Data;
using App1.Models;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using static App1.Models.AssignmentModel;

namespace App1.ViewModels
{
    public class AssignmentAddingViewModel: BaseAssignmentViewModel
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command AddTag { get; }
        public Command LoadTagsCommand { get; }
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
        public ObservableCollection<string> TagList { get; }
        public INavigation Navigation { get; set; }
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
        
        public List<EnumPriority> Priority { get; set; }

        public AssignmentAddingViewModel(INavigation navigation)
        {
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            TagList = new ObservableCollection<string>();
            LoadTagsCommand = new Command(async () => await ExecuteLoadTagsCommand());
            Navigation = navigation;
            Priority = new List<EnumPriority> { EnumPriority.Нет, EnumPriority.Низкий, EnumPriority.Средний, EnumPriority.Высокий };
            this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
            Assignment = new AssignmentModel();
        }
        private async void OnSave()
        {
            Assignment.Priority = SelectedPriority;
            Assignment.Tag = SelectedTag;
            var assignment = Assignment;
            await App.AssignmentsDB.AddItemAsync(assignment);
            await Navigation.PopPopupAsync();
            IsBusy = true;
        }
        public async Task ExecuteLoadTagsCommand()
        {
            try
            {
                TagList.Clear();
                var tags= (await App.AssignmentsDB.GetItemsAsync()).Select(x=> x.Tag).Distinct().ToList();
                foreach (var tag in tags)
                    TagList.Add(tag);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async void OnCancel()
        {
            await Navigation.PopPopupAsync();
            IsBusy = true;
        }
    }
}
