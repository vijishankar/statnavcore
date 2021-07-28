using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatNav.WebApplication.Models
{
    public static class AppConfigValues
    {
        public static string ApiBaseUrl { get; set; }
        public static string ApiToken { get; set; }
        public static string ApiVersion { get; set; }
        public static string LogStorageContainer { get; set; }
        public static string StorageAccountKey { get; set; }
        public static string StorageAccountName { get; set; }
        public static string DataSource { get; set; }
        public static string UserId { get; set; }
        public static string InitialCatalog { get; set; }
        public static string Password { get; set; }
    }
}
