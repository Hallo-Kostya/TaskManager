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
	public partial class FolderPage : ContentPage
	{
        FolderPageViewModel folderpaggeviewmodel;
        public FolderPage ()
		{
			InitializeComponent ();
			BindingContext = new FolderPageViewModel(Navigation);
		}
        public FolderPage(ListModel folder)
        {
            InitializeComponent();
            BindingContext = folderpaggeviewmodel=new FolderPageViewModel(Navigation);
			if (folder!=null)
                ((FolderPageViewModel)BindingContext).Folder = folder;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            folderpaggeviewmodel.OnAppearing();
        }
        private bool isExpanded = false;
        private void Vector_Clicked(object sender, EventArgs e)
        {
            // Переключаем изображение в зависимости от текущего состояния
            if (isExpanded)
            {
                VectorButton.Source = "vector_down";
            }
            else
            {
                VectorButton.Source = "vector_up";
            }

            // Переключаем видимость DoneTasks
            DoneTasks.IsVisible = !isExpanded;

            // Обновляем состояние кнопки
            isExpanded = !isExpanded;
        }
        private void TagList_Clicked(object sender, EventArgs e)
        {
            // Переключаем видимость DoneTasks
            TagLists.IsVisible = !isExpanded;

            // Обновляем состояние кнопки
            isExpanded = !isExpanded;
        }
    }
}