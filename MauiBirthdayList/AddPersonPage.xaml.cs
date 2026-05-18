namespace MauiBirthdayList;

public partial class AddPersonPage : ContentPage
{
	public event Action<Person>? PersonAdded;

	public AddPersonPage()
	{
		InitializeComponent();
	}
	private void OnDobSelected(object sender, DateChangedEventArgs e)
	{
		var today = DateTime.Today;
		var dob = e.NewDate;

		int age = today.Year - dob.Year;

		// Step back a year if the birthday hasn't occurred yet this year
		if (dob.Month > today.Month || (dob.Month == today.Month && dob.Day > today.Day))
			age--;

		AgeEntry.Text = age.ToString();
	}
	private async void OnSaveClicked(object sender, EventArgs e)
	{
		// Validate name
		if (string.IsNullOrWhiteSpace(NameEntry.Text))
		{
			await DisplayAlert("Validation", "Please enter a name.", "OK");
			return;
		}

		// Validate age
		if (!int.TryParse(AgeEntry.Text, out int age) || age <= 0 || age > 150)
		{
			await DisplayAlert("Validation", "Please enter a valid age.", "OK");
			return;
		}

		var person = new Person
		{
			Name = NameEntry.Text.Trim(),
			Age = age,
			DateOfBirth = DobPicker.Date
		};

		PersonAdded?.Invoke(person);
		await Navigation.PopAsync();
	}
}
