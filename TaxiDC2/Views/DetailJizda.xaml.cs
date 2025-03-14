using TaxiDC2.ViewModels;

namespace TaxiDC2
{
    public partial class DetailJizda : ContentPage, IQueryAttributable
	{
		private readonly IBussinessState _bs;
		private TripDetailViewModel _model;

		public DetailJizda(TripDetailViewModel vm, IBussinessState bs)
        {
	        _bs = bs;
	        InitializeComponent();
	        BindingContext = _model = vm;
        }
		
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
	        if (query.ContainsKey("id"))
	        {
		        var idAsString = query["id"]?.ToString();
		        if (Guid.TryParse(idAsString, out var parsedId))
		        {
			        
			        var vm = BindingContext as TripDetailViewModel;
			        vm?.LoadData(parsedId);
		        }
	        }
        }

        private async void OnBackButtonPressed(object sender, EventArgs e)
        {
	        await Shell.Current.GoToAsync($"{nameof(SeznamJizd)}");
			
        }
		
	}
}
