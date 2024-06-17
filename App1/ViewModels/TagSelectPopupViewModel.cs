using App1.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public  class TagSelectPopupViewModel : BaseAssignmentViewModel
    {
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
        public INavigation Navigation { get; set; }
        public Command FilterByTagCommand { get; }
        public TagSelectPopupViewModel(INavigation navigation)
        {
            Navigation = navigation;
            TagList = new ObservableCollection<TagModel>();
            Task.Run(async () => await LoadTags());
            FilterByTagCommand = new Command(OnTagFiltered);
        }

        public async Task LoadTags()
        {
            TagList.Clear();
            TagList.Add(new TagModel() { Name = "Все задачи" });
            var tags = (await App.AssignmentsDB.GetTagsAsync()).Where(x => !string.IsNullOrWhiteSpace(x.Name)).Distinct().ToList();
            foreach (var tag in tags)
                TagList.Add(tag);
        }
        private async void OnTagFiltered()
        {
            await Navigation.PopPopupAsync();
            MessagingCenter.Send(SelectedTag, "TagSelected");
        }
    }
}
