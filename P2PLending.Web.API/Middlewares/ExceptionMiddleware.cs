using log4net.Core;
using Microsoft.AspNetCore.Http;
using P2PLending.Web.Entities;
using P2PLending.Web.Entities.DTO.ResultModel;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace P2PLending.Web.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(MessageException ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var errorDetail = new ErrorDetail
                {
                    Message = ex?.Message,
                    MessageCode = ex?.MessageCode,
                    StatusCode = httpContext.Response.StatusCode,
                    InnerException = JsonSerializer.Serialize(ex.InnerException)
                };
                await httpContext.Response.WriteAsync(errorDetail.ToString());
            }
            catch (Exception ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var errorDetail = new ErrorDetail
                {
                    Message = ex?.Message,
                    StatusCode = httpContext.Response.StatusCode,
                    InnerException = JsonSerializer.Serialize(ex.InnerException)
                };
                await httpContext.Response.WriteAsync(errorDetail.ToString());
            }
        }
    }
}
