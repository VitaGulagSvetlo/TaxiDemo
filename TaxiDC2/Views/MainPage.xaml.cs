
using TaxiDC2.ViewModels;

namespace TaxiDC2
{
	public partial class MainPage : ContentPage
	{
		private readonly MainViewModel _model;

		public MainPage(MainViewModel model)
		{
			InitializeComponent();

			BindingContext = _model = model;
		}

		//public Driver Driver => _bussinessState.ActiveUser;

		private void OnNewClicked(object sender, EventArgs e)
		{
			Shell.Current.GoToAsync($"{nameof(NovaJizda)}");
		}

		private void OnListClicked(object sender, EventArgs e)
		{
			Shell.Current.GoToAsync($"{nameof(SeznamJizd)}");
		}

		private void OnSetClicked(object sender, EventArgs e)
		{
			Shell.Current.GoToAsync($"{nameof(AboutPage)}");
		}


		private async void Button_OnClicked(object sender, EventArgs e)
		{
			if(_model.ActiveUser!=null)
				await Shell.Current.GoToAsync($"{nameof(DetailRidic)}?id={_model.ActiveUser.IdDriver}");
		}

		protected override void OnAppearing()
		{
			_model.NotifyUi();
			base.OnAppearing();
		}
	}

}
