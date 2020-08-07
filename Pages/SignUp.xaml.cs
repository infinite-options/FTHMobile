using InfiniteMeals.Model.Database;
using InfiniteMeals.Model.Login;
using InfiniteMeals.Model.SignUp;
using InfiniteMeals.Model.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InfiniteMeals
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUp : ContentPage
    {
        public Boolean termsOfServiceChecked = true;
        public Boolean weeklyUpdatesChecked = false;
        public HttpClient client = new HttpClient();
        public const string signUpApi = "https://dc3so1gav1.execute-api.us-west-1.amazonaws.com/dev/api/v2/signup";
        const string accountSaltURL = "https://uavi7wugua.execute-api.us-west-1.amazonaws.com/dev/api/v2/accountsalt/"; // api to get account salt; need email at the end of link
        const string loginURL = "https://dc3so1gav1.execute-api.us-west-1.amazonaws.com/dev/api/v2/login"; // api to log in; need email + hashed password at the end of link

        public SignUp()
        {
             
            InitializeComponent();

        }

        private async void Signup_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("begin check: " + (Navigation == null).ToString());
            System.Diagnostics.Debug.WriteLine("begin navigation count: " + Navigation.NavigationStack.Count);
            if (String.IsNullOrEmpty(this.firstNameEntry.Text) || String.IsNullOrEmpty(this.lastNameEntry.Text) || String.IsNullOrEmpty(this.emailEntry.Text) ||
                String.IsNullOrEmpty(this.confirmEmailEntry.Text) || String.IsNullOrEmpty(this.passwordEntry.Text) || String.IsNullOrEmpty(this.confirmPasswordEntry.Text) ||
                String.IsNullOrEmpty(this.phoneNumberEntry.Text))
            { // fields are empty  
                await DisplayAlert("Error", "Please fill all fields", "OK");
                return;
            }
            else
            {
                if (!validateEmail(this.emailEntry.Text) || !validateEmail(this.confirmEmailEntry.Text))
                { // email isn't valid
                    await DisplayAlert("Error", "Please enter a valid email", "OK");
                    return;
                }
                else
                {
                    if (this.emailEntry.Text != this.confirmEmailEntry.Text)
                    { // email and confirm email don't match
                        await DisplayAlert("Error", "Emails do not match", "OK");
                        return;
                    }
                    else
                    {
                        if (this.passwordEntry.Text != this.confirmPasswordEntry.Text)
                        { // passwords don't match
                            await DisplayAlert("Error", "Passwords do not match", "OK");
                            return;
                        }
                        else
                        {
                            if (this.phoneNumberEntry.Text.Length < 10)
                            { // phone number isn't valid 
                                await DisplayAlert("Error", "Please enter a valid phone number", "OK");
                            }
                            else
                            {
                                if (!termsOfServiceChecked)
                                { // terms of service aren't checked

                                    await DisplayAlert("Error", "The Terms of Service must be accepted", "OK");
                                    return;
                                }
                                else
                                {
                                    var signUpAttempt = await signUp();

                                    if (signUpAttempt != null)
                                    { // successful sign up 
                                        if (signUpAttempt.Result == "Email address taken.")
                                        {
                                            await DisplayAlert("Error", "That email is already taken", "OK");
                                            return;
                                        }

                                        await Navigation.PopToRootAsync();

                                        //var loginAttempt = await login(this.emailEntry.Text, this.passwordEntry.Text, accountSalt);
                                        //System.Diagnostics.Debug.WriteLine("logging in");
                                        //if (loginAttempt != null && loginAttempt.Message != "Request failed, wrong password.")
                                        //{
                                        //    System.Diagnostics.Debug.WriteLine("capturing login");
                                        //    captureLoginSession(loginAttempt);
                                            
                                        //}
                                        //else
                                        //{
                                        //    await DisplayAlert("Error", "There was an error logging in", "OK");
                                        //    return;
                                        //}


                                    }
                                    else
                                    {
                                        await DisplayAlert("Error", "There was an error creating the account", "OK");
                                        return;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            //await Navigation.PopToRootAsync(); 
        }


        public void Button_Clicked(object sender, EventArgs e)
        {
            //Supposed to respond to the allergies clicked by the user
        }


            // validates an email 
            // returns true if the email is valid and false otherwise
            public Boolean validateEmail(string email)
        {
            try
            {
                MailAddress address = new MailAddress(email);
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }

        }


        // signs the user up based on the information filled in the entries
        // returns a SignUpResponse if successful and null if unsuccessful
        private async Task<SignUpResponse> signUp()
        {
            var addressLine2 = "";
            if (Address2.Text == null)
            {
                addressLine2 = "";
            }
            SignUpPost signUpContent = new SignUpPost
            { // SignUpPost object to send to database 
                FirstName = this.firstNameEntry.Text,
                LastName = this.lastNameEntry.Text,
                Address1 = this.Address1.Text,
                Address2 = addressLine2,
                City = this.City.Text,
                State = this.State.Text,
                Zipcode = this.Zipcode.Text,
                PhoneNumber = this.phoneNumberEntry.Text,
                Email = this.emailEntry.Text,
                Password = this.passwordEntry.Text,
                UserIsCustomer = 1,
                UserIsDonor = 1,
                UserIsAdmin = 1,
                UserIsFoodbank = 1
            };

        string signUpContentJson = JsonConvert.SerializeObject(signUpContent); // convert to json 
            var httpContent = new StringContent(signUpContentJson, Encoding.UTF8, "application/json"); // convert to string content
            try
            {
                var response = await client.PostAsync(signUpApi, httpContent); // send to database

                if (response.Content != null)
                { // successful post 
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var signUpResponse = JsonConvert.DeserializeObject<SignUpResponse>(responseContent);
                    return signUpResponse; // return the response from the api
                }
                return null;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }

        }

        // uses account salt api to retrieve the user's account salt
        // account salt is used to find the user's hashed password
        public async Task<AccountSalt> retrieveAccountSalt(string userEmail)
        {
            try
            {
                var content = await client.GetStringAsync(accountSaltURL + userEmail); // get the requested account salt
                var accountSalt = JsonConvert.DeserializeObject<AccountSalt>(content);
                return accountSalt;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);

            }
            return null;
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
            catch (Exception e)
            {
                Console.WriteLine("Error in login.xaml " + e);
            }
            return null;
        }

        // captures a login session and sends it to the local database
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
            App.setLoggedIn(true); // update the login status for the app
            System.Diagnostics.Debug.WriteLine("change main page");

            var mainPage = new MainPage(userSessionInformation);
            await Navigation.PushAsync(mainPage);
            //mainPage.updateLoginButton(); // update login button to be login / logout - not implemented
            //await Navigation.PopToRootAsync();
            await DisplayAlert("Success", "Your account was created", "OK");
        }


    }
}

