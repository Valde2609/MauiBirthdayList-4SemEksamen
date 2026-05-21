using MauiBirthdayList.Services;

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

		// Create a new Person object from api
		var person = new Person
		{
			UserId = FirebaseAuthService.CurrentUserEmail,
			Name = NameEntry.Text.Trim(),
			Age = age,
			BirthYear = DobPicker.Date.Year,
			BirthMonth = DobPicker.Date.Month,
			BirthDayOfMonth = DobPicker.Date.Day
		};

		PersonAdded?.Invoke(person);
		await Navigation.PopAsync();
	}
}
