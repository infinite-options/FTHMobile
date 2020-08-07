using System;
namespace InfiniteMeals.Kitchens.Model
{
    public class KitchensModel
    {
        public string foodbank_name { get; set; }
        public string tag_line { get; set; }
        public string foodbank_zip { get; set; }
        public string foodbank_id { get; set; }
        public string delivery_hours { get; set; }
        public string pickup_hours { get; set; }
        public string foodbank_address { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public int delivery { get; set; }
        public int pickup { get; set; }


        public string title { get; set; }
        public string description { get; set; }
        public string zipcode { get; set; }
        public string delivery_period { get; set; }
        public string order_period { get; set; }
        public bool isOpen { get; set; }
        public string status { get; set; }
        public string statusColor { get; set; }
        public string opacity { get; set; }
    }
}
