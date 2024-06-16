using App1.Models;
using App1.Services.Notifications;
using App1.ViewModels;
using App1.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPage : ContentPage
    {
        NotificationCenter notificationCenter;
        private EditViewModel viewModel;
        public EditPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new EditViewModel(Navigation);
            notificationCenter = new NotificationCenter();


        }
        public EditPage(AssignmentModel assignment,bool _isFromPopup)
        {
            InitializeComponent();
            BindingContext = viewModel = new EditViewModel(Navigation);
            if (assignment != null )
            {
                ((EditViewModel)BindingContext).Assignment=assignment;
                ((EditViewModel)BindingContext).isFromPopup = _isFromPopup;
            }
            notificationCenter = new NotificationCenter();
            if (assignment.IsChild == true)
            {
                tags.IsVisible = false;
                layout_list.IsVisible = false;
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }

        private void ButtonSave_Clicked(object sender, EventArgs e)
            {
            var assign = ((EditViewModel)BindingContext).Assignment;
            if (assign.Name!=null && assign.Name.Length >= 101)
            {
                LongNameAlert.IsVisible = true;
            }
            if (assign.Description!=null && assign.Description.Length >= 501)
            {
                LongDescAlert.IsVisible = true;
            }
            if (assign.HasNotification)
            {
                notificationCenter.SendExtendedNotification(assign);
            }
            LongDescAlert.IsVisible = false;
            LongNameAlert.IsVisible = false;
        }

    }
}