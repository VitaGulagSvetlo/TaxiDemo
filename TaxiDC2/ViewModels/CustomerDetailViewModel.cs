using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TaxiDC2.ViewModels
{
	public partial class CustomerDetailViewModel :BaseViewModel
    {

        public CustomerDetailViewModel(IDataService dataService):base(dataService)
        {
        }

		[ObservableProperty]
		private Customer _customer = new();

		public async Task LoadData(Guid id)
		{
			Customer customer = await DataService.GetCustomerByIdAsync(id);
				Customer = customer??new Customer();
		}

		[RelayCommand]
        public async Task SaveData()
        {
            bool ret = await DataService.SaveCustomerAsync(Customer);
            if (ret)
            {
	            //await Shell.Current.DisplayAlert("Zákazníci", "Zákazník uložen", "OK");
                // OK
				await Shell.Current.GoToAsync("..");
            }
            else
            {
                //ERR
            }
        }
        
    }
}