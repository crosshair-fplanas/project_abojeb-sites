using System.Collections.Generic;
using System.Xml.Serialization;

namespace AbojebApi.Utilities.Models.KML
{
    [XmlRoot("kml")]
    public class PlacemarkCollection
    {
        public PlacemarkCollection()
        {
            Document = new List<Placemark>();
        }

        [XmlArrayItem("Placemark")]
        public List<Placemark> Document { get; set; }
    }
}
