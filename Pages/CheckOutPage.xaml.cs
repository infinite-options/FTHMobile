using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InfiniteMeals.Meals.Model;
using Newtonsoft.Json;
using Xamarin.Forms;
using InfiniteMeals.Model.Database;




namespace InfiniteMeals
{
    public partial class OrderInfo
    {
        public string customer_id = "";
        public string phone = "";
        public string street = "";
        public string city = "";
        public string state = "";
        public string zipcode = "";
        public int totalAmount = 0;
        public string delivery_note = "";
        public string kitchen_id = "";
        public string longitude = "";
        public string latitude = "";
        public string delivery_date = "";
        public string order_type = "";
        public List<Dictionary<string, string>> ordered_items = new List<Dictionary<string, string>>();
    }

    public partial class CheckOutPage : ContentPage
    {
        OrderInfo currentOrder = new OrderInfo();
        ObservableCollection<MealsModel> mealsOrdered = new ObservableCollection<MealsModel>();
        UserLoginSession userSesh = new UserLoginSession();

        double totalCostsForMeals = 0;
        double calculatedTaxAmount = 0;
        string kitchenAddress;
        
        public Label cartReviewLabel = new Label() { VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 21, HeightRequest = 45, MinimumWidthRequest = 170, Text = "Cart Review", Margin = 5};
        public Label nameLabel = new Label() { VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 18, HeightRequest = 45, MinimumWidthRequest = 170, Text = "Default Food Bank", Margin = 8 };
        public Image logoImage = new Image() { VerticalOptions = LayoutOptions.Center, Source = "logo.png", HeightRequest = 60, MinimumWidthRequest = 170};
        public Label confirmLabel = new Label() { VerticalOptions = LayoutOptions.Center, FontSize = 25, HeightRequest = 25, MinimumWidthRequest = 170, Text = "Confirm Pickup", Margin = 0 };
        public Label addressLabel = new Label() { VerticalOptions = LayoutOptions.Center, FontSize = 12, HeightRequest = 25, WidthRequest = 170, Text = "Default Address", Margin = 5 };
        public Button pickupCheckoutButton = new Button() { FontSize = 25, Text = "Pickup Checkout", Margin = 5, BackgroundColor=Color.White, HeightRequest=50 };
        public Label confirmDeliveryLabel = new Label() { VerticalOptions = LayoutOptions.Center, FontSize = 25, HeightRequest = 25, MinimumWidthRequest = 170, Text = "Confirm Delivery", Margin = 0 };
        public Image truckImage = new Image() { VerticalOptions = LayoutOptions.Center, Source = "truck.png", HeightRequest = 40, MinimumWidthRequest = 130 };
        public Button deliveryButton = new Button() { FontSize = 25, Text = "Delivery Checkout", Margin = 5, BackgroundColor = Color.White, HeightRequest = 50 };

        public CheckOutPage(ObservableCollection<MealsModel> meals, string foodbank_id, string foodbankName, string kitchen_address, UserLoginSession userLoginSession)
        {
            InitializeComponent();

            userSesh = userLoginSession;
            kitchenAddress = kitchen_address;
            mealsOrdered = meals;

            SetupUI();

            currentOrder.kitchen_id = foodbank_id;
            currentOrder.customer_id = userSesh.ID.ToString();
            currentOrder.phone = userSesh.PhoneNumber;
            currentOrder.street = userSesh.Street;
            currentOrder.city = userSesh.City;
            currentOrder.state = userSesh.State;
            currentOrder.zipcode = userSesh.Zipcode;
            currentOrder.delivery_note = "Test delivery note";
            currentOrder.latitude = "0.00";
            currentOrder.longitude = "0.00";
            currentOrder.delivery_date = timePicker.Time.ToString();

            nameLabel.Text = foodbankName;
            addressLabel.Text = kitchen_address;

            int total = 0;

            foreach (var meal in mealsOrdered)
            {
                if (meal.order_qty > 0)
                {
                    total += meal.order_qty;
                    Dictionary<string, string> mealDict = new Dictionary<string, string>
                    {
                        { "meal_id", meal.id },
                        { "qty", meal.order_qty.ToString() }
                    };
                    currentOrder.ordered_items.Add(mealDict);
                }
            }
            currentOrder.totalAmount = total;
        }

