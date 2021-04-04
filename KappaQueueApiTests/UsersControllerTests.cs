using KappaQueue;
using KappaQueueCommon.Common.DTO;
using KappaQueueCommon.Models.Users;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Xunit;

namespace KappaQueueApiTests
{
    public class UsersControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string USERS_URL = "/api/Users";
        private string _admToken;
        private string _testToken;

        private readonly WebApplicationFactory<Startup> _factory;

        public UsersControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _admToken = Auth("admin","admin");
            NewUser();
            _testToken = Auth("test", "test");            
        }

        private string Auth(string username, string password)
        {
            return _factory.CreateClient().GetStringAsync($"/api/Auth?username={username}&password={password}").Result;            
        }

        private void NewUser()
        {
            UserAddDto user = new UserAddDto {
                FirstName = "Test",
                LastName = "Test",
                Password = "test",
                Username = "test",
            };

            string json = JsonSerializer.Serialize(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            using var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _admToken);
            using var response = client.PostAsync(USERS_URL, data).Result;
      /*      string resp = response.Content.ReadAsStringAsync().Result;
            string b = resp;*/
        }

        [Fact]
        public async void GetUsers_OkResult()
        {
            //����������
            var request = new HttpRequestMessage(new HttpMethod("GET"), USERS_URL);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _admToken);
            //��������
            using var client = _factory.CreateClient();
            using var response = await client.SendAsync(request);
            //��������
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void GetUsers_Unauthorized()
        {
            //����������
            var request = new HttpRequestMessage(new HttpMethod("GET"), USERS_URL);
            //��������
            using var client = _factory.CreateClient();
            using var response = await client.SendAsync(request);

            //��������
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async void GetUsers_Forbidden()
        {
            //����������
            var request = new HttpRequestMessage(new HttpMethod("GET"), USERS_URL);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _testToken);
            //��������
            using var client = _factory.CreateClient();
            using var response = await client.SendAsync(request);
            //��������
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async void GetUsers_Body()
        {
            //����������
            var request = new HttpRequestMessage(new HttpMethod("GET"), USERS_URL);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _admToken);
            //��������
            using var client = _factory.CreateClient();
            using var response = await client.SendAsync(request);
            string body = response.Content.ReadAsStringAsync().Result;
            List<User> users = JsonSerializer.Deserialize<List<User>>(body);
            //��������
            Assert.NotNull(users.FirstOrDefault(u => u.Id == 1));
        }

        [Theory]
        [InlineData(1)]
        public async void GetUser_OkResult(int id)
        {
            //����������
            var request = new HttpRequestMessage(new HttpMethod("GET"), USERS_URL + $"/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _admToken);
            //��������
            using var client = _factory.CreateClient();
            using var response = await client.SendAsync(request);
            //��������
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(101)]
        [InlineData(102)]
        public async void GetUser_NotFound(int id)
        {
            //����������
            var request = new HttpRequestMessage(new HttpMethod("GET"), USERS_URL + $"/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _admToken);
            //��������
            using var client = _factory.CreateClient();
            using var response = await client.SendAsync(request);
            //��������
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(101)]
        [InlineData(102)]
        public async void GetUser_Forbidden(int id)
        {
            //����������
            var request = new HttpRequestMessage(new HttpMethod("GET"), USERS_URL + $"/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _testToken);
            //��������
            using var client = _factory.CreateClient();
            using var response = await client.SendAsync(request);
            //��������
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("100")]
        [InlineData("101")]
        [InlineData("102")]
        [InlineData("admin")]
        [InlineData("test")]
        public async void GetUser_Unauthorized(string id)
        {
            //����������
            var request = new HttpRequestMessage(new HttpMethod("GET"), USERS_URL + $"/{id}");
            //��������
            using var client = _factory.CreateClient();
            using var response = await client.SendAsync(request);
            //��������
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        public async void GetUser_Body(int id)
        {
            //����������
            var request = new HttpRequestMessage(new HttpMethod("GET"), USERS_URL + $"/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _admToken);
            //��������
            using var client = _factory.CreateClient();
            using var response = await client.SendAsync(request);
            string body = response.Content.ReadAsStringAsync().Result;
            User user = JsonSerializer.Deserialize<User>(body);
            //��������
            Assert.NotNull(user);
            Assert.True(user.Id == 1);
        }
    }
}
