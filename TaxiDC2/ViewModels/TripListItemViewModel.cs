using CommunityToolkit.Mvvm.ComponentModel;

namespace TaxiDC2.ViewModels
{
	[ObservableObject]
	public partial class TripListItemViewModel
	{

		[ObservableProperty] private Trip _data = new();
		[ObservableProperty] public bool _minLabelVisible;

		public bool IsMemoNotEmpty => !string.IsNullOrWhiteSpace(Data.Memo);
		public bool CustomerMemoVisible => !string.IsNullOrWhiteSpace(Data.Customer?.Memo);

		//[DependsOn("DeadLine,Counter1")]
		public int MinToDeadLine
		{
			get
			{
				if (Data.BoardingTime != null) return (int)(Data.BoardingTime.Value - DateTime.Now).TotalMinutes;
				else
				{
					return
						Data.DeadLine.HasValue
						 ? ((Data.OrderTime + Data.DeadLine.Value - DateTime.Now).TotalMinutes > 0
							 ? (int)(Data.OrderTime + Data.DeadLine.Value - DateTime.Now).TotalMinutes
							 : 0)
						 : 0;

				}
			}
		}

		//[DependsOn("MinToDeadLine")]
		public string MinToDeadLineTxt
		{
			get
			{
				if (MinToDeadLine > 60 * 24)
				{
					MinLabelVisible = false;
					return $"{Data.BoardingTime:dd.MM. HH:mm}";
				}
				MinLabelVisible = true;
				return MinToDeadLine < 0 ? "0" : MinToDeadLine.ToString();
			}
		}
		
		//[DependsOn("TripState")]
		public Color StateColor
		{
			get
			{
				Color c = (Color)Application.Current.Resources[$"STC_{Data.TripState.ToString()}"];     //nova neprevzata
				return c;
			}
		}

		//[DependsOn("TripState")]
		public Color StateTextColor
		{
			get
			{
				Color c = ContrastColor(StateColor);

				return c;
			}
		}

		Color ContrastColor(Color color)
		{
			int d = 0;

			// Counting the perceptive luminance - human eye favors green color...      
			double luminance = (0.299 * color.Red + 0.587 * color.Green + 0.114 * color.Blue) / 255;

			d = luminance > 0.5 ? 0 : 255;

			return Color.FromRgb(d, d, d);
		}

		//[DependsOn("TripState")]
		public FileImageSource StateImage
		{
			get
			{
				FileImageSource i = new();

				switch (Data.TripState)
				{
					case TripState.NewOrder:
						i = (FileImageSource)ImageSource.FromFile("ico_star_black.png");        // hvezdicka
						break;

					case TripState.RejectedByDiver:
						i = (FileImageSource)ImageSource.FromFile("ico_star_black.png");        // hvezdicka
						break;

					case TripState.ForwardToDiver:
						i = (FileImageSource)ImageSource.FromFile("ico_user_black.png");        // user
						break;

					case TripState.AcceptedDiver:
						i = (FileImageSource)ImageSource.FromFile("ico_user_black.png");        // user
						break;

					case TripState.Running:
						i = (FileImageSource)ImageSource.FromFile("ico_car_black.png");         // auticko
						break;

					case TripState.Sms1Sended:
						i = (FileImageSource)ImageSource.FromFile("ico_sms1_black.png");         // smska
						break;

					case TripState.Sms2Sended:
						i = (FileImageSource)ImageSource.FromFile("ico_sms2_black.png");         // smska
						break;

					case TripState.Call:
						i = (FileImageSource)ImageSource.FromFile("ico_phone_black.png");       // sluchatko
						break;

					case TripState.Comleted:
						i = (FileImageSource)ImageSource.FromFile("ico_thumbsup_black.png");    // palec nahoru
						break;

					case TripState.Canceled:
						i = (FileImageSource)ImageSource.FromFile("ico_cross_black.png");       // krizek
						break;

					case TripState.NewFromWww:
						i = (FileImageSource)ImageSource.FromFile("ico_globe_black.png");       // globus
						break;

					default:
						break;
				}

				return i;
			}
		}

		//[DependsOn("DeadLine,Counter1")]
		public Color TimeColor =>
			Data.TripState == TripState.Canceled || Data.TripState == TripState.Comleted
				? (Color)Application.Current.Resources["PozadiTmava"]
				: MinToDeadLine < 2
					? (Color)Application.Current.Resources["Cervena"]
					: MinToDeadLine < 5
						? (Color)Application.Current.Resources["Oranzova"]
						: MinToDeadLine < 60
							? (Color)Application.Current.Resources["Zelena"]
							: (Color)Application.Current.Resources["Zluta2"];
		

		//[DependsOn("TripState")]
		public bool TimeVisible => Data.TripState != TripState.Canceled && Data.TripState != TripState.Comleted;
		
		public void RefreshTime()
		{
			OnPropertyChanged(nameof(MinToDeadLine));
			OnPropertyChanged(nameof(MinToDeadLineTxt));
			OnPropertyChanged(nameof(TimeColor));
		}

		public static TripListItemViewModel FromTrip(Trip trip)
		{
			return new TripListItemViewModel() { Data = trip };
		}
	}
}