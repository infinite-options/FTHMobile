using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using InfiniteMeals.Kitchens.Model;
using System.Collections.ObjectModel;
using System.Globalization;
using Xamarin.Forms.Internals;

namespace InfiniteMeals
{
    public partial class MainPage : ContentPage
    {

        ObservableCollection<KitchensModel> Kitchens = new ObservableCollection<KitchensModel>();

        protected async void GetKitchens()
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://dc3so1gav1.execute-api.us-west-1.amazonaws.com/dev/api/v2/foodbanks");
            //request.RequestUri = new Uri("https://phaqvwjbw6.execute-api.us-west-1.amazonaws.com/dev/api/v1/kitchens");

            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var kitchensString = await content.ReadAsStringAsync();
                JObject foodbanks = JObject.Parse(kitchensString);
                this.Kitchens.Clear();


                foreach (var k in foodbanks["result"]["result"])
                {
                    Boolean businessIsOpen;
                    //string accepting_hours;

                    int dayOfWeekIndex = getDayOfWeekIndex(DateTime.Today);
                    string day = getDayOfWeekFromIndex(dayOfWeekIndex);

                    string dayString = DateTime.Today.DayOfWeek.ToString().ToLower();
                    string togetherString = "fb_" + dayString + "_time";

                    this.Kitchens.Add(new KitchensModel()
                    {
                        foodbank_id = k["foodbank_id"].ToString(),
                        foodbank_name = k["fb_name"].ToString(),
                        tag_line = k["fb_tag_line"].ToString(),
                        foodbank_zip = k["fb_zipcode"].ToString(),
                        foodbank_address = k["fb_address1"].ToString(),
                        open_hours = k[togetherString].ToString()

                    });
                }

                kitchensListView.ItemsSource = Kitchens;
            }

        }

        public MainPage()
        {
            InitializeComponent();

            kitchensListView.RefreshCommand = new Command(() =>
            {
                GetKitchens();
                kitchensListView.IsRefreshing = false;
            });

            kitchensListView.ItemSelected += Handle_ItemTapped();

        }

        protected override async void OnAppearing()
        {
            GetKitchens();
        }


        private EventHandler<SelectedItemChangedEventArgs> Handle_ItemTapped()
        {
            return OnItemSelected;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // disable selected item highlighting;
            if (e.SelectedItem == null) return;
            ((ListView)sender).SelectedItem = null;


            // do something with the selection
            var foodbank = e.SelectedItem as KitchensModel;


            parseFoods(foodbank.foodbank_id);


            await Navigation.PushAsync(new SelectMealOptions(foodbank.foodbank_id, foodbank.foodbank_name, foodbank.foodbank_zip));
        }


        private double calcDistance(double pos1, double pos2)
        {
            return Math.Sqrt(pos1*pos1 + pos2*pos2);
        }




        //  Get integer index of day of the week, with 0 as Sunday
        private int getDayOfWeekIndex(DateTime day)
        {
            if (day.DayOfWeek == DayOfWeek.Sunday)
                return 0;
            if (day.DayOfWeek == DayOfWeek.Monday)
                return 1;
            if (day.DayOfWeek == DayOfWeek.Tuesday)
                return 2;
            if (day.DayOfWeek == DayOfWeek.Wednesday)
                return 3;
            if (day.DayOfWeek == DayOfWeek.Thursday)
                return 4;
            if (day.DayOfWeek == DayOfWeek.Friday)
                return 5;
            if (day.DayOfWeek == DayOfWeek.Saturday)
                return 6;
            return -1;
        }

        //  Get day of week string from intger index of the week, with 0 as Sunday
        private string getDayOfWeekFromIndex(int index)
        {
            if (index == 0)
                return "sunday";
            if (index == 1)
                return "monday";
            if (index == 2)
                return "tuesday";
            if (index == 3)
                return "wednesday";
            if (index == 4)
                return "thursday";
            if (index == 5)
                return "friday";
            if (index == 6)
                return "saturday";
            return "Error";
        }

        //  Is the store already closed or is it opening later today?
        private int isAlreadyClosed(TimeSpan end_time)
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            if (now < end_time)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }


        protected async void parseFoods(String foodbankID)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://dc3so1gav1.execute-api.us-west-1.amazonaws.com/dev/api/v2/excess");

            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var kitchensString = await content.ReadAsStringAsync();
                JObject foodbanks = JObject.Parse(kitchensString);
                this.Kitchens.Clear();

                foreach (var k in foodbanks["result"]["result"])
                {
                    if (foodbankID == k["foodbank_id"].ToString())
                    {
                        string foodbank_id = k["fl_name"].ToString();
                    }
                }
                    
            }
        }


        //  Function checking if business is currently open
        private Boolean isBusinessOpen(TimeSpan open_time, TimeSpan close_time, Boolean is_accepting)
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            //  Accepting orders on current day?
            if (is_accepting == false)
            {
                return false;
            }
            else
            {
                //  Opening and closing hours on same day
                if (open_time <= close_time)
                {
                    if (now >= open_time && now <= close_time)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                //  Opening and closing hours on different day
                else
                {
                    if (now >= open_time || now <= close_time)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }


        private string formatZipcode(string zipcode)
        {
            if (zipcode == "95120")
            {
                return "Almaden";
            }
            if (zipcode == "95135")
            {
                return "Evergreen";
            }
            if (zipcode == "95060")
            {
                return "Santa Cruz";
            }
            return "Other";
        }

        // https://www.niceonecode.com/Question/20540/how-to-convert-24-hours-string-format-to-12-hours-format-in-csharp
        public static string ConvertFromToTime(string timeHour, string inputFormat, string outputFormat)
        {
            var timeFromInput = DateTime.ParseExact(timeHour, inputFormat, null, DateTimeStyles.None);
            string timeOutput = timeFromInput.ToString(
                outputFormat,
                CultureInfo.InvariantCulture);
            return timeOutput;
        }
    }
}
