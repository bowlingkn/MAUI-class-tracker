namespace MauiApp1;

public partial class AddUpdateClass : ContentPage
{
	public bool Modify;
	public Classes selectedClass;
	public int selectedTerm;
	public AddUpdateClass()
	{
		InitializeComponent();
		Modify = false;
	}

	public AddUpdateClass(int currentTermId)
	{
		InitializeComponent();
		selectedTerm = currentTermId;
		Modify = false;
		Notify.IsToggled = false;
	}
	public AddUpdateClass(Classes currentClass)
	{
		InitializeComponent();
		Modify = true;
		selectedClass = currentClass;

		ClassName.Text = selectedClass.Name;
		StatusPicker.SelectedItem = selectedClass.Status;
		StartDate.Date = selectedClass.Start;
		EndDate.Date = selectedClass.End;
		TeacherName.Text = selectedClass.TeacherName;
		TeacherEmail.Text = selectedClass.TeacherEmail;
		TeacherPhone.Text = selectedClass.TeacherPhone;
		NotesEditor.Text = selectedClass.Notes;
		Notify.IsToggled = selectedClass.Notify;

	}
    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
		if (string.IsNullOrWhiteSpace(ClassName.Text))
		{
			await DisplayAlert("Missing class name", "Please enter a class name.", "OK");
			return;
		}
        if (string.IsNullOrWhiteSpace((string)StatusPicker.SelectedItem)) 
		{
            await DisplayAlert("Missing status", "Please select a status.", "OK");
            return;
        }
		if (string.IsNullOrWhiteSpace(TeacherName.Text))
		{
            await DisplayAlert("Missing instructor name", "Please enter instructor name.", "OK");
            return;
        }
		if (string.IsNullOrWhiteSpace(TeacherEmail.Text))
		{
            await DisplayAlert("Missing instructor email", "Please enter an instructor email address.", "OK");
            return;
        }
		if (string.IsNullOrWhiteSpace(TeacherPhone.Text))
		{
            await DisplayAlert("Missing instructor phone", "Please enter an instructor phone number.", "OK");
            return;
        }
		if (StartDate.Date > EndDate.Date)
		{
			await DisplayAlert("Dates", "Start date must be before end date.", "OK");
			return;
		}

		if (Modify)
		{
			await AppDB.UpdateClass(selectedClass.Id, ClassName.Text, StartDate.Date, EndDate.Date, StatusPicker.SelectedItem.ToString(), TeacherName.Text, TeacherPhone.Text, TeacherEmail.Text, NotesEditor.Text, Notify.IsToggled);
			await Navigation.PopAsync();
		}
		else
		{
			await AppDB.AddClass(selectedTerm, ClassName.Text, StartDate.Date, EndDate.Date, StatusPicker.SelectedItem.ToString(), TeacherName.Text, TeacherPhone.Text, TeacherEmail.Text, NotesEditor.Text, Notify.IsToggled);
			await Navigation.PopAsync();

		}
    }

}