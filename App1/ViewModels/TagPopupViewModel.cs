using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using App1.Data;
using App1.Models;
using App1.Views;
using App1.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;
using static App1.Models.AssignmentModel;

namespace App1.ViewModels
{
    public class TagPopupViewModel: BaseAssignmentViewModel
    {
        public Command LoadTagsCommand { get; }
        public Command SelectedItemCommand { get; }
        public Command SetTagCommand { get; }
        public INavigation Navigation { get; set; }
        public bool IsRefreshing { get; set; }
        private ObservableCollection<string> tagList { get; set; }
        public ObservableCollection<string> TagList 
        {
            get { return tagList; }
            set
            {
                tagList = value;
                OnPropertyChanged();
            }
        }
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
        public TagPopupViewModel(INavigation navigation)
        {
            tagList = new ObservableCollection<string>();
            LoadTagsCommand = new Command(OnLoaded);
            SelectedItemCommand = new Command(OnSelected);
            SetTagCommand = new Command(SetTag);
            Navigation = navigation;
            Assignment = new AssignmentModel();
        }
        
        private async void OnLoaded()
        {
            tagList.Clear();
            var tags = (await App.AssignmentsDB.GetItemsAsync()).Select(t=>t.Tag).Where(t=>!string.IsNullOrEmpty(t)).ToList();
            foreach(var tag in tags)
            {
                tagList.Add(tag);
            }
        }

        private async void SetTag()
        {
            Assignment.Tag = SelectedTag;
            await Navigation.PopPopupAsync();
        }
        private async void OnSelected()
        {
            Assignment.Tag = SelectedTag;
            await Navigation.PopPopupAsync();
        }
    }
}
