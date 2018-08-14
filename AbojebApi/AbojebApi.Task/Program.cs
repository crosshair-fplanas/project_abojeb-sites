using AbojebApi.Data.Services;

namespace AbojebApi.Task
{
    class Program
    {
        static void Main(string[] args)
        {
            ApiService apiSvc = new ApiService();
            apiSvc.GetAllVesselReports();
        }
    }
}
