using AbojebApi.Utilities.Models.KML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Web.Hosting;

namespace AbojebApi.Utilities.Export
{
    public class KmlFileResult : BaseFileResult<Placemark>
    {
        #region Fields

        private const string _contentType = "application/vnd.google-earth.kml+xml";
        private string _tempPath;

        #endregion

        #region Properties

        public string TempPath
        {
            get
            {
                if (string.IsNullOrEmpty(_tempPath))
                {
                    _tempPath = HostingEnvironment.MapPath(Path.Combine(@"~/App_Data", this.FileDownloadName));
                }
                return _tempPath;
            }
            set
            {
                _tempPath = Path.Combine(value, this.FileDownloadName);
            }
        }

        #endregion

        #region Ctor
        /// <summary>
        /// Creats new instance of KmlFileResult{TEntity}
        /// </summary>
        /// <param name="source">List of data to be transformed to csv</param>
        /// <param name="fileDownloadName">KML file name</param>
        public KmlFileResult(IEnumerable<Placemark> source, string fileDownloadName)
            : base(source, fileDownloadName, _contentType)
        {

        }

        /// <summary>
        /// Creats new instance of KmlFileResult{TEntity}
        /// </summary>
        /// <param name="source">List of data to be transformed to csv</param>
        /// <param name="fileDownloadName">KML file name</param>
        /// <param name="map">Custom transformation delegate</param>
        /// <param name="headers">Columns headers</param>
        public KmlFileResult(IEnumerable<Placemark> source, string fileDownloadName, Func<Placemark, IEnumerable<string>> map, IEnumerable<string> headers)
            : base(source, fileDownloadName, map, headers, _contentType)
        {

        }

        #endregion

        #region override

        protected override void WriteFile(HttpResponseBase response)
        {
            var collection = new PlacemarkCollection();
            collection.Document = (List<Placemark>)DataSource;
            var serialized = Serialize(FileDownloadName, collection);

            if (HasPreamble)
            {
                var preamble = this.ContentEncoding.GetPreamble();
                response.OutputStream.Write(preamble, 0, preamble.Length);
            }

            //XmlDocument doc = new XmlDocument();
            //doc.WriteTo(serialized);
            //var streambuffer = ContentEncoding.GetBytes(doc.OuterXml);
            var streambuffer = File.ReadAllBytes(this.TempPath);
            response.OutputStream.Write(streambuffer, 0, streambuffer.Length);

            if (File.Exists(this.TempPath))
            {
                File.Delete(this.TempPath);
            }
        }

        #endregion

        #region local routines

        private const string KML_NAME_SPACE = "http://www.opengis.net/kml/2.2";

        private XmlWriter Serialize(string fileName, PlacemarkCollection placemarks)
        {
            if (File.Exists(this.TempPath))
            {
                File.Delete(this.TempPath);
            }

            var serializer = new XmlSerializer(typeof(PlacemarkCollection),
                KML_NAME_SPACE);
            using (var stream = new FileStream(this.TempPath, FileMode.Create))
            {
                using (var writer = new XmlTextWriter(stream, Encoding.Unicode))
                {
                    var namespaces = new XmlSerializerNamespaces();
                    namespaces.Add(string.Empty, KML_NAME_SPACE);
                    serializer.Serialize(writer, placemarks, namespaces);
                    return writer;
                }
            }
        }

        #endregion
    }
}
