using App1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class СreateTagViewModel : BaseAssignmentViewModel
    {
        public Command DeleteNewTag { get; }
        public Command AddTagCommand { get; }
        public Command TagSelectedCommand { get; }
        public Command LoadTagsCommand { get; }
        public Command SetColorCommand { get; }
        public Command OnCancelCommand { get; }
        public Command SaveCommand { get; }
        public Command ConfirmTagCommand { get; }
        private TagModel _tag;
        public TagModel Tag
        {
            get { return _tag; }
            set { _tag = value; OnPropertyChanged(); }
        }
        private TagModel _selectedtag;
        public TagModel SelectedTag
        {
            get { return _selectedtag; }
            set { _selectedtag = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TagModel> tagList;
        public ObservableCollection<TagModel> TagList
        {
            get => tagList;
            set => SetProperty(ref tagList, value);
        }
        private bool IsSelected { get; set; } = false;
        public INavigation Navigation { get; set; }

        public СreateTagViewModel(INavigation navigation)
        {
            DeleteNewTag = new Command(DeleteNew);
            AddTagCommand = new Command(AddTag);
            TagSelectedCommand = new Command<TagModel>(TagSelected);
            SetColorCommand = new Command<string>(SetColor);
            Navigation = navigation;
            Tag = new TagModel();
            TagList=new ObservableCollection<TagModel>();
            OnCancelCommand = new Command(OnCancel);
            ConfirmTagCommand = new Command(ConfirmTag);
            LoadTagsCommand = new Command(async() => await OnLoaded());
            Task.Run(async () => await OnLoaded());
        }
        public async void OnCancel()
        {
            await Navigation.PopAsync();
        }
        public async void DeleteNew()
        {
            if (IsSelected && SelectedTag != null)
            {
                await App.AssignmentsDB.DeleteTagAsync(SelectedTag.ID);
            }
            await OnLoaded();
            Tag = new TagModel();
        }

        public void TagSelected(TagModel tag)
        {
            SelectedTag = tag;
            Tag = tag;
            IsSelected = true;
        }
        public void SetColor(string color)
        {
            Tag.TagColor = color;
        }
        public  void AddTag()
        {
            TagList.Add(new TagModel { Name = Tag.Name, TagColor = Tag.TagColor });
            Tag = new TagModel();
        }
        public async void ConfirmTag()
        {
            if (IsSelected)
            {
                await App.AssignmentsDB.AddTagAsync(Tag);
            }
            else
            {
                await App.AssignmentsDB.AddTagAsync(Tag);
            }
            SelectedTag = null;
            IsSelected = false;
            await OnLoaded();
            Tag = new TagModel();
        }
        async Task OnLoaded()
        {
            var tags = (await App.AssignmentsDB.GetTagsAsync()).ToList();
            TagList = new ObservableCollection<TagModel>(tags);
        }
    }
}
