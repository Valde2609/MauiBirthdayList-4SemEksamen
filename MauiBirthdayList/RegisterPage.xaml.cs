using Firebase.Auth;
using MauiBirthdayList.Services;

namespace MauiBirthdayList;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}

	private async void OnRegisterClicked(object sender, EventArgs e)
	{
		var email = EmailEntry.Text?.Trim();
		var password = PasswordEntry.Text;
		var confirm = ConfirmPasswordEntry.Text;

		if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
		{
			await DisplayAlert("Validation", "Please fill in all fields.", "OK");
			return;
		}

		if (password != confirm)
		{
			await DisplayAlert("Validation", "Passwords do not match.", "OK");
			return;
		}

		if (password.Length < 6)
		{
			await DisplayAlert("Validation", "Password must be at least 6 characters.", "OK");
			return;
		}

		try
		{
			await FirebaseAuthService.RegisterAsync(email, password);
			await DisplayAlert("Success", "Account created! You are now logged in.", "OK");
			await Shell.Current.GoToAsync("//main");
		}
		catch (FirebaseAuthException ex)
		{
			var message = ex.Reason switch
			{
				AuthErrorReason.EmailExists => "An account with this email already exists.",
				AuthErrorReason.WeakPassword => "Password is too weak.",
				_ => "Registration failed. Please try again."
			};

			await DisplayAlert("Registration failed", message, "OK");
		}
	}

	private async void OnBackToLoginClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}