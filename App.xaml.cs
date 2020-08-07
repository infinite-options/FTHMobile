using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InfiniteMeals.Model.Database;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using InfiniteMeals.Meals.Model;



[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace InfiniteMeals
{
    public partial class App : Application
    {
        static UserLoginDatabase database;
        static Boolean loggedIn = false;

        public NavigationPage Login { get; }



        public App()
        {
            InitializeComponent();

            if (database == null)
            {
                database = new UserLoginDatabase();
            }

            MainPage = new NavigationPage(new Login())
            {
                BarBackgroundColor = Color.White,
            };
        }



        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }


        public static UserLoginDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new UserLoginDatabase();
                }
                return database;
            }
        }


        public static Boolean LoggedIn
        {
            get
            {
                return loggedIn;
            }
        }

        public static void setLoggedIn(Boolean loggedIn)
        {
            App.loggedIn = loggedIn;
        }
    }
}
