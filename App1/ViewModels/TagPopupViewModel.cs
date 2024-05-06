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
        public bool IsRefreshing { get; set; }

        private ObservableCollection<string> tagList;
        public ObservableCollection<string> TagList 
        {
            get => tagList;
            set => SetProperty(ref tagList, value);
        }
        public TagPopupViewModel(INavigation navigation)
        {
            TagList = new ObservableCollection<string>();
            //LoadTagsCommand = new Command(OnLoaded);
            SelectedItemCommand = new Command(OnSelected);
            SetTagCommand = new Command(SetTag);
            Navigation = navigation;
            Assignment = new AssignmentModel();
            Task.Run(async () => await OnLoaded());
            
        }
        
        async Task OnLoaded()
        {
            var tags = (await App.AssignmentsDB.GetItemsAsync()).Select(t=>t.Tag).Where(t=>!string.IsNullOrEmpty(t)).Distinct().ToList();
            TagList = new ObservableCollection<string>(tags);
        }
        
        private async void SetTag()
        {
            Assignment.Tag = SelectedTag;
            var assign = Assignment;
            await App.AssignmentsDB.AddItemAsync(assign);
            await OnLoaded();
        }
        private async void OnSelected()
        {
            Assignment.Tag = SelectedTag;
            var assign = Assignment;
            await App.AssignmentsDB.AddItemAsync(assign);
            await Navigation.PopPopupAsync();
        }
    }
}
