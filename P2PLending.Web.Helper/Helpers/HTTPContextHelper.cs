using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.Helper.Helpers
{
    public static class HTTPContextHelper
    {
        public static string GetBaseURL(HttpContext httpContext)
        {
            string baseURL = "";

            baseURL = $@"{httpContext.Request.Scheme}://{httpContext.Request.Host}";

            if (!string.IsNullOrEmpty(httpContext.Request.PathBase))
            {
                baseURL += $"{httpContext.Request.PathBase}";
            }
            
            return baseURL;
        }
    }
}
