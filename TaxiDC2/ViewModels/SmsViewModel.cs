using System.ComponentModel;
using System.Windows.Input;

namespace TaxiDC2.ViewModels
{
    public class SmsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IBussinessState _bs;

        public bool IsBusy { get; set; }
        public string PhoneNumber { get; set; }
        public string Title { get; set; }
        public string Message1 { get; set; } = string.Empty;
        public string Message2 { get; set; } = string.Empty;
        public List<string> Buttons { get; set; }

        public ICommand SendCmd { get; }
        public string IdTrip { get; set; }

        public ICommand AddItemCommand { get; set; }
        
        public SmsViewModel(IBussinessState bs)
        {
            _bs = bs;
            Title = "Send SMS";
            SendCmd = new Command<string>(
                execute: async arg =>
                {
                    await Send(arg);
                },
                canExecute: arg => true);

            Message1 = $"Za ?? min pro vás přijede Taxi {(_bs.ActiveUser?.Car != null ? _bs.ActiveUser.Car.FullName : "####")}. Děkujeme Taxi-Děčín 777557776";
            Message2 = $"Vaše Taxi {(_bs.ActiveUser?.Car != null ? _bs.ActiveUser.Car.FullName : "####")} je přistaveno. Děkujeme Taxi-Děčín 777557776";

            Buttons = new List<string>() { "3", "5", "10", "15", "20" };
            AddItemCommand = new Command(OnAddItem);
        }

        private void OnAddItem(object obj)
        {
            Shell.Current.GoToAsync(nameof(NovaJizda));
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        public async Task PosliSms(string messageText, string recipient)
        {
            if (string.IsNullOrEmpty(recipient))
            {
                Debug.WriteLine("Není vyplneno telefoni cislo");
                return;
            }

            try
            {
                SmsMessage message = new(messageText, new[] { recipient });
                await Sms.ComposeAsync(message);

                IApiProxy proxy = DependencyService.Get<IApiProxy>();
                //if (_viewModel.TripState != TripState.SMS1sended)
                //{
                //    var ret = await proxy.ChangeTripState(_viewModel.IdTrip, TripState.SMS1sended);
                //    if (ret.State == ResultCode.OK)
                //        _viewModel.TripState = TripState.SMS1sended;
                //}
                //else
                //{
                //    var ret = await proxy.ChangeTripState(_viewModel.IdTrip, TripState.SMS2sended);
                //    if (ret.State == ResultCode.OK)
                //        _viewModel.TripState = TripState.SMS2sended;
                //}

            }
            catch (FeatureNotSupportedException ex)
            {
                // Sms is not supported on this device.
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        public async Task Send(string minut)
        {
            if (minut == "0")
                await PosliSms(Message2, PhoneNumber);
            else
                await PosliSms(Message1.Replace("??", minut), PhoneNumber);

            await Shell.Current.GoToAsync($"..");
        }

    }
}
