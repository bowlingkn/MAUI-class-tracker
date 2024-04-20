
namespace MauiApp1;

public partial class ClassesPage : ContentPage
{	private readonly int _selectedTermId;
    TermClass selectedTerm;
	Classes currentClass;
	public ClassesPage()
	{
		InitializeComponent();
	}

	public ClassesPage(TermClass term)
	{
		InitializeComponent();
		_selectedTermId = term.Id;
        selectedTerm = term;

        CurrentTerm.Text = selectedTerm.Name;

		
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		ClassCollectionView.ItemsSource = await AppDB.GetClass(_selectedTermId);
	}

    private void ClassCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
		if (e.CurrentSelection != null)
		{
			currentClass = (Classes)e.CurrentSelection.FirstOrDefault();
		}
    }

    async void DeleteInvoked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Delete?", "Are you sure you want to delete this term?", "Yes", "No");
        if (answer)
        {

            await AppDB.RemoveClass(currentClass.Id);
            ClassCollectionView.ItemsSource = await AppDB.GetClass(_selectedTermId);
        }
    }

    private void AddClass_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new AddUpdateClass(_selectedTermId));
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new CVDetailClass(currentClass.Id, currentClass));
    }
}