using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AbojebApi.Utilities.Export
{
    /// <summary>
    /// CSV file result impementation
    /// </summary>
    /// <typeparam name="TEntity">Entity list to transform to CSV</typeparam>
    public class CsvFileResult<TEntity> : BaseFileResult<TEntity> where TEntity : class
    {
        #region Fields

        private const string _contentType = "text/csv";
        private string _delimiter;
        private string _lineBreak;

        #endregion

        #region Properties

        public string Delimiter
        {
            get
            {
                if (string.IsNullOrEmpty(this._delimiter))
                {
                    this._delimiter = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
                }

                return this._delimiter;
            }

            set { this._delimiter = value; }
        }

        /// <summary>
        /// Line  delimiter \n
        /// </summary>
        public string LineBreak
        {
            get
            {
                if (string.IsNullOrEmpty(this._lineBreak))
                {
                    this._lineBreak = Environment.NewLine;
                }

                return this._lineBreak;
            }

            set { this._lineBreak = value; }
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Creats new instance of CsvFileResult{TEntity}
        /// </summary>
        /// <param name="source">List of data to be transformed to csv</param>
        /// <param name="fileDownloadName">CSV file name</param>
        public CsvFileResult(IEnumerable<TEntity> source, string fileDownloadName)
            : base(source, fileDownloadName, _contentType)
        {

        }

        /// <summary>
        /// Creats new instance of CsvFileResult{TEntity}
        /// </summary>
        /// <param name="source">List of data to be transformed to csv</param>
        /// <param name="fileDownloadName">CSV file name</param>
        /// <param name="map">Custom transformation delegate</param>
        /// <param name="headers">Columns headers</param>
        public CsvFileResult(IEnumerable<TEntity> source, string fileDownloadName, Func<TEntity, IEnumerable<string>> map, IEnumerable<string> headers)
            : base(source, fileDownloadName, map, headers, _contentType)
        {

        }

        #endregion

        #region override

        protected override void WriteFile(HttpResponseBase response)
        {
            response.ContentEncoding = this.ContentEncoding;
            response.BufferOutput = this.BufferOutput;
            if (HasPreamble)
            {
                var preamble = this.ContentEncoding.GetPreamble();
                response.OutputStream.Write(preamble, 0, preamble.Length);
            }

            var streambuffer = ContentEncoding.GetBytes(this.GetCSVData());
            response.OutputStream.Write(streambuffer, 0, streambuffer.Length);
        }

        #endregion

        #region local routines

        private string GetCSVHeader()
        {
            string csv = "";
            csv = String.Join(this.Delimiter, this.Headers.Select(x => this.FormatCSV(x)));

            return csv;
        }

        private string GetCSVData()
        {
            string csv = GetCSVHeader();
            Func<TEntity, string> expr = x => this.Map == null ? this.FormatPropertiesCSV(x) : this.FormatMapCSV(x);
            csv += this.LineBreak + String.Join(this.LineBreak, this.DataSource.Select(expr));
            return csv;
        }

        private string FormatCSV(string str)
        {
            str = (str ?? "").Replace(this.Delimiter, "\"" + this.Delimiter + "\"");
            str = str.Replace(this.LineBreak, "\"" + this.LineBreak + "\"");
            str = str.Replace("\"", "\"\"");

            return String.Format("\"{0}\"", str);
        }

        private string FormatPropertiesCSV(TEntity obj)
        {
            string csv = "";

            foreach (var pi in this.SourceProperties)
            {
                string val = GetPropertyValue(pi, obj);
                csv += FormatCSV(val) + this.Delimiter;
            }

            csv = csv.TrimEnd(this.Delimiter.ToCharArray());
            return csv;
        }

        private string GetPropertyValue(PropertyInfo pi, object source)
        {
            try
            {
                var result = pi.GetValue(source, null);
                return (result == null) ? "" : result.ToString();
            }
            catch (Exception)
            {
                return "Can not obtain the value";
            }
        }

        private string FormatMapCSV(TEntity obj)
        {
            return String.Join(this.Delimiter, this.Map(obj).Select(x => FormatCSV(x)));
        }

        #endregion
    }
}
