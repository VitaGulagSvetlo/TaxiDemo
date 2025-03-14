using TaxiDC2.ViewModels;
namespace TaxiDC2
{
    public partial class SeznamRidicu : ContentPage
    {
        readonly DriverListViewModel _model;

        public SeznamRidicu(DriverListViewModel model)
        {
            InitializeComponent();
            BindingContext = _model = model;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _model.OnAppearing();
        }

		private async void OnBackButtonPressed(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync($"{nameof(MainPage)}");
		}
        
	}
}
