using TaxiDC2.ViewModels;

namespace TaxiDC2
{
    public partial class DetailAuto : ContentPage, IQueryAttributable
	{
		private readonly CarDetailViewModel _model;

		public DetailAuto(CarDetailViewModel model)
        {
	        InitializeComponent();
            BindingContext = _model = model; 
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
	        if (query.ContainsKey("id"))
	        {
		        var idAsString = query["id"]?.ToString();
		        if (Guid.TryParse(idAsString, out var parsedId))
		        {
			        Task.Run(async ()=> await _model.LoadData(parsedId)).Wait();
		        }
	        }
        }

        private async void OnBackButtonPressed(object sender, EventArgs e)
        {
	        Shell.Current.GoToAsync($"{nameof(SeznamAut)}");
        }



	}
}
