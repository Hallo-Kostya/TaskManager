using App1.Models;
using App1.ViewModels;
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
    public partial class AssignmentAddingPage : ContentPage
    {
        
        public AssignmentModel Assignment { get; set; }
        public AssignmentAddingPage()
        {
            InitializeComponent();
            BindingContext = new AssignmentAddingViewModel();
        }
        public AssignmentAddingPage(AssignmentModel assignment)
        {
            InitializeComponent();
            BindingContext = new AssignmentAddingViewModel();
            if (assignment != null)
            {
                ((AssignmentAddingViewModel)BindingContext).Assignment = assignment;
            }

        }

        private void DatePickerDate_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
        private void picker_SelectedIndexChanged(object sender, DateChangedEventArgs e)
        {
            

        }
    }
}
