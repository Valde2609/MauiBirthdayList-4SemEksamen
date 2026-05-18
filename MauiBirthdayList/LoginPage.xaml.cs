namespace MauiBirthdayList;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}
	private async void OnLoginClicked(object sender, EventArgs e)
	{
		var email = EmailEntry.Text?.Trim();
		var password = PasswordEntry.Text;

		if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
		{
			await DisplayAlert("Validation", "Please enter your email and password.", "OK");
			return;
		}

		try
		{
			// TODO: Replace with Firebase auth call, e.g.:
			// await FirebaseAuthService.LoginAsync(email, password);

			await GoToMainPage();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Login failed", ex.Message, "OK");
		}
	}

	private async void OnRegisterClicked(object sender, EventArgs e)
	{
		await Shell.Current.Navigation.PushAsync(new RegisterPage());
	}

	private async Task GoToMainPage()
	{
		// Swap out the root page so the user can't navigate back to login
		await Shell.Current.GoToAsync("//main");
	}
}