namespace MauiApp1;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}

    private void LoadDataClicked(object sender, EventArgs e)
    {
        AppDB.LoadStartData();
    }

    private async void ClearDataClicked(object sender, EventArgs e)
    {
        await AppDB.ClearStartData();
    }
}