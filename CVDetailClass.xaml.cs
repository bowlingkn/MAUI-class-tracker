

namespace MauiApp1;

public partial class CVDetailClass : ContentPage
{
    private readonly int _selectedClassId;
    AssessmentsClass currentAssessment;
    Classes CurrentClass;
    
    public CVDetailClass()
	{
		InitializeComponent();
	}

    public CVDetailClass(int classId, Classes selectedClass)
    {
        InitializeComponent();
        _selectedClassId = classId;
        CurrentClass = selectedClass;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        ClassTitle.Text = CurrentClass.Name;
        ClassCV.ItemsSource = await AppDB.GetSelectedClass(_selectedClassId);
        AssessmentsCV.ItemsSource = await AppDB.GetAssessments(_selectedClassId);      
    }

    private void AssessmentsCV_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null)
        {
            currentAssessment = (AssessmentsClass)e.CurrentSelection.FirstOrDefault();
        }
    }

    private void EditClass_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddUpdateClass(CurrentClass));
    }

    private void ModifyInvoked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AddUpdateAssessment(currentAssessment));
    }

    private async void DeleteInvoked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Delete?", "Are you sure you want to delete this assessment?", "Yes", "No");
        if (answer)
        {
            await AppDB.RemoveAssessment(currentAssessment.Id);
            AssessmentsCV.ItemsSource = await AppDB.GetAssessments(_selectedClassId);
        }
    }

    private async void BtnAddAssessment_Clicked(object sender, EventArgs e)
    {
        var assessmentList = await AppDB.GetAssessments(CurrentClass.Id);
        if (assessmentList.Count() < 2) 
        {        
            await Navigation.PushAsync(new AddUpdateAssessment(CurrentClass));
        }
        else
        {
           await DisplayAlert("Cannot add assessment", "There are already 2 assessments. You cannot add any more.", "Cancel");
        }
    }

    private async void ShareButton_Clicked(object sender, EventArgs e)
    {
        var text = CurrentClass.Notes;
        await Share.RequestAsync(new ShareTextRequest
        { 
            Text = text,
            Title = "Share Notes"
        });
    }
}