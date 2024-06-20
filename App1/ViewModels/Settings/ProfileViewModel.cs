using App1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App1.ViewModels.Settings
{
    public class ProfileViewModel : BaseAssignmentViewModel
    {
        public Command SetImageCommand { get; }
        public Command SaveCommand { get; }
        public Command OnCancelCommand { get; }
        public INavigation Navigation { get; set; }
        private UserModel _user;
        public UserModel User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }
        private int _overDueCount;
        public int OverDueCount
        {
            get => _overDueCount;
            set
            {
                SetProperty(ref _overDueCount, value);

            }
        }
        private int _doneCount;
        public int DoneCount
        {
            get => _doneCount;
            set
            {
                SetProperty(ref _doneCount, value);

            }
        }

        public ProfileViewModel(INavigation navigation)
        {
            OnCancelCommand = new Command(OnCancel);
            SaveCommand = new Command(Save);
            Navigation = navigation;
            SetImageCommand = new Command(SetImage);
            Task.Run(async () => await LoadUser());
            MessagingCenter.Unsubscribe<object>(this, "UpdateOverdue");
            MessagingCenter.Unsubscribe<object>(this, "UpdateDone");

            // Подписываемся на сообщения об обновлении
            MessagingCenter.Subscribe<object>(this, "UpdateOverdue", (sender) =>
            {
                OverDueCount++;
                Console.WriteLine("ВОФВФОВФОФОВФООВОФВОФФОВОВФ");
                UpdateUserCounts();
            });
            MessagingCenter.Subscribe<object>(this, "UpdateDone", (sender) =>
            {
                DoneCount++;
                Console.WriteLine("ЫВФВФВФФЫВФВФВ");
                UpdateUserCounts();
            });
        }
        private async void Save() {
            await App.AssignmentsDB.AddUserAsync(User);
        }
        private async void SetImage()
        {
            var action = await Application.Current.MainPage.DisplayActionSheet("Выберите источник изображения", "Отмена", null, "Галерея", "Камера");

            if (action == "Галерея")
            {
                await PickPhotoAsync();
            }
            else if (action == "Камера")
            {
                await TakePhotoAsync();
            }
        }

        private async Task PickPhotoAsync()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Выберите фото"
                });

                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    var filePath = await SavePhotoToFileAsync(stream);
                    DeleteOldPhoto(User.IconPath);
                    User.Icon = await SavePhotoToDatabase(stream);
                    OnPropertyChanged(nameof(User.Icon));
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur
                Console.WriteLine($"Photo picking failed: {ex.Message}");
            }
        }

        private async Task TakePhotoAsync()
        {
            try
            {
                var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Сделайте фото"
                });

                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    User.Icon = await SavePhotoToDatabase(stream);
                    OnPropertyChanged(nameof(User.Icon));
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur
                Console.WriteLine($"Photo capture failed: {ex.Message}");
            }
        }
        private async Task<string> SavePhotoToFileAsync(System.IO.Stream photoStream)
        {
            var fileName = $"{Guid.NewGuid()}.jpg";
            var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await photoStream.CopyToAsync(fileStream);
            }

            return filePath;
        }
        private void DeleteOldPhoto(string filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete old photo: {ex.Message}");
                }
            }
        }
        private async Task<string> SavePhotoToDatabase(System.IO.Stream photoStream)
        {
            // Convert stream to byte array
            byte[] photoBytes;
            using (var memoryStream = new System.IO.MemoryStream())
            {
                await photoStream.CopyToAsync(memoryStream);
                photoBytes = memoryStream.ToArray();
            }

            // Convert byte array to Base64 string
            string photoBase64 = Convert.ToBase64String(photoBytes);

            // Save the Base64 string to the database
            User.Icon = photoBase64;
            await App.AssignmentsDB.AddUserAsync(User);

            return photoBase64;
        }
        async Task LoadUser()
        {
            int userId = Preferences.Get("CurrentUserID", -1);
            if (userId != -1)
            {
                User = await App.AssignmentsDB.GetUserAsync(userId);
                DoneCount = 0;
                OverDueCount = 0;
            }
        }
        private async void UpdateUserCounts()
        {
            if (User != null)
            {
                User.AllOverDue += OverDueCount;
                User.OverDueForWeek += OverDueCount;
                User.DoneForWeek += DoneCount;
                User.DoneAll += DoneCount;
                await App.AssignmentsDB.AddUserAsync(User);
                DoneCount = 0;
                OverDueCount = 0;
                OnPropertyChanged(nameof(DoneCount));
                OnPropertyChanged(nameof(OverDueCount));
            }
        }
        public async void OnCancel()
        {
            await Navigation.PopAsync();
        }
    }
}
