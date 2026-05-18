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
			// TODO: Replace with Firebase auth call, e.g.:
			// await FirebaseAuthService.RegisterAsync(email, password);

			await DisplayAlert("Success", "Account created! Please log in.", "OK");
			await Navigation.PopAsync();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Registration failed", ex.Message, "OK");
		}
	}

	private async void OnBackToLoginClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}