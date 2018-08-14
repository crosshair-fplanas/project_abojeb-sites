using System.Text;
using System.Xml.Serialization;

namespace AbojebApi.Utilities.Models.KML
{
    public class Location
    {
        private const string TOKEN = ",";

        public Location()
        {
            Altitude = 0;
        }

        [XmlIgnore()]
        public double Latitude { get; set; }

        [XmlIgnore()]
        public double Longitude { get; set; }

        [XmlIgnore()]
        public int Altitude { get; set; }

        [XmlElement("coordinates")]
        public string Coordinates
        {
            get
            {
                var sCoordinates = new StringBuilder();
                sCoordinates.Append(Longitude.ToString() + TOKEN);
                sCoordinates.Append(Latitude.ToString() + TOKEN);
                sCoordinates.Append(Altitude.ToString());
                return sCoordinates.ToString();
            }
            set { }
        }
    }
}
