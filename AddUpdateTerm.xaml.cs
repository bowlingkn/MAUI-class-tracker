namespace MauiApp1;

public partial class AddUpdateTerm : ContentPage
{
	public bool Modify;
	public int termId;
	public AddUpdateTerm()
	{
		InitializeComponent();
		Modify = false;
	}

	public AddUpdateTerm(TermClass term)
	{
        InitializeComponent();
		Modify = true;

		termId = term.Id;

		TermName.Text = term.Name;
		StartDate.Date = term.Start;
		EndDate.Date = term.End;
	}

    public async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TermName.Text))
        {
            await DisplayAlert("Missing class name", "Please enter a class name.", "OK");
            return;
        }
        if (StartDate.Date > EndDate.Date)
        {
            await DisplayAlert("Dates", "Start date must be before end date.", "OK");
            return;
        }

        if (Modify)
		{
			await AppDB.UpdateTerm(termId, TermName.Text, StartDate.Date, EndDate.Date);
			await Navigation.PopAsync();
		}
		else
		{
			await AppDB.AddTerm(TermName.Text, StartDate.Date, EndDate.Date);
			await Navigation.PopAsync();
		}
		
    }
}