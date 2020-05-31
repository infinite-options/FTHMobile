using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace InfiniteMeals
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private void clearText(Entry sender, System.EventArgs e)
        {
            sender.Text = "";
        }

        async void loginClick(object sender, System.EventArgs e)
        {
            var loggedIn = new MainPage();
            await Navigation.PushAsync(loggedIn);
        }

        async void signUpClick(object sender, System.EventArgs e)
        {
            var signUpPage = new SignUp();
            await Navigation.PushAsync(signUpPage);
        }
    }
}
