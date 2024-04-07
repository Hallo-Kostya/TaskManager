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
    public partial class AssignmentPage : ContentPage
    {
        AssignmentViewModel assignmentViewModel;
        public AssignmentPage()
        {
            InitializeComponent();
            BindingContext = assignmentViewModel = new AssignmentViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            assignmentViewModel.OnAppearing();
        }
    }
}