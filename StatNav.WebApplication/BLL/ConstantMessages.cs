using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatNav.WebApplication.BLL
{   
    public static class ConstantMessages
    {
        public static string Load = "{page} Loaded Successfully .";
        public static string Create = "{page} created Successfully";
        public static string Update = "{page} updated Successfully.";
        public static string Delete = "{page} deleted Successfully .";
        public static string Duplicate = "{page} details Already Exists.";
        public static string Error = "Error Occurred in While processing your requst.";
        public static string SignUpExists = "{page} already exists in Singup.";
        public static string LoadFailure = "Failed to retrieve the {page}-{0}.";
        public static string CreateFailure = "Failed to create {page} -{0}";
        public static string UpdateFailure = "Failed to update {page}-{0}";
        public static string DeleteFailure = "Failed to delete {page}-{0}";
        public static string ExistsFailure = "{page} not exists -{0}";
        public static string NoRecordsFound = "No records found in {page}";
        public static string SelfDelete = "You cannot delete yourself";
        public static string SignUpNotExists = "{page} not exists in SignUp";
    }
}
