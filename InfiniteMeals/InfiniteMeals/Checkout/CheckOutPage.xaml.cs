using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InfiniteMeals.Meals.Model;
using Newtonsoft.Json;
using Xamarin.Forms;



namespace InfiniteMeals
{
    public partial class OrderInfo
    {
        public string customer_id = "";
        public string phone = ""; 
        public string street = "";
        public string city = "";
        public string state = "";
        public string kitchen_id = "";
        public string zipcode = "";
        public string totalAmount = "";
        public string delivery_note = "";
        public string latitude = "";
        public string longitude = "";
        public string delivery_date = "";
        public List<Dictionary<string, string>> ordered_items = new List<Dictionary<string, string>>() ;
    }

    public partial class CheckOutPage : ContentPage
    {
        OrderInfo currentOrder = new OrderInfo();

        ObservableCollection<MealsModel> mealsOrdered = new ObservableCollection<MealsModel>();

        double totalCostsForMeals = 0;

        double calculatedTaxAmount = 0;

        string kitchenZipcode;

        
        public Label cartReviewLabel = new Label() { VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 23, HeightRequest = 45, MinimumWidthRequest = 170, Text = "Cart Review", Margin = 5};
        public Label nameLabel = new Label() { VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 20, HeightRequest = 45, MinimumWidthRequest = 170, Text = "Second Harvest Food Bank", Margin = 8 };


        public Image cartImage = new Image() { VerticalOptions = LayoutOptions.Center, Source = "truck.png", HeightRequest = 60, MinimumWidthRequest = 170, Margin = new Thickness(180, 120, 0, 0)};
        public Label confirmLabel = new Label() { VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 20, HeightRequest = 45, MinimumWidthRequest = 170, Text = "Confirm Order", Margin = 0 };
        public Label addressLabel = new Label() { VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 15, HeightRequest = 25, MinimumWidthRequest = 170, Text = "123 Santa Clara, CA 95120", Margin = 5 };





        public CheckOutPage(ObservableCollection<MealsModel> meals, string foodbank_id, string kitchen_zipcode)
        {
            InitializeComponent();

            kitchenZipcode = kitchen_zipcode;

            mealsOrdered = meals;
            SetupUI();



            currentOrder.kitchen_id = foodbank_id;
            currentOrder.customer_id = "12345";
            currentOrder.phone = "408 466 7899";
            currentOrder.street = "Mission St";
            currentOrder.city = "Santa Cruz";
            currentOrder.state = "CA";
            currentOrder.zipcode = "95060";
            currentOrder.delivery_note = "Test delivery note";
            currentOrder.latitude = "0.000";
            currentOrder.longitude = "0.000";
            currentOrder.delivery_date = "today";

            int total = 0;

            foreach (var meal in mealsOrdered)
            {
                if (meal.order_qty > 0)
                {
                    total += meal.order_qty;
                    currentOrder.ordered_items.Add(new Dictionary<string, string>()
                    {
                        { "meal_id", meal.id },
                        { "qty", meal.order_qty.ToString() }
                    });
                }
            }
            currentOrder.totalAmount = total.ToString();

        }

        void SetupUI()
        {

            var mainStackLayout = new StackLayout() { Orientation = StackOrientation.Vertical };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            var grid1 = new StackLayout() { Orientation = StackOrientation.Vertical, MinimumHeightRequest = 150, Margin = 0 };
            var grid1Half = new StackLayout() { Orientation = StackOrientation.Horizontal, MinimumHeightRequest = 150, Margin = 0 };
            var grid2 = new StackLayout() { Orientation = StackOrientation.Vertical, MinimumHeightRequest = 150, Margin = 0 };
            var grid2Half = new StackLayout() { Orientation = StackOrientation.Horizontal, MinimumHeightRequest = 150, Margin = 0 };
            var grid3 = new StackLayout() { Orientation = StackOrientation.Vertical, MinimumHeightRequest = 150, Margin = 0 };
            var grid4 = new StackLayout() { Orientation = StackOrientation.Vertical, MinimumHeightRequest = 50, Margin = 0 };
            var grid5 = new StackLayout() { Orientation = StackOrientation.Horizontal, Margin = 0 };


            var addressFieldStackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, MinimumHeightRequest = 45, HorizontalOptions = LayoutOptions.FillAndExpand };



