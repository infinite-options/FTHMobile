using System;
using System.ComponentModel;

namespace InfiniteMeals.Meals.Model
{
    public class AddressModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public int order_qty { get; set; }
        public String[] orderList { get; set; }

        //public int meal_qty;
        public int qty
        {
            get { return order_qty; }
            set
            {
                order_qty = value;
                PropertyChanged(this, new PropertyChangedEventArgs("order_qty"));
            }
        }
    }
}
