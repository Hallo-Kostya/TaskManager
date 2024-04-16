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
        public static readonly BindableProperty TagProperty = BindableProperty.Create("Tag",
        typeof(string), typeof(Assignment));
        public AssignmentAddingPage()
        {
            SetBinding(TagProperty, new Binding(@"Tag", BindingMode.OneWayToSource));
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
        public string Tag
        {
            get => (string)GetValue(TagProperty);
            set
            {
                SetValue(TagProperty, value);
            }
        }
    }
}
