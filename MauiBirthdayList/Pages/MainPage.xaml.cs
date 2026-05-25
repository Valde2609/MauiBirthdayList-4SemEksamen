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
			ApplySort();
		}
		private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
		{
			var query = e.NewTextValue?.Trim().ToLower();
			ApplySort();

			if (string.IsNullOrEmpty(query))
				BirthdayList.ItemsSource = _allPeople;
			else
				BirthdayList.ItemsSource = _allPeople
					.Where(p => p.Name.ToLower().Contains(query))
					.ToList();
		}

		private async void OnSortClicked(object sender, EventArgs e)
		{
			string action = await DisplayActionSheet(
				"Sort by",
				"Cancel",
				null,
				"Name (A-Z)",
				"Name (Z-A)",
				"Birthday (Soonest first)",
				"Birthday (Latest first)"
			);

			if (action == null || action == "Cancel") return;

			SortButton.Text = $"Sort: {action}";

			_allPeople = action switch
			{
				"Name (A-Z)" => _allPeople.OrderBy(p => p.Name).ToList(),
				"Name (Z-A)" => _allPeople.OrderByDescending(p => p.Name).ToList(),
				"Birthday (Soonest first)" => _allPeople.OrderBy(p => p.BirthMonth).ThenBy(p => p.BirthDayOfMonth).ToList(),
				"Birthday (Latest first)" => _allPeople.OrderByDescending(p => p.BirthMonth).ThenByDescending(p => p.BirthDayOfMonth).ToList(),
				_ => _allPeople
			};

			ApplySort();
		}

		private void ApplySort()
		{
			var query = SearchBar.Text?.Trim().ToLower();

			BirthdayList.ItemsSource = string.IsNullOrEmpty(query)
				? _allPeople
				: _allPeople.Where(p => p.Name.ToLower().Contains(query)).ToList();
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
			ApplySort();
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
			ApplySort();
		}

		private async void OnDeleteClicked(object sender, EventArgs e)
		{
			var person = (Person)((Button)sender).CommandParameter;
			bool confirmed = await DisplayAlert("Delete", $"Delete {person.Name}?", "Yes", "Cancel");
			if (confirmed)
			{
				await _service.DeleteAsync(person.Id);
				_allPeople = await _service.GetAllAsync(_userId);
				//BirthdayList.ItemsSource = _allPeople;
				ApplySort();
			}
		}
	}
}
