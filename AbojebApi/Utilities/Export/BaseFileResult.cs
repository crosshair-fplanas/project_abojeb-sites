using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AbojebApi.Utilities.Export
{
    public abstract class BaseFileResult<TEntity> : FileResult where TEntity : class
    {
        #region Fields

        protected Encoding _contentEncoding;
        protected IEnumerable<string> _headers;
        protected IEnumerable<PropertyInfo> _sourceProperties;
        protected IEnumerable<TEntity> _dataSource;
        protected Func<TEntity, IEnumerable<string>> _map;

        #endregion

        #region Properties

        public Func<TEntity, IEnumerable<string>> Map
        {
            get
            {
                return _map;
            }
            set { _map = value; }
        }

        public IEnumerable<TEntity> DataSource
        {
            get
            {
                return this._dataSource;
            }
        }

        /// <summary>
        /// Content Encoding (default is UTF8).
        /// </summary>
        public Encoding ContentEncoding
        {

            get
            {
                if (this._contentEncoding == null)
                {
                    this._contentEncoding = Encoding.Unicode;
                }

                return this._contentEncoding;
            }

            set { this._contentEncoding = value; }


        }

        /// <summary>
        /// the first line of the CSV file, column headers
        /// </summary>
        public IEnumerable<string> Headers
        {
            get
            {
                if (this._headers == null)
                {
                    this._headers = typeof(TEntity).GetProperties().Select(x => x.Name);
                }

                return this._headers;
            }

            set { this._headers = value; }
        }

        public IEnumerable<PropertyInfo> SourceProperties
        {
            get
            {
                if (this._sourceProperties == null)
                {
                    this._sourceProperties = typeof(TEntity).GetProperties();
                }

                return this._sourceProperties;
            }
        }

        /// <summary>
        ///  byte order mark (BOM)  .
        /// </summary>
        public bool HasPreamble { get; set; }

        /// <summary>
        /// Get or Set the response output buffer 
        /// </summary>
        public bool BufferOutput { get; set; }

        #endregion

        #region Ctor
        /// <summary>
        /// Creats new instance of BaseCsvFileResult{TEntity}
        /// </summary>
        /// <param name="source">List of data to be transformed to csv</param>
        /// <param name="fileDownloadName">CSV file name</param>
        /// <param name="contentType">Http response content type</param>
        public BaseFileResult(IEnumerable<TEntity> source, string fileDownloadName, string contentType)
            : base(contentType)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            this._dataSource = source;

            if (string.IsNullOrEmpty(fileDownloadName))
                throw new ArgumentNullException("fileDownloadName");
            this.FileDownloadName = fileDownloadName;

            this.BufferOutput = true;
        }

        /// <summary>
        /// Creats new instance of BaseCsvFileResult{TEntity}
        /// </summary>
        /// <param name="source">List of data to be transformed to csv</param>
        /// <param name="fileDownloadName">CSV file name</param>
        /// <param name="map">Custom transformation delegate</param>
        /// <param name="headers">Columns headers</param>
        /// <param name="contentType">Http response content type</param>
        public BaseFileResult(IEnumerable<TEntity> source, string fileDownloadName, Func<TEntity, IEnumerable<string>> map, IEnumerable<string> headers, string contentType)
            : this(source, fileDownloadName, contentType)
        {
            this._headers = headers;
            this._map = map;
        }

        #endregion
    }
}
