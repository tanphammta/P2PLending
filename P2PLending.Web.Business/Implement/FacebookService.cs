using Newtonsoft.Json;
using P2PLending.Web.Business.Interface;
using P2PLending.Web.Entities.Entities.Facebook;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace P2PLending.Web.Business.Implement
{
    public class FacebookService: IFacebookService
    {
        private readonly HttpClient _httpClient;

        public FacebookService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com/v11.0/")
            };
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<FacebookUserResource> GetUserFromFacebookAsync(string facebookToken)
        {
            var result = await GetAsync<dynamic>(facebookToken, "me", "fields=id,name,first_name,last_name,email,picture.width(100).height(100), link");
            if (result == null)
            {
                throw new Exception("User from this token not exist");
            }

            var account = new FacebookUserResource()
            {
                Id = result.id,
                Email = result.email,
                Name = result.name,
                FirstName = result.first_name,
                LastName = result.last_name,
                Link = result.link
                //Picture = result.picture.data.url
            };
            string picLink = result.picture.data.url;
            picLink.Replace("{", "");
            picLink.Replace("}", "");
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(picLink);
                var pictureData = Convert.ToBase64String(data);
                account.Picture = pictureData;
            }

            return account;
        }

        private async Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
