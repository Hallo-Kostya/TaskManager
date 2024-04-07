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
        public Assignment Assignment { get; set; }
        public AssignmentAddingPage()
        {
            InitializeComponent();
            BindingContext = new AssignmentAddingViewModel();
        }
        public AssignmentAddingPage(Assignment assignment)
        {
            InitializeComponent();
            BindingContext = new AssignmentAddingViewModel();
            if (assignment != null)
            {
                ((AssignmentAddingViewModel)BindingContext).Assignment = assignment;
            }
        }
    }
}