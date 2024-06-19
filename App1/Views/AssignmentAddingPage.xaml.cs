using App1.Models;
using App1.Services.Notifications;
using App1.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssignmentAddingPage : PopupPage
    {
        NotificationCenter notificationCenter;
        public AssignmentModel Assignment { get; set; }
        public AssignmentAddingPage()
        {
            InitializeComponent();
            BindingContext = new AssignmentAddingViewModel(Navigation);
            
            notificationCenter = new NotificationCenter();
        }
        public AssignmentAddingPage(ListModel folder)
        {
            InitializeComponent();
            BindingContext = new AssignmentAddingViewModel(Navigation);
            if (folder != null)
            {
                ((AssignmentAddingViewModel)BindingContext).Assignment.FolderName = folder.Name;
            }
            
            notificationCenter = new NotificationCenter();
        }
        public AssignmentAddingPage(AssignmentModel assignment)
        {
            InitializeComponent();
            BindingContext = new AssignmentAddingViewModel(Navigation);
            if (assignment != null)
            {
                ((AssignmentAddingViewModel)BindingContext).Assignment = assignment;
            }
            
            notificationCenter = new NotificationCenter();
        }
        public AssignmentAddingPage(bool _isChildAssignment)
        {
            InitializeComponent();
            BindingContext = new AssignmentAddingViewModel(Navigation);
            ((AssignmentAddingViewModel)BindingContext).IsChildAssignment = _isChildAssignment;
            if (_isChildAssignment == true)
            {
                tags.IsVisible = false;
                folders.IsVisible = false;
                TagsList.IsVisible = false;
            }
           
            notificationCenter = new NotificationCenter();

        }


        private void ButtonSave_Clicked(object sender, EventArgs e)
        {
            var assign = ((AssignmentAddingViewModel)BindingContext).Assignment;
            if (assign.Name!=null && assign.Name.Length>=101)
            {
                TooLongNameAlert.IsVisible = true;
            }
            if (assign.Description!=null && assign.Description.Length >=501)
            {
                TooLongDescAlert.IsVisible = true;
            }
            if (assign.HasNotification)
            {
                notificationCenter.CancelNotification(assign);
                notificationCenter.SendExtendedNotification(assign);
            }
        }
        
    }
}
