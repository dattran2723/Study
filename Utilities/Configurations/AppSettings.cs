using System.Web.Configuration;

namespace Utilities.Configurations
{
    public static class AppSettings
    {        
        public static string ConnectString
        {
            get { return WebConfigurationManager.AppSettings["ConnectionStrings"]; }
        }
    }
}
