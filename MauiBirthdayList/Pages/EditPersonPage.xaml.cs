using MauiBirthdayList.Services;

namespace MauiBirthdayList;

public partial class EditPersonPage : ContentPage
{
	private readonly Person _person;
	public event Action<Person>? PersonUpdated;

	public EditPersonPage(Person person)
	{
		InitializeComponent();

		_person = person;

		// Pre-fill the fields with existing values
		NameEntry.Text = person.Name;
		AgeEntry.Text = person.Age.ToString();
		DobPicker.Date = new DateTime(person.BirthYear, person.BirthMonth, person.BirthDayOfMonth);
	}

	private void OnDobSelected(object sender, DateChangedEventArgs e)
	{
		var today = DateTime.Today;
		var dob = e.NewDate;

		int age = today.Year - dob.Year;
		if (dob.Month > today.Month || (dob.Month == today.Month && dob.Day > today.Day))
			age--;

		AgeEntry.Text = age.ToString();
	}

	private async void OnSaveClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(NameEntry.Text))
		{
			await DisplayAlert("Validation", "Please enter a name.", "OK");
			return;
		}

		if (!int.TryParse(AgeEntry.Text, out int age) || age <= 0 || age > 150)
		{
			await DisplayAlert("Validation", "Please enter a valid age.", "OK");
			return;
		}

		_person.UserId = FirebaseAuthService.CurrentUserEmail;
		_person.Name = NameEntry.Text.Trim();
		_person.Age = age;
		_person.BirthYear = DobPicker.Date.Year;
		_person.BirthMonth = DobPicker.Date.Month;
		_person.BirthDayOfMonth = DobPicker.Date.Day;

		PersonUpdated?.Invoke(_person);
		await Navigation.PopAsync();
	}
}