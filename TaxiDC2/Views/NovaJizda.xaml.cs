using Syncfusion.Maui.Picker;
using Syncfusion.Maui.Popup;
using System.Collections.ObjectModel;
using TaxiDC2.ViewModels;

namespace TaxiDC2
{
    public partial class NovaJizda : ContentPage
    {
	    private readonly TripNewViewModel _model;
	    public ObservableCollection<int> MinutesList { get; set; }
        public int SelectedMinute { get; set; }
        public NovaJizda(TripNewViewModel model)
        {
	        _model = model;
	        InitializeComponent();
            
            BindingContext = _model = model;
        }

        private async void OnTimePickerButtonClicked(object sender, EventArgs e)
        {
            SfPopup timePickerPopup = null;

            var timePicker = new SfTimePicker
            {
               
                Format = PickerTimeFormat.HH_mm,
                SelectedTime = TimeSpan.Zero,
                WidthRequest = 300,
                HeightRequest = 200
            };

            timePicker.SelectedTime = _model.CasNastupu ?? new TimeSpan(0,DateTime.Now.Hour, DateTime.Now.Minute, 0);
           timePicker.HeaderView.Background = Color.FromArgb("#404040");
            timePicker.BackgroundColor = Color.FromArgb("#404040");
            timePickerPopup = new SfPopup
            {
                HeaderTitle = "Èas jízdy",
                BackgroundColor = Color.FromArgb("#404040"),
                WidthRequest = 350,
                HeightRequest = 400,
                ContentTemplate = new DataTemplate(() =>
                {
                    return new VerticalStackLayout
                    {
                        
                        Spacing = 10,
                        Children =
                {
                    timePicker,
                    new Button
                    {
                        Text = "OK",
                        BackgroundColor = Colors.Orange,
                        TextColor = Colors.White,
                        WidthRequest = 100,
                        Command = new Command(() =>
                        {
                            string selectedTime = timePicker.SelectedTime?.ToString(@"hh\:mm") ?? "00:00";
                            //TimePickerButton.Text = selectedTime;
                            _model.CasNastupu = timePicker.SelectedTime;
                            timePickerPopup.IsOpen = false;
                        })
                    },
                    new Button
                    {
                        Text = "Zrušit",
                        BackgroundColor = Colors.Gray,
                        TextColor = Colors.White,
                        WidthRequest = 100,
                        Command = new Command(() =>
                        {
                            timePickerPopup.IsOpen = false;
                        })
                    }
                }
                    };
                }),
                IsOpen = true
            };

            await Task.CompletedTask;
        }
        
        private async void OnBackButtonPressed(object sender, EventArgs e)
        {
	        Shell.Current.GoToAsync($"///{nameof(SeznamJizd)}");
        }
    }
}
