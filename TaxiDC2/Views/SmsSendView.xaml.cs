namespace TaxiDC2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(IdTrip), nameof(IdTrip))]
    [QueryProperty(nameof(Phone), nameof(Phone))]
    public partial class SmsSendView : ContentPage
    {
		public SmsSendView(ViewModels.SmsViewModel model)
		{
			InitializeComponent();
			BindingContext = _model = model;
		}
        
		readonly ViewModels.SmsViewModel _model;

        public string IdTrip
        {
            get => _model.IdTrip;
            set
            {
                _model.IdTrip = _model.IdTrip;
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get => _model.PhoneNumber;
            set
            {
                _model.PhoneNumber = Uri.UnescapeDataString(value ?? string.Empty); 
                OnPropertyChanged();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _model.OnAppearing();
        }

        private async void OnSwipe(object sender, SwipedEventArgs e)
        {
            await Shell.Current.GoToAsync($"..");
        }

        protected override bool OnBackButtonPressed()
        {
            Shell.Current.GoToAsync($"..");
            return true;
        }
    }
}