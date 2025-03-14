using TaxiDC2.ViewModels;

namespace TaxiDC2
{
    public partial class DetailZakaznik : ContentPage, IQueryAttributable
	{
		private readonly CustomerDetailViewModel _model;

		public DetailZakaznik(CustomerDetailViewModel model)
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
					
					var vm = BindingContext as CustomerDetailViewModel;
					vm?.LoadData(parsedId);
				}
			}
		}

		//private async void OnBackButtonPressed(object sender, EventArgs e)
  //      {
  //          Shell.Current.GoToAsync($"{nameof(SeznamZakazniku)}");
  //      }
    }
}
