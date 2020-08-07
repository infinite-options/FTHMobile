namespace InfiniteMeals.Model.Login
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class LoginResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("result")]
        public LoginResponseResult Result { get; set; }

        [JsonProperty("auth_success")]
        public bool AuthSuccess { get; set; }

        [JsonProperty("login_attempt_log")]
        public LoginAttemptLog LoginAttemptLog { get; set; }
    }

    public partial class LoginAttemptLog
    {
        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        [JsonProperty("login_id")]
        public string LoginId { get; set; }
    }

    public partial class LoginResponseResult
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("result")]
        public ResultElement[] Result { get; set; }

        [JsonProperty("sql")]
        public string Sql { get; set; }
    }

    public partial class ResultElement
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("user_is_customer")]
        public long UserIsCustomer { get; set; }

        [JsonProperty("user_is_donor")]
        public long UserIsDonor { get; set; }

        [JsonProperty("user_is_admin")]
        public long UserIsAdmin { get; set; }

        [JsonProperty("user_is_foodbank")]
        public long UserIsFoodbank { get; set; }

        [JsonProperty("user_first_name")]
        public string UserFirstName { get; set; }

        [JsonProperty("user_last_name")]
        public string UserLastName { get; set; }

        [JsonProperty("user_address1")]
        public string UserAddress1 { get; set; }

        [JsonProperty("user_address2")]
        public string UserAddress2 { get; set; }

        [JsonProperty("user_city")]
        public string UserCity { get; set; }

        [JsonProperty("user_state")]
        public string UserState { get; set; }

        [JsonProperty("user_zipcode")]
        public long UserZipcode { get; set; }

        [JsonProperty("user_phone")]
        public string UserPhone { get; set; }

        [JsonProperty("user_email")]
        public string UserEmail { get; set; }

        [JsonProperty("user_join_date")]
        public string UserJoinDate { get; set; }

        [JsonProperty("user_email_verified")]
        public long UserEmailVerified { get; set; }
    }
}
