using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

//Converts C# values into json, before sending it to the DB

namespace InfiniteMeals.Model.SignUp {

    // object to send to database when user attempts to sign up 
    // link: https://uavi7wugua.execute-api.us-west-1.amazonaws.com/dev/api/v2/signup
    public class SignUpPost {

        [JsonProperty("id")]
        public string CustomerId { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("address1")]
        public string Address1 { get; set; }

        [JsonProperty("address2")]
        public string Address2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("zipcode")]
        public String Zipcode { get; set; }

        [JsonProperty("phone")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("user_is_customer")]
        public int UserIsCustomer { get; set; }

        [JsonProperty("user_is_donor")]
        public int UserIsDonor { get; set; }

        [JsonProperty("user_is_admin")]
        public int UserIsAdmin { get; set; }

        [JsonProperty("user_is_foodbank")]
        public int UserIsFoodbank { get; set; }

        [JsonProperty("password")]
        public String Password { get; set; }
    }
}
