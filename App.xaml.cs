using System.ComponentModel;

namespace MauiApp1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            if (Settings.FirstRun)
            {
                AppDB.LoadStartData();
                Settings.FirstRun = false;
            }
            

            MainPage = new NavigationPage(new MainPage());

        }
    }
}