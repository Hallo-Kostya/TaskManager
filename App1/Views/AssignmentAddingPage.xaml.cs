using App1.Models;
using App1.ViewModels;
using Rg.Plugins.Popup.Pages;
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
    public partial class AssignmentAddingPage : PopupPage
    {
        
        public AssignmentModel Assignment { get; set; }
        public AssignmentAddingPage()
        {
            InitializeComponent();
            BindingContext = new AssignmentAddingViewModel(Navigation);
            this.Focused += (sender, e) => { };
        }
        public AssignmentAddingPage(AssignmentModel assignment)
        {
            InitializeComponent();
            BindingContext = new AssignmentAddingViewModel(Navigation);
            if (assignment != null)
            {
                ((AssignmentAddingViewModel)BindingContext).Assignment = assignment;
            }
            this.Focused += (sender, e) => { };
        }

        private void OnEntryFocused(object sender, FocusEventArgs e)
        {
            DummyBox.Focus();
        }

        private void DatePickerDate_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
        private void picker_SelectedIndexChanged(object sender, DateChangedEventArgs e)
        {
            

        }

        private void EntryName_Focused(object sender, FocusEventArgs e)
        {

        }
    }
}
