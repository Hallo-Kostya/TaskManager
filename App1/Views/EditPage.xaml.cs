using App1.Models;
using App1.ViewModels;
using App1.Views.Popups;
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
        public EditPage()
        {
            InitializeComponent();
            BindingContext = new EditViewModel(Navigation);
        }
        public EditPage(AssignmentModel assignment)
        {
            InitializeComponent();
            BindingContext = new EditViewModel(Navigation);
            if (assignment != null )
            {
                ((EditViewModel)BindingContext).Assignment=assignment;
            }
        }

        private void DatePickerDate_DateSelected(object sender, DateChangedEventArgs e)
        {

        }
    }
}