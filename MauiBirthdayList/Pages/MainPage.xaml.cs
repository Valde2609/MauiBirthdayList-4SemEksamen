using MauiBirthdayList.Services;
using System.Collections.ObjectModel;
using System.Xml;

namespace MauiBirthdayList
{
    public partial class MainPage : ContentPage
	{
		private readonly BirthdayService _service = new();
		private readonly string _userId = FirebaseAuthService.CurrentUserEmail;

		public MainPage()
		{
			InitializeComponent();
		}
		protected override async void OnAppearing()
		{
			base.OnAppearing();
			BirthdayList.ItemsSource = await _service.GetAllAsync(_userId);
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
			BirthdayList.ItemsSource = await _service.GetAllAsync(_userId);
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
			BirthdayList.ItemsSource = await _service.GetAllAsync(_userId);
		}

		private async void OnDeleteClicked(object sender, EventArgs e)
		{
			var person = (Person)((Button)sender).CommandParameter;
			bool confirmed = await DisplayAlert("Delete", $"Delete {person.Name}?", "Yes", "Cancel");
			if (confirmed)
			{
				await _service.DeleteAsync(person.Id);
				BirthdayList.ItemsSource = await _service.GetAllAsync(_userId);
			}
		}
	}
}
