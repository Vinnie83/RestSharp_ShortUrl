using RestSharp;
using System.Net;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace RestSharpAPITests
{
    public class RestSharpAPI_Tests
    {
        private RestClient client;
        private const string baseUrl = "https://shorturl.velinski.repl.co/api";

        [SetUp]
        public void Setup()
        {
            this.client = new RestClient(baseUrl);
        }

        [Test]
        public void Test_ListAlShortUrls()
        {
            // Arrange
            var request = new RestRequest("urls", Method.Get);

            // Act
            var response = this.client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var urls = JsonSerializer.Deserialize<List<Urls>>(response.Content);
            Assert.That(urls, Is.Not.Empty);
        }

        [Test]
        public void Test_FindUrlsByShortCode()
        {
            // Arrange
            var request = new RestRequest("urls/nak", Method.Get);

            // Act
            var response = this.client.Execute(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var urls = JsonSerializer.Deserialize<Urls>(response.Content);
            Assert.That(urls.shortCode, Is.EqualTo("nak"));
            Assert.That(urls.url, Is.EqualTo("https://nakov.com"));

        }

        [Test]

        public void Test_FindInvalid_ShortCode()
        {
            var request = new RestRequest("urls/vinnie", Method.Get);

            var response = this.client.Execute(request);

            var errorMessage = JsonSerializer.Deserialize<Message>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(errorMessage.errMsg, Is.EqualTo("Short code not found: vinnie"));

           

        }

        [Test]

        public void Test_CreateShortUrl()
        {
            
            var request = new RestRequest("urls", Method.Post);
            var newurl = "url" + DateTime.Now.Ticks;
            var reqBody = new
            {
                url = $"https://{newurl}.com",
                shortCode = newurl
            };

            request.AddBody(reqBody);

            var response = this.client.Execute(request);

            var newUrl = JsonSerializer.Deserialize<NewUrl>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(newUrl.msg, Is.EqualTo("Short code added."));

        }

        [Test]

        public void Test_DuplicateShortUrl()
        {
            var request = new RestRequest("urls", Method.Post);
            var reqBody = new
            {
                url = "https://url638348976314808227.com",
                shortCode = "url638348976314808227"
            };

            request.AddBody(reqBody);

            var response = this.client.Execute(request);

            var newUrl = JsonSerializer.Deserialize<NewUrl>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        }
    }
}