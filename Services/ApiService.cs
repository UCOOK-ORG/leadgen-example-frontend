using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LeadgenWebApi;

namespace LeadgenFrontend.Services
{
    public class ApiService
    {
        public readonly Client Client;

        public ApiService()
        {
            var httpClient = new HttpClient();
            Client = new Client("https://leadgen-silvertree.azurewebsites.net", httpClient);
        }
    }
}
