using System.Collections.Generic;
using WaiveApp.Models;


namespace WaiveApp
{
    public static class Constants
    {
        // URL of REST service
        //public static string RestUrl = "https://YOURPROJECT.azurewebsites.net:8081/api/todoitems/{0}";

        // URL of REST service (Android does not use localhost)
        // Use http cleartext for local deployment. Change to https for production
        public static string RestUrl = "https://waiveapi.azure-api.net/api/RenewableShareData/get/";
        public static string location_name_global = ""; 
        public static double renewability_border_global = 0.0;
        public static List<RenewableShareData> items_global = new List<RenewableShareData>();
    }
}

    