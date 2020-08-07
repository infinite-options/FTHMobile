using System;
using System.Collections.Generic;

namespace InfiniteMeals.Kitchens.Model
{
    public class UPS
    {
        public List<Address> XAVRequest { get; set; }
    }



    public class Address
    {
        public List<Addresses> AddressKeyFormat { get; set; }
    }

    public class Addresses
    {
        public List<String> AddressKeyFormat { get; set; }
    }
}
