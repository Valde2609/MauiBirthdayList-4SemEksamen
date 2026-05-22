using MauiBirthdayList.Services;
using System.Collections.ObjectModel;
using System.Xml;

namespace MauiBirthdayList
{
    public partial class MainPage : ContentPage
	{
		private readonly BirthdayService _service = new();
		private readonly string _userId = FirebaseAuthService.CurrentUserEmail;
		private List<Person> _allPeople = new();

		public MainPage()
		{
			InitializeComponent();
		}
		protected override async void OnAppearing()
		{
			base.OnAppearing();
			_allPeople = await _service.GetAllAsync(_userId);
			BirthdayList.ItemsSource = _allPeople;
		}
		private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
		{
			var query = e.NewTextValue?.Trim().ToLower();

			if (string.IsNullOrEmpty(query))
				BirthdayList.ItemsSource = _allPeople;
			else
				BirthdayList.ItemsSource = _allPeople
					.Where(p => p.Name.ToLower().Contains(query))
					.ToList();
		}

		private async void OnAddPersonClicked(object sender, EventArgs e)
		{
			var addPage = new AddPersonPage();
			addPage.PersonAdded += OnPersonAdded;
			await Shell.Current.Navigation.PushAsync(addPage);
		}

		private async void OnPersonAdded(Person person)
		{
			await _service.CreateAsync(person);
			_allPeople = await _service.GetAllAsync(_userId);
			BirthdayList.ItemsSource = _allPeople;
		}

		private async void OnEditClicked(object sender, EventArgs e)
		{
			var person = (Person)((Button)sender).CommandParameter;

			var editPage = new EditPersonPage(person);
			editPage.PersonUpdated += OnPersonUpdated;
			await Shell.Current.Navigation.PushAsync(editPage);
		}

		private async void OnPersonUpdated(Person updatedPerson)
		{
			await _service.UpdateAsync(updatedPerson);
			_allPeople = await _service.GetAllAsync(_userId);
			BirthdayList.ItemsSource = _allPeople;
		}

		private async void OnDeleteClicked(object sender, EventArgs e)
		{
			var person = (Person)((Button)sender).CommandParameter;
			bool confirmed = await DisplayAlert("Delete", $"Delete {person.Name}?", "Yes", "Cancel");
			if (confirmed)
			{
				await _service.DeleteAsync(person.Id);
				_allPeople = await _service.GetAllAsync(_userId);
				BirthdayList.ItemsSource = _allPeople;
			}
		}
	}
}
