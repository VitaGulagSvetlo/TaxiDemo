using CommunityToolkit.Mvvm.ComponentModel;

namespace TaxiDC2.ViewModels
{
	public partial class BaseViewModel : ObservableValidator
	{
		protected IDataService DataService { get; }

		[ObservableProperty]
		private bool _isBusy = false;
		[ObservableProperty]
		private string _title = string.Empty;
		[ObservableProperty]
		private string _message = string.Empty;

		/// <inheritdoc/>
		public BaseViewModel(IDataService dataService)
		{
			DataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
			ErrorsChanged += (s, e) =>
			{
				OnPropertyChanged(string.Empty); // notifikuje UI ze se zmenily validacni chyby
			};
		}

		public bool IsMessageVisible => !string.IsNullOrWhiteSpace(Message);

		// Výchozí indexer pro binding chyb validace
		public IEnumerable<string> this[string propertyName]
		{
			get
			{
				return GetErrors(propertyName).Select(s => s.ErrorMessage).ToList();
			}
		}

	}
}
