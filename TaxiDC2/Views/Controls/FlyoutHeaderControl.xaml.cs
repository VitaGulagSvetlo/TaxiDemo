using Firebase.Auth;

namespace TaxiDC2.Components;

public partial class FlyoutHeaderControl :StackLayout
{
	public FlyoutHeaderControl(FirebaseAuthClient authClient)
	{
		InitializeComponent();
		if (!string.IsNullOrWhiteSpace(authClient?.User?.Info?.Email))
		{
			lblUserEmail.Text = authClient?.User?.Info?.Email;
			lblUser.IsVisible = !string.IsNullOrWhiteSpace(authClient?.User?.Info?.DisplayName);
			lblUser.Text = authClient?.User?.Info?.DisplayName;
		}
	}
}