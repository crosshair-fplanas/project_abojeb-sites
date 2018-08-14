using System;
using System.Globalization;
using System.Xml.Serialization;

namespace AbojebApi.Utilities.Models.KML
{
    public class Placemark
    {
        public Placemark()
        {

        }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("Point")]
        public Location Point { get; set; }
    }
}
