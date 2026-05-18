using MauiBirthdayList.Services;
namespace MauiBirthdayList

{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
			// Register Pages for Shell navigation
			Routing.RegisterRoute(nameof(AddPersonPage), typeof(AddPersonPage));
			Routing.RegisterRoute(nameof(EditPersonPage), typeof(EditPersonPage));
			Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (FirebaseAuthService.IsLoggedIn)
				await GoToAsync("//main");
		}
		private async void OnLogoutClicked(object sender, EventArgs e)
		{
			bool confirmed = await DisplayAlert("Log out", "Are you sure?", "Yes", "Cancel");
			if (confirmed)
			{
				FirebaseAuthService.Logout();
				await GoToAsync("//login");
			}
		}
	}
}