        TimePicker timePicker = new TimePicker
        {
            Time = new TimeSpan(4, 15, 26) // Time set to "04:15:26"
        };

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
            var grid1Half = new StackLayout() { Orientation = StackOrientation.Horizontal, MinimumHeightRequest = 80, Margin = 0 };
            var grid2 = new StackLayout() { Orientation = StackOrientation.Vertical, MinimumHeightRequest = 50, Margin = 0 };
            var grid2Half = new StackLayout() { HorizontalOptions = LayoutOptions.Center, Orientation = StackOrientation.Horizontal, MinimumHeightRequest = 50, Margin = 20 };
            var grid3 = new StackLayout() { Orientation = StackOrientation.Vertical, MinimumHeightRequest = 50, Margin = 0 };
            var grid4 = new StackLayout() { HorizontalOptions = LayoutOptions.Center, Orientation = StackOrientation.Horizontal, MinimumHeightRequest = 150, Margin = 10};
            var grid5 = new StackLayout() { Orientation = StackOrientation.Vertical, MinimumHeightRequest = 150, Margin = 0, HorizontalOptions = LayoutOptions.Center, TranslationY = 10  };


            var addressFieldStackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, MinimumHeightRequest = 45, HorizontalOptions = LayoutOptions.FillAndExpand };
            var orderNamesStackLayout = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.StartAndExpand, Margin = 10 };
            var orderPriceAndQtyLayout = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.EndAndExpand};
            var imageLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Center };
            var imageTextLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Center};

            var deliveryOnly = false;
            var pickupOnly = false;

            foreach (var meal in mealsOrdered)
            {
                if (meal.order_qty >= 1)
                {
                    var imageText = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions=LayoutOptions.Center, MinimumHeightRequest = 0};
                    var mealImage = new Image() { Source = meal.imageString, HeightRequest = 70, MinimumWidthRequest = 70 };
                    var mealQuantity = new Label() { Text = meal.order_qty.ToString(), FontSize = 20, TranslationY = 0, TranslationX = 30, TextColor=Color.Black };
                    imageText.Children.Add(mealImage);
                    imageText.Children.Add(mealQuantity);
                    imageLayout.Children.Add(imageText);
                    if (meal.delivery == 0)
                    {
                        pickupOnly = true;
                    }
                    else if (meal.pickup == 0)
                    {
                        deliveryOnly = true;
                    }
                }
            }


            grid1.Children.Add(cartReviewLabel);
            grid1.Children.Add(nameLabel);
            grid1.Children.Add(imageLayout);
            grid1.Children.Add(imageTextLayout);


            if (!deliveryOnly)
            {
                grid2.Children.Add(confirmLabel);
                grid2.Children.Add(addressLabel);
                grid2Half.Children.Add(logoImage);
                grid2Half.Children.Add(grid2);
                grid3.Children.Add(pickupCheckoutButton);
            }

            if (!pickupOnly)
            {
                grid4.Children.Add(confirmDeliveryLabel);
                grid4.Children.Add(truckImage);
                grid5.Children.Add(timePicker);
                grid5.Children.Add(deliveryButton);
            }

            grid.Children.Add(grid1);
            grid.Children.Add(grid2Half);
            grid.Children.Add(grid3);
            grid.Children.Add(grid4);
            grid.Children.Add(grid5);

            Grid.SetRow(grid1, 0);
            Grid.SetRow(grid2Half, 1);
            Grid.SetRow(grid3, 2);
            Grid.SetRow(grid4, 3);
            Grid.SetRow(grid5, 4);


            var scrollView = new ScrollView()
            {
                Content = grid
            };


            deliveryButton.Clicked += Handle_Clicked("delivery");
            pickupCheckoutButton.Clicked += Handle_Clicked("pickup");

            mainStackLayout.Children.Add(scrollView);
            mainStackLayout.Children.Add(deliveryButton);

            var copyrightLabel = new Label() { Text = "© Infinite Options v0.9", FontSize = 10, HorizontalOptions = LayoutOptions.Center,  Margin = new Thickness(20, 10, 20, 10) };

            mainStackLayout.Children.Add(copyrightLabel);

            Content = new StackLayout
            {
                Children = { mainStackLayout }
            };
        }

        private EventHandler Handle_Clicked(String deliveryOrCheckout)
        {
            currentOrder.order_type = deliveryOrCheckout;
            return placeOrder;
        }


        async void placeOrder(object sender, System.EventArgs e)
        {
            ((Button)sender).IsEnabled = false;

            await Application.Current.SavePropertiesAsync();
            await sendOrderRequest(currentOrder);
            await DisplayAlert("Order Placed!", "Thank you for shopping with Feed the Hungry!", "OK");
            await Navigation.PushAsync(new MainPage(userSesh));
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

            var content = new StringContent(currentOrderJSONString, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                request.Content = content;
                var httpResponse = await httpClient.PostAsync("https://dc3so1gav1.execute-api.us-west-1.amazonaws.com/dev/api/v2/add_order", content);
                Console.WriteLine("This is the order's response " + httpResponse);
            }
        }

        private HttpContent Json(object jsonData, object allowGet)
        {
            throw new NotImplementedException();
        }
    }
}
