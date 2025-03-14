using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TaxiDC2.ViewModels
{

	public partial class CarDetailViewModel : BaseViewModel
    {
        
        private readonly IDataService _dataService;

        public CarDetailViewModel(IDataService dataService) : base(dataService)
		{
	        _dataService = dataService;
        }

        [ObservableProperty]
        private Car _car  = new();

		[RelayCommand]
		public async Task LoadData(Guid id)
        {
	        Car c = await _dataService.GetCarByIdAsync(id);
	        Car= c ?? new Car();
		}

		[RelayCommand]
		private async Task SaveData()
		{
			bool ret = await _dataService.SaveCarAsync(Car);

			if (ret)
			{
				await Shell.Current.DisplayAlert("Cars", "Auto uloženo", "OK");
				await Shell.Current.GoToAsync($"..");
			}
			else
			{
				//Message = "Chyba ukládání auta";
			}
		}

	}
}