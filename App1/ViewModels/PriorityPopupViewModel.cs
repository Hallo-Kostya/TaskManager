using App1.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static App1.Models.AssignmentModel;

namespace App1.ViewModels
{
    public class PriorityPopupViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }
        public List<EnumPriority> Priority { get; set; }
        public AssignmentModel Assignmentic { get; }
        private EnumPriority selectedPriority { get; set; }
        public EnumPriority SelectedPriority
        {
            get { return selectedPriority; }
            set
            {
                if (selectedPriority != value)
                {
                    selectedPriority = value;
                    OnPropertyChanged();
                }
            }
        }
        public Command PrioritySelectedCommand { get; }


        public PriorityPopupViewModel(INavigation navigation)
        {
            Priority = new List<EnumPriority> { EnumPriority.Нет, EnumPriority.Низкий, EnumPriority.Средний, EnumPriority.Высокий };
            Navigation = navigation;
            PrioritySelectedCommand = new Command(OnSelected);
            Assignmentic = new AssignmentModel();
        }
        private async void OnSelected()
        {
            Assignmentic.Priority = SelectedPriority;
            await Navigation.PopPopupAsync();
            MessagingCenter.Send(Assignmentic, "ProrityChanged");
        }
    }
}
