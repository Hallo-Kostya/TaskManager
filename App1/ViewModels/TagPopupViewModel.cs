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
using static App1.Models.TagModel;
using static App1.Models.AssignmentModel;

namespace App1.ViewModels
{
    public class TagPopupViewModel: BaseAssignmentViewModel
    {
        public Command LoadTagsCommand { get; }
        public Command SelectedItemCommand { get; }
        public Command SetTagCommand { get; }
        
        public INavigation Navigation { get; set; }
        private string writenTag { get; set; }
        public string WritenTag
        {
            get { return writenTag; }
            set
            {
                if (writenTag != value)
                {
                    writenTag = value;
                    OnPropertyChanged();
                }
            }
        }
        private TagModel selectedtag { get; set; }
        public TagModel SelectedTag
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

        private ObservableCollection<TagModel> tagList;
        public ObservableCollection<TagModel> TagList 
        {
            get => tagList;
            set => SetProperty(ref tagList, value);
        }
        public TagPopupViewModel(INavigation navigation)
        {
            TagList = new ObservableCollection<TagModel>();
            //LoadTagsCommand = new Command(OnLoaded);
            SelectedItemCommand = new Command(OnSelected);
            SetTagCommand = new Command(SetTag);
            Navigation = navigation;
            Task.Run(async () => await OnLoaded());
           
        }
        
        async Task OnLoaded()
        {
            var tags = (await App.AssignmentsDB.GetTagsAsync()).ToList();
            TagList = new ObservableCollection<TagModel>(tags);
        }
        
        private async void SetTag()
        {
            var Tag = new TagModel();
            Tag.Name = WritenTag;
            //Tag.TagColor = SelectedColor;
            await App.AssignmentsDB.AddTagAsync(Tag);
            await OnLoaded();
        }
        private async void OnSelected()
        {
            //Assignment.Tag = SelectedTag.Name;
            //var assign = Assignment;
            //await App.AssignmentsDB.AddItemAsync(assign);
            await Navigation.PopPopupAsync();
            MessagingCenter.Send<TagModel>(SelectedTag, "TagChanged");
        }
    }
}
