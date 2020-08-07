using System;
using System.ComponentModel;

namespace InfiniteMeals.Meals.Model
{
    public class MealsModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string imageString { get; set; }
        public string food_name { get; set; }
        public string price { get; set; }
        public string id { get; set; }
        public string foodbank_id { get; set; }
        public int order_qty { get; set; }
        public int delivery { get; set; }
        public int pickup { get; set; }
        public double order_opacity { get; set; }

        public double opacity {
            get { return order_opacity; }
            set
            {
                order_opacity = value;
                PropertyChanged(this, new PropertyChangedEventArgs("order_opacity"));
            }
        }

        public int qty {
            get { return order_qty; }
            set
            {
                order_qty = value;
                PropertyChanged(this, new PropertyChangedEventArgs("order_qty"));
            }
        }
    }
}