            var orderNamesStackLayout = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.StartAndExpand, Margin = 10 };
            var orderPriceAndQtyLayout = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.EndAndExpand};
            var imageLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Center };
            var imageTextLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Center};


            foreach (var meal1 in mealsOrdered)
            {
                if (meal1.order_qty >= 1)
                {
                    var mealImage = new Image() { Source = meal1.imageString, HeightRequest = 60, MinimumWidthRequest = 60 };
                    var mealQuantity = new Label() { Text = meal1.order_qty.ToString(), FontSize = 20 };
                    imageLayout.Children.Add(mealImage);
                    imageTextLayout.Children.Add(mealQuantity);
                }
            }


            grid1.Children.Add(cartReviewLabel);
            grid1.Children.Add(nameLabel);
            grid1.Children.Add(imageLayout);
            grid1.Children.Add(imageTextLayout);

            //grid1Half.Children.Add(imageLayout);
            //grid1Half.Children.Add(imageTextLayout);


            //grid2Half.Children.Add(cartImage);
            grid2.Children.Add(confirmLabel);
            grid2.Children.Add(addressLabel);
            //grid2Half.Children.Add(grid2);


            //grid4.Children.Add(orderTitleLabel);

            grid5.Children.Add(orderNamesStackLayout);
            grid5.Children.Add(orderPriceAndQtyLayout);


            grid.Children.Add(grid1);
            //grid.Children.Add(grid1Half);
            grid.Children.Add(grid2Half);
            grid.Children.Add(grid2);
            grid.Children.Add(grid3);
            grid.Children.Add(grid4);
            grid.Children.Add(grid5);

            Grid.SetRow(grid1, 0);
            Grid.SetRow(grid2, 1);
            Grid.SetRow(grid3, 2);
            Grid.SetRow(grid4, 3);
            Grid.SetRow(grid5, 4);


            var scrollView = new ScrollView()
            {
                Content = grid
            };

            var checkoutButton = new Button() { Text = "Confirm Checkout", HeightRequest = 40, Margin = new Thickness(20, 10, 20, 10), BorderWidth = 0.5, BorderColor = Color.Gray};
            checkoutButton.Clicked += Handle_Clicked();


            mainStackLayout.Children.Add(scrollView);
            mainStackLayout.Children.Add(checkoutButton);

            var copyrightLabel = new Label() { Text = "© Infinite Options v1.2", FontSize = 10, HorizontalOptions = LayoutOptions.Center,  Margin = new Thickness(20, 10, 20, 10) };

            mainStackLayout.Children.Add(copyrightLabel);

            Content = new StackLayout
            {
                Children = { mainStackLayout }
            };
        }

        private EventHandler Handle_Clicked()
        {
            return placeOrder;
        }


        async void placeOrder(object sender, System.EventArgs e)
        {
            ((Button)sender).IsEnabled = false;

            await Application.Current.SavePropertiesAsync();

            await sendOrderRequest(currentOrder);
            Device.OpenUri(new System.Uri("https://servingnow.me/payment/" + currentOrder.customer_id));

            //await Navigation.PopModalAsync();
            // "(Copyright Symbol) 2019 Infinite Options   v1.2"
        }
            

        void Auto_Fill(string key, Entry location)
        {
            if (Application.Current.Properties.ContainsKey(key) && Application.Current.Properties[key] != null)
            {
                location.Text = Application.Current.Properties[key].ToString();
            }
        }

        async Task sendOrderRequest(OrderInfo currentOrder)
        {

            var currentOrderJSONString = JsonConvert.SerializeObject(currentOrder);


            Console.WriteLine(currentOrderJSONString);


            //JObject currentOrderJSON = JObject.Parse(currentOrderJSONString);
            //System.Console.WriteLine(currentOrderJSON.GetType());
            var content = new StringContent(currentOrderJSONString, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                //System.Console.WriteLine(JsonConvert.SerializeObject(content));
                var request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                request.Content = content;
                //var responseString = await request.Content.ReadAsStringAsync();
                var httpResponse = await httpClient.PostAsync("https://dc3so1gav1.execute-api.us-west-1.amazonaws.com/dev/api/v2/add_order", content);
                Console.WriteLine("This is the response " + httpResponse);
            }


                //OrderInfo currentOrder2 = new OrderInfo();
                //var currentOrderJSONString = JsonConvert.SerializeObject();



                //HttpResponseMessage response = await client.SendAsync(request);
                //System.Console.WriteLine(response);
                //HttpResponseMessage response = null;

                //string uri = "https://o5yv1ecpk1.execute-api.us-west-2.amazonaws.com/dev/api/v1/meal/order";
                //response = await client.PostAsync(uri, content);
                //var result = await response.Content.ReadAsStringAsync();
            }

        private HttpContent Json(object jsonData, object allowGet)
        {
            throw new NotImplementedException();
        }

        private String FormatCurrency(String number)
        {
            if (!number.Contains("."))
            {
                return number + ".00";
            }
            if (number[number.Length - 2] == '.')
            {
                return number + "0";
            }
            return number;
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

        private string parseAreaToZipcode(string zipcode)
        {
            if (zipcode == "Almaden")
            {
                return "95120";
            }
            if (zipcode == "Evergreen")
            {
                return "95135";
            }
            if (zipcode == "Santa Cruz")
            {
                return "95060";
            }
            if (zipcode == "Other")
            {
                return "90000";
            }
            return "";
        }

    }
}
