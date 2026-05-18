using Firebase.Auth;
using MauiBirthdayList.Services;


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
			await FirebaseAuthService.LoginAsync(email, password);
			await Shell.Current.GoToAsync("//main");
		}
		catch (FirebaseAuthException ex)
		{
			var message = ex.Reason switch
			{
				AuthErrorReason.WrongPassword => "Incorrect password.",
				AuthErrorReason.UnknownEmailAddress => "No account found with that email.",
				AuthErrorReason.TooManyAttemptsTryLater => "Too many attempts. Please try again later.",
				_ => "Login failed. Please try again."
			};

			await DisplayAlert("Login failed", message, "OK");
		}
	}

	private async void OnRegisterClicked(object sender, EventArgs e)
	{
		await Shell.Current.Navigation.PushAsync(new RegisterPage());
	}
}