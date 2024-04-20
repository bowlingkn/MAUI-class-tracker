namespace MauiApp1;

public partial class AddUpdateAssessment : ContentPage
{
	public bool Modify;
	public AssessmentsClass selectedAssessment;
	public Classes CurrentClass;
	public int ClassId;
	public string unModifiedType;
	public bool CheckType = true;

	public AddUpdateAssessment()
	{
		InitializeComponent();
		Modify = false;
	}
	public AddUpdateAssessment(Classes currentClass)
	{
		InitializeComponent();
		Modify = false;
		CurrentClass = currentClass;
	}
	public AddUpdateAssessment(AssessmentsClass assessment)
	{
		InitializeComponent();
		Modify = true;
		selectedAssessment = assessment;
		unModifiedType = assessment.Type;

        AssessName.Text = assessment.Name;
		TypePicker.SelectedItem = assessment.Type;
		StartDate.Date = assessment.Start;
		EndDate.Date = assessment.End;
		Notify.IsToggled = assessment.Notify;
	}

	public async void SaveButton_Clicked(object sender, EventArgs e)
	{
		//--------------------------------------selecting correct class id
		if (Modify) 
		{
			ClassId = selectedAssessment.ClassId;
			if (TypePicker.SelectedItem == unModifiedType)
			{
				CheckType = false;
			}

		}
		else if (!Modify)
		{
			ClassId = CurrentClass.Id;
		}

		if (CheckType)
		{
			var currentAssessments = await AppDB.GetAssessments(ClassId);
			int count = currentAssessments.Count();
			for (int i = 0; i < count; i++)
			{
				if (currentAssessments[i].Type == "Performance Assessment")
				{
					if (TypePicker.SelectedIndex == 0)
					{
						await DisplayAlert("Error", "Performance assessment already exists. You cannot add another.", "Cancel");
						return;
					}
				}
				if (currentAssessments[i].Type == "Objective Assessment")
				{
					if (TypePicker.SelectedIndex == 1)
					{
						await DisplayAlert("Error", "Objective assessment already exists. You cannot add another.", "Cancel");
						return;
					}
				}
			}
		}
		//---------------------------------------input validation
        if (string.IsNullOrWhiteSpace(AssessName.Text))
        {
            await DisplayAlert("Missing assessment name", "Please enter assessment name.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace((string)TypePicker.SelectedItem))
        {
            await DisplayAlert("Missing assessment type", "Please choose an assessment type.", "OK");
            return;
        }
        if (StartDate.Date > EndDate.Date)
        {
            await DisplayAlert("Dates", "Start date must be before end date.", "OK");
            return;
        }

        if (Modify)
		{
			await AppDB.UpdateAssessment(selectedAssessment.Id, AssessName.Text, TypePicker.SelectedItem.ToString(), StartDate.Date, EndDate.Date, Notify.IsToggled);
			await Navigation.PopAsync();
		}
		else
		{
			await AppDB.AddAssessment(CurrentClass.Id, AssessName.Text, TypePicker.SelectedItem.ToString(), StartDate.Date, EndDate.Date, Notify.IsToggled);
			await Navigation.PopAsync();
		}
    }
}