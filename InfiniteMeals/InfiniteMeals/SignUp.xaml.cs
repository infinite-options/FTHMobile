using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;


namespace InfiniteMeals
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class SignUp : ContentPage
    {
        public SignUp()
        {
            InitializeComponent();

        }

        public static string GetXMLElement(XElement element, string name)
        {
            var el = element.Element(name);
            if (el != null)
            {
                return el.Value;
            }
            return "";
        }

        public static string GetXMLAttribute(XElement element, string name)
        {
            var el = element.Attribute(name);
            if (el != null)
            {
                return el.Value;
            }
            return "";
        }

        private async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            // Case of what happend?
            XDocument requestDoc = new XDocument(
                new XElement("AddressValidateRequest",
                new XAttribute("USERID", "400INFIN1745"),
                new XElement("Revision", "1"),
                new XElement("Address",
                new XAttribute("ID", "0"),
                new XElement("Address1", Address.Text),
                new XElement("Address2", ""),
                new XElement("City", City.Text),
                new XElement("State", State.Text),
                new XElement("Zip5", Zipcode.Text),
                new XElement("Zip4", "")
                     )
                 )
             );
            var url = "http://production.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" + requestDoc;
            Console.WriteLine(url);
            var client = new WebClient();
            var response = client.DownloadString(url);

            var xdoc = XDocument.Parse(response.ToString());

            foreach (XElement element in xdoc.Descendants("Address"))
            {
                if (GetXMLElement(element, "Error").Equals(""))
                {
                    if (GetXMLElement(element, "DPVConfirmation").Equals("Y")) // Best case
                    {
                        // Get longitude and latitide because we can make a deliver here. Move on to next page.
                        // Console.WriteLine("The address you entered is valid and deliverable by USPS. We are going to get its latitude & longitude");
                        GetAddressLatitudeLongitude();
                    }
                    else if (GetXMLElement(element, "DPVConfirmation").Equals("D"))
                    {
                        AddressIsMissingSomething();
                    }
                    else
                    {
                        AddressNotValid();
                    }


                }
                else
                {   // USPS sents an error saying address not found in there records. In other words, this address is not valid because it does not exits.
                    Console.WriteLine("Error from USPS. The address you entered was not found");
                    AddressNotFound();
                }

            }
        }

        private async void GetAddressLatitudeLongitude()
        {
            Geocoder geoCoder = new Geocoder();

            IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync("1110 Calle Almaden, San Jose, California");
            Position position = approximateLocations.FirstOrDefault();

            string coordinates = $"{position.Latitude}, {position.Longitude}";
            Console.WriteLine(coordinates);
            await DisplayAlert("Coordinates", coordinates, "Ok");
        }

        private async void AddressNotValid()
        {
            await DisplayAlert("Alert!", "Addres not valid.", "Ok");
        }

        private async void AddressIsMissingSomething()
        {
            await DisplayAlert("Alert!", "Address is missing information like 'Apartment number'.", "Ok");
        }

        private async void AddressNotFound()
        {
            await DisplayAlert("Alert!", "Error from USPS. The address you entered was not found.", "Ok");
        }
    }
}
