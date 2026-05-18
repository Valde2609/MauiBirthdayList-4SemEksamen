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
		private void OnSettingsClicked(object? sender, EventArgs e) { }
	}
}
