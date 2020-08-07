using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace InfiniteMeals.Model.Login {

    /* contains information to send to the login api
       link: https://uavi7wugua.execute-api.us-west-1.amazonaws.com/dev/api/v2/account/
       full link is https://uavi7wugua.execute-api.us-west-1.amazonaws.com/dev/api/v2/account/email/password
       email is user's email, password is hashed password */

    class LoginPost {

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
