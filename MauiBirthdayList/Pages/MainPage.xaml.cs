using System.Collections.ObjectModel;
using System.Xml;

namespace MauiBirthdayList
{
    public partial class MainPage : ContentPage
    {

		public ObservableCollection<Person> People { get; } = new()
	{
		new Person { Name = "John Doe",    Age = 32, DateOfBirth = new DateTime(1992, 3, 15) },
		new Person { Name = "Jane Smith",  Age = 28, DateOfBirth = new DateTime(1996, 7, 22) },
		new Person { Name = "Bob Johnson", Age = 45, DateOfBirth = new DateTime(1979, 11, 3) },
	};

		public MainPage()
		{
			InitializeComponent();
			BindingContext = this;
		}

		private async void OnAddPersonClicked(object sender, EventArgs e)
		{
			var addPage = new AddPersonPage();
			addPage.PersonAdded += OnPersonAdded;
			await Shell.Current.Navigation.PushAsync(addPage);
		}

		private void OnPersonAdded(Person person)
		{
			People.Add(person);
		}

		private async void OnEditClicked(object sender, EventArgs e)
		{
			var person = (Person)((Button)sender).CommandParameter;

			var editPage = new EditPersonPage(person);
			editPage.PersonUpdated += OnPersonUpdated;
			await Shell.Current.Navigation.PushAsync(editPage);
		}

		private void OnPersonUpdated(Person updatedPerson)
		{
			var existing = People.FirstOrDefault(p => p.Id == updatedPerson.Id);
			if (existing is null) return;

			int index = People.IndexOf(existing);
			People[index] = updatedPerson;
		}

		private async void OnDeleteClicked(object sender, EventArgs e)
		{
			var person = (Person)((Button)sender).CommandParameter;
			bool confirmed = await DisplayAlert("Delete", $"Delete {person.Name}?", "Yes", "Cancel");
			if (confirmed)
				People.Remove(person);
		}
	}
}
