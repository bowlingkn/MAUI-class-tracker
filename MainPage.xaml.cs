using Android.Views.TextClassifiers;
using Plugin.LocalNotification;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
       // int count = 0;
        TermClass currentTerm;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            TermCV.ItemsSource = await AppDB.GetTerms();

            var classList = await AppDB.GetClasses();
            var assessmentList = await AppDB.GetAssessments();

            if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
            {
                await LocalNotificationCenter.Current.RequestNotificationPermission();
            }

            foreach (Classes classes in classList)
            {
                if (classes.Notify)
                {
                    if (classes.Start.Date == DateTime.Today)
                    {
                        var request = new NotificationRequest
                        {
                            NotificationId = 1,
                            Title = $"{classes.Name} begins today!",
                        };
                        await LocalNotificationCenter.Current.Show(request);
                    }
                    if (classes.End.Date == DateTime.Today) 
                    {
                        var request = new NotificationRequest
                        {
                            Title = $"{classes.Name} ends today!"
                        };
                        await LocalNotificationCenter.Current.Show(request);
                    }
                }
            }

            foreach (AssessmentsClass assessments in assessmentList)
            {
                if (assessments.Notify)
                {
                    if (assessments.Start.Date == DateTime.Today)
                    {
                        var request = new NotificationRequest
                        {
                            NotificationId = 1,
                            Title = $"{assessments.Name} begins today!",
                        };
                        await LocalNotificationCenter.Current.Show(request);
                    }
                    if (assessments.End.Date == DateTime.Today)
                    {
                        var request = new NotificationRequest
                        {
                            Title = $"{assessments.Name} ends today!"
                        };
                        await LocalNotificationCenter.Current.Show(request);
                    }
                }
            }

        }

        private void ModifyInvoked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddUpdateTerm(currentTerm));
        }

        async void DeleteInvoked (object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Delete?", "Are you sure you want to delete this term?", "Yes", "No");
            if (answer)
            {
                await AppDB.RemoveTerm(currentTerm.Id);
                TermCV.ItemsSource = await AppDB.GetTerms();
            }
        }

        private void AddTerm_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddUpdateTerm());
        }

        private void TermCV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                currentTerm = (TermClass)e.CurrentSelection.FirstOrDefault();             
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new ClassesPage(currentTerm));
        }

        private async void Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}