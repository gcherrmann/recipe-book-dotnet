using CommonTestUtilities.Requests;
using RecipeBook.Communication.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Json;
using FluentAssertions;

namespace WebApi.Test.Login.DoLogin
{
    public class DoLoginTest: IClassFixture<CustomWebApplicationFactory>
    {
        private readonly string method = "login";

        private readonly HttpClient _client;

        private readonly string _email;
        private readonly string _password;
        private readonly string _name;


        public DoLoginTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _email = factory.GetEmail();
            _password = factory.GetPassword();
            _name = factory.GetName();
        }

        [Fact]
        public async Task Success()
        {
            var request = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

           
            var response = await _client.PostAsJsonAsync($"api/{method}", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("name").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_name);
            responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().Should().NotBeNullOrEmpty();
        }
    }
}
