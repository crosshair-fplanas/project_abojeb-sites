using AbojebApi.Core.Data;
using AbojebApi.Core.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;

namespace AbojebApi.Data.Services
{
    public class ApiService
    {
        string baseUrl = "https://otis.stratumfive.com/rest/";
        string apiVersion = ConfigurationManager.AppSettings["ApiVersion"];

        /// <summary>
        /// Connects to and gets data from the API.
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <param name="url">API link where data will be retrieved</param>
        /// <returns></returns>
        private Log<T> GetFromApi<T>(string url) where T : class
        {
            string username = "Abojeb";
            string password = "eLKTYD8KaPJ2vP8M";
            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@url);
            request.Method = "GET";
            request.Headers["Authorization"] = "Basic " + credentials;
            HttpWebResponse response = null;
            string responseValue = "";

            try
            {
                response = request.GetResponse() as HttpWebResponse;
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }
                    }
                }

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Log<T>));
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(responseValue)))
                {
                    var results = (Log<T>)serializer.ReadObject(ms);
                    return results;
                }

                //JArray obj = (JArray)JObject.Parse(responseValue).SelectToken("obj");
                //var test = new JavaScriptSerializer().Deserialize<List<T>>(obj.ToString());
                //return new JavaScriptSerializer().Deserialize<Log<T>>(responseValue);
            }
            catch (Exception ex)
            {
                responseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                return new Log<T>();
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }
        }

        /// <summary>
        /// Gets IMO numbers from the API.
        /// </summary>
        /// <returns>List of IMO numbers</returns>
        private List<int> GetIMOlist()
        {
            string url = baseUrl + apiVersion + "/vessels";
            List<string> lstImos = GetFromApi<string>(url).obj;
            return lstImos.ConvertAll(int.Parse);
        }

        /// <summary>
        /// Gets MMSI numbers from the API.
        /// </summary>
        /// <returns>List of IMO numbers</returns>
        private List<int> GetMMSIlist()
        {
            string url = baseUrl + apiVersion + "/vesselsmmsi";
            List<string> lstMmsi = GetFromApi<string>(url).obj;
            return lstMmsi.ConvertAll(int.Parse);
        }

        /// <summary>
        /// Gets vessels by IMO list and saves them to database.
        /// </summary>
        /// <param name="IMOs"></param>
        /// <returns>List of vessels, each with latest report</returns>
        public List<VesselDto> GetVesselsByIMOs(List<int> IMOs)
        {
            var queryString = string.Join(",", IMOs);
            string url = baseUrl + apiVersion + "/vessels/" + queryString;
            var results = GetFromApi<VesselDto>(url).obj.Distinct().ToList();

            //another api link
            url = baseUrl + "api/Vessels?versioncode=" + apiVersion + "&IMOS=" + queryString;
            var otherVessels = GetFromApi<VesselDto>(url).obj.Distinct();

            var addtlResults = otherVessels.Where(s => !results.Any(a => a.IMO == s.IMO)).ToList();
            if (addtlResults.Count > 0)
            {
                results.AddRange(addtlResults);
            }

            UpdateData(results);

            return results;
        }

        /// <summary>
        /// Gets vessels by MMSI. Retrieved data is not yet saved to database.
        /// </summary>
        /// <param name="MMSIs"></param>
        /// <returns></returns>
        private List<VesselDto> GetVesselsByMMSI(List<int> MMSIs)
        {
            var queryString = string.Join(",", MMSIs);
            string url = baseUrl + apiVersion + "/vesselsbymmsi/" + queryString;
            var results = GetFromApi<VesselDto>(url).obj.Distinct().ToList();
            return results;
        }

        /// <summary>
        /// Gets vessels and corresponding reports by all available IMO and MMSI numbers.
        /// </summary>
        public void GetAllVesselReports()
        {
            var lstIMO = GetIMOlist();
            var lstMMSI = GetMMSIlist();

            var results = GetVesselsByIMOs(lstIMO);
            var MMSIvessels = GetVesselsByMMSI(lstMMSI);

            var addtlResults = MMSIvessels.Where(w => !results.Any(a => a.IMO == w.IMO)).ToList();
            if (addtlResults.Count > 0)
            {
                UpdateData(addtlResults);
            }
        }

        /// <summary>
        /// Gets reports related to a vessel. Retrieved data is not yet saved to database.
        /// </summary>
        /// <param name="IMO">IMO number of the vessel</param>
        /// <param name="MMSI">MMSI number of the vessel</param>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        private List<ReportDto> GetReportsByIds(int IMO, int MMSI)
        {
            string url = baseUrl + apiVersion + "/vessels/" + IMO + "/reports";
            var results = GetFromApi<ReportDto>(url).obj.Distinct().ToList();

            url = baseUrl + apiVersion + "/vessels/" + MMSI + "/reportsbymmsi";
            var otherReports = GetFromApi<ReportDto>(url).obj.Distinct();

            var addtlResults = otherReports.Where(w => !results.Any(a => a.ReportId == w.ReportId)).ToList();
            if (addtlResults.Count > 0)
            {
                results.AddRange(addtlResults);
            }

            return results;
        }

        /// <summary>
        /// Updates Vessel records
        /// </summary>
        /// <param name="vessels">List of vessels</param>
        private void UpdateData(List<VesselDto> vessels)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new ApplicationDbContext()))
                {
                    var newVessels = vessels.Where(w => !unitOfWork.Vessel.Collections
                            .Any(a => a.IMO == w.IMO))
                        .AsEnumerable()
                        .Select(s => s.Convert<VesselDto, Vessel>())
                        .ToList();
                    unitOfWork.Vessel.AddRange(newVessels);

                    //var lstVessels = vessels.Select(s => s.Convert<VesselDto, Vessel>()).ToList();
                    List<ReportDto> reports = new List<ReportDto>();

                    foreach (var vessel in vessels)
                    {
                        //if (unitOfWork.Vessel.GetById(vessel.IMO) == null)
                        //{
                        //    unitOfWork.Vessel.Add(vessel);
                        //}
                        //else
                        //{
                        //    unitOfWork.Vessel.Modify(vessel);
                        //}
                        reports.AddRange(GetReportsByIds(vessel.IMO, vessel.MMSI));
                    }

                    var newReports = reports.Where(w => !unitOfWork.Report.Collections
                            .Any(a => a.ReportId == w.ReportId && a.IMO == w.IMO))
                        .AsEnumerable()
                        .Select(s => s.Convert<ReportDto, Report>())
                        .ToList();
                    unitOfWork.Report.AddRange(newReports);

                    unitOfWork.Save();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
