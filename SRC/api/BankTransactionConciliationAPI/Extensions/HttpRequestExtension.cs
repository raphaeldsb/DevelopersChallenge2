using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;

namespace BankTransactionConciliationAPI.Extensions
{
    public static class HttpRequestExtension
    {
        public static string AsString(this HttpRequest request)
        {
            try
            {
                request.EnableBuffering();
                
                var result = "";
                request.Body.Position = 0;
                using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
                {
                    result = reader.ReadToEndAsync()
                        .GetAwaiter().GetResult();
                }
                request.Body.Position = 0;

                return result;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
