using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;

using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using InfiniteMeals.Model.Login;
using InfiniteMeals.Model.User;
using System.Threading.Tasks;
using InfiniteMeals.Model.Database;
using System.Windows.Input;

namespace InfiniteMeals
{
    public partial class Login : ContentPage
    {
        const string accountSaltURL = "https://uavi7wugua.execute-api.us-west-1.amazonaws.com/dev/api/v2/accountsalt/"; // api to get account salt; need email at the end of link
        const string loginURL = "https://dc3so1gav1.execute-api.us-west-1.amazonaws.com/dev/api/v2/login"; // api to log in; need email + hashed password at the end of link
        public HttpClient client = new HttpClient(); // client to handle all api calls

        public Login()
        {
            InitializeComponent();
            BackgroundImageSource = "newBack.jpg";
        }

        private async void ClickedLogin(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.loginEmail.Text) && String.IsNullOrEmpty(this.loginPassword.Text))
            { // check if all fields are filled out
                await DisplayAlert("Error", "Please fill in all fields", "OK");
            }
            else
            {
                
                var loginAttempt = await login(this.loginEmail.Text, this.loginPassword.Text);
                if (loginAttempt != null && loginAttempt.Message != "Request failed, wrong password.")
                { // make sure the login attempt was successful
                    captureLoginSession(loginAttempt);
                    await Navigation.PopAsync();

                }
            }
        }

        // logs the user into the app 
        // returns a LoginResponse if successful and null if unsuccessful 
        public async Task<LoginResponse> login(string userEmail, string userPassword)
        {
            try
            {
                LoginPost loginPostContent = new LoginPost()
                { // object that contains ip address and browser type; will be converted into a json object 
                    Email = userEmail,
                    Password = userPassword
                };

                string loginPostContentJson = JsonConvert.SerializeObject(loginPostContent); // make orderContent into json

                var httpContent = new StringContent(loginPostContentJson, Encoding.UTF8, "application/json"); // encode orderContentJson into format to send to database
                var response = await client.PostAsync(loginURL, httpContent); // try to post to database

                if (response.Content != null)
                { // post was successful
                    var responseContent = await response.Content.ReadAsStringAsync();

                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseContent);
                    return loginResponse;

                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error in login.xaml " + e);
            }
            return null;
        }

        // uses account salt api to retrieve the user's account salt
        // account salt is used to find the user's hashed password
        public async Task<AccountSalt> retrieveAccountSalt(string userEmail)
        {
            try
            {
                var content = await client.GetStringAsync(accountSaltURL + userEmail); // get the requested account salt
                var accountSalt = JsonConvert.DeserializeObject<AccountSalt>(content);
                System.Diagnostics.Debug.WriteLine("account salt good");
                return accountSalt;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }
            return null;
        }

        // navigates the user to the sign up page
        private async void SignUpClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUp());
        }


        public async void captureLoginSession(LoginResponse loginResponse)
        {
            var userSessionInformation = new UserLoginSession
            { // object to send into local database
                UserUid = loginResponse.Result.Result[0].UserId,
                FirstName = loginResponse.Result.Result[0].UserFirstName,
                LastName = loginResponse.Result.Result[0].UserLastName,
                SessionId = loginResponse.LoginAttemptLog.SessionId,
                LoginId = loginResponse.LoginAttemptLog.LoginId,
                PhoneNumber = loginResponse.Result.Result[0].UserPhone,
                Address1 = loginResponse.Result.Result[0].UserAddress1,
                Address2 = loginResponse.Result.Result[0].UserAddress2,
                City = loginResponse.Result.Result[0].UserCity,
                State = loginResponse.Result.Result[0].UserState,
                Street = loginResponse.Result.Result[0].UserAddress1,
                Zipcode = loginResponse.Result.Result[0].UserZipcode.ToString(),
                Email = loginResponse.Result.Result[0].UserEmail
            };

            await App.Database.SaveItemAsync(userSessionInformation); // send login session to local database
            App.setLoggedIn(true);
            var mainPage = new MainPage(userSessionInformation);
            await Navigation.PushAsync(mainPage);

            //This is to change "login" button to "logout" button. Not implemented
            //mainPage.updateLoginButton();
        }
    }
}
