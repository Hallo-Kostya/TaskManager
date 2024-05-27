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

namespace App1.Views.Popups.EditPopup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditFoldersPopupPage : PopupPage
    {
        public EditFoldersPopupPage()
        {
            InitializeComponent();
            BindingContext = new FoldersPopupViewModel(Navigation);
        }

    }
}