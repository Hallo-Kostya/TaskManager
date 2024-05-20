﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App1.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DateSelectionPage : ContentPage
    {
        public DateSelectionPage()
        {
            InitializeComponent();
            BindingContext = new DateSelectionViewModel(Navigation);

            ToolbarItem completeButton = new ToolbarItem
            {
                Text = "Применить", // Текст кнопки
                Command = new Command(OnCustomToolbarItemPressed), // Команда, которая будет выполняться при нажатии // Параметр команды (в данном случае, передаем саму страницу)
            };
            ToolbarItems.Add(completeButton);
        }
        private async void OnCustomToolbarItemPressed()
        {
            await (BindingContext as DateSelectionViewModel).AcceptAndClose();
        }
    }
}