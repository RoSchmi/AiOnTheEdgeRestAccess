using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text;
using System.Text.Json;
using System.Windows.Threading;
using System.Net.Http;
using System.Windows;
using System.Collections.ObjectModel;
using System.Globalization;



namespace AiOnTheEdgeRestAccess
{
    internal sealed partial class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()   
        {
            Get_Value_Clicked_Command = new AsyncRelayCommand(GetValue);
            valuesList = new ObservableCollection<string?>();    
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();
            ReadInterval = "10";
            HostName = "gasmeter";
            ConsumptionBaseOffset = "0";
        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            int newReadInterval = int.Parse(ReadInterval);
            if (int.TryParse(ReadInterval, out newReadInterval))
            {
                newReadInterval = newReadInterval < 5 ? 5 : newReadInterval;
                newReadInterval = newReadInterval > 300  ? 300 : newReadInterval;
                ReadInterval = newReadInterval.ToString();
            }
            else
            {
                ReadInterval = "10";
            }
            timer.Interval = TimeSpan.FromSeconds(int.Parse(ReadInterval));
  
            string? result = await GetValue();
        }

        public IAsyncRelayCommand Get_Value_Clicked_Command { get; }


        private async Task<string?> GetValue()
        {
            const string jsonPath = "json";
            MeterJson? meterJson = null;
            System.Net.Http.HttpResponseMessage? response;
            string? content = null;

            using (var client = new HttpClient())
            {
                RequestUrl = $"http://{HostName}/{jsonPath}";
                
                try
                {
                    response = await client.GetAsync(RequestUrl);
                    content = await response.Content.ReadAsStringAsync();
                }
                catch
                {
                    content = string.Empty;
                }
            }

            meterJson = content == string.Empty? null : JsonSerializer.Deserialize<MeterJson>(content);
            
            if ((meterJson != null) && (meterJson.main != null) && (meterJson.main.raw != null))
            {

                string? rawString = meterJson.main.raw;
                float rawFloat = 0;

                ErrorMessage = meterJson.main.error;

                if (float.TryParse(rawString, formatProvider, out rawFloat)) 
                {
                    if(int.TryParse(consumptionBaseOffset, out consumptionBaseValue)) { };
                   
                    rawFloat = rawFloat + consumptionBaseValue;
                    
                    ValuesList.Add(FormattableString.Invariant($"{rawFloat.ToString("f2", formatProvider)}    PictureTime: {meterJson.main.timestamp.Substring(11,8)}    ReadTime: {DateTime.Now.TimeOfDay.ToString().Substring(0,8)}"));
             
                }
                else
                {
                    ValuesList.Add("??");
                }   
            }
            return content;
        }

        [ObservableProperty]
        private string? hostName;

        [ObservableProperty]
        private string ? readInterval = "10";

        [ObservableProperty]
        private string? errorMessage;

        [ObservableProperty]
        private string? requestUrl;

        [ObservableProperty]
        private string? consumptionBaseOffset;

        [ObservableProperty]
        private ObservableCollection<string?>? valuesList;

        private IFormatProvider? formatProvider = new CultureInfo("en-US");

        private int consumptionBaseValue = 0;
        
        private DispatcherTimer timer;
    }
}
