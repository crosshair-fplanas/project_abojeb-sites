using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace AbojebApi.Utilities.Export
{
    public class XlsFileResult<TEntity> : BaseFileResult<TEntity> where TEntity : class
    {
        #region Fields

        private const string _contentType = "application/vnd.ms-excel";
        private string _tempPath;
        private string _tableName;

        #endregion

        #region Properties

        public string TableName
        {
            get
            {

                if (string.IsNullOrEmpty(_tableName))
                {
                    _tableName = typeof(TEntity).Name;
                }

                _tableName = _tableName.Trim().Replace(" ", "_");
                if (_tableName.Length > 30)
                {
                    _tableName = _tableName.Substring(0, 30);
                }

                return _tableName;
            }
            set { _tableName = value; }
        }

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
        /// Creats new instance of CsvFileResult{TEntity}
        /// </summary>
        /// <param name="source">List of data to be transformed to csv</param>
        /// <param name="fileDownloadName">CSV file name</param>
        public XlsFileResult(IEnumerable<TEntity> source, string fileDownloadName)
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
        public XlsFileResult(IEnumerable<TEntity> source, Func<TEntity, IEnumerable<string>> map, IEnumerable<string> headers, string fileDownloadName)
            : base(source, fileDownloadName, map, headers, _contentType)
        {

        }

        #endregion

        #region local routines

        protected override void WriteFile(HttpResponseBase response)
        {
            response.ContentEncoding = this.ContentEncoding;
            response.BufferOutput = this.BufferOutput;

            if (HasPreamble)
            {
                var preamble = this.ContentEncoding.GetPreamble();
                response.OutputStream.Write(preamble, 0, preamble.Length);
            }

            this.RenderResponse(response);
        }

        private void RenderResponse(HttpResponseBase response)
        {
            if (File.Exists(this.TempPath))
            {
                File.Delete(this.TempPath);
            }
            string sexcelconnectionstring = GetConnectionString(this.TempPath);
            using (System.Data.OleDb.OleDbConnection oledbconn = new System.Data.OleDb.OleDbConnection(sexcelconnectionstring))
            {
                oledbconn.Open();
                this.createTable(oledbconn);
                this.InsertData(oledbconn);
            }

            //var streambuffer = this.ContentEncoding.GetBytes(File.ReadAllText(this.TempPath));
            var streambuffer = File.ReadAllBytes(this.TempPath);

            response.OutputStream.Write(streambuffer, 0, streambuffer.Length);

            if (File.Exists(this.TempPath))
            {
                File.Delete(this.TempPath);
            }
        }

        private IEnumerable<string> GetEntityValues(TEntity obj)
        {
            IEnumerable<string> ds = null;
            if (this.Map != null)
            {
                ds = this.Map(obj);
            }
            else
            {
                ds = this.SourceProperties.Select(x => this.GetPropertyValue(x, obj));

            }

            return ds;
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

        private string GetConnectionString(string FileName, bool hasHeaders = true)
        {
            string HDR = hasHeaders ? "Yes" : "No";
            return Path.GetExtension(FileName).Equals(".xlsx") ?
                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"" :
                "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
        }

        private void createTable(OleDbConnection con)
        {
            string tyed = string.Join(",", this.Headers.Select(x => x + " " + "VARCHAR"));
            string commandText = string.Format("CREATE TABLE [{0}]({1});", this.TableName, tyed);
            OleDbCommand oledbcmd = new OleDbCommand(commandText, con);
            oledbcmd.ExecuteNonQuery();
        }

        private void InsertData(OleDbConnection con)
        {
            OleDbDataAdapter oleAdap = new OleDbDataAdapter("SELECT * FROM [" + this.TableName + "]", con);
            OleDbCommandBuilder oleCmdBuilder = new OleDbCommandBuilder(oleAdap);
            oleCmdBuilder.QuotePrefix = "[";
            oleCmdBuilder.QuoteSuffix = "]";
            OleDbCommand cmd = oleCmdBuilder.GetInsertCommand();
            foreach (TEntity row in this.DataSource)
            {
                var pVals = GetEntityValues(row);
                int index = 0;
                foreach (OleDbParameter param in cmd.Parameters)
                {
                    param.Value = pVals.ElementAt(index);
                    index++;
                }

                cmd.ExecuteNonQuery();
            }
        }

        private void InsertDataQuery(OleDbConnection cn)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Length = 0;
            sbSql.Insert(0, "INSERT INTO [" + this.TableName + "]");
            sbSql.Append(" (");
            sbSql.Append(string.Join(",", this.Headers));
            sbSql.Append(")");
            sbSql.Append(string.Join(" UNION ALL ", this.DataSource.Select(x => "  SELECT  " + string.Join(",", GetEntityValues(x)) + " ")));
            sbSql.Append(";");
            OleDbCommand cmd = new OleDbCommand(sbSql.ToString(), cn);
            cmd.ExecuteNonQuery();
        }

        #endregion
    }
}
