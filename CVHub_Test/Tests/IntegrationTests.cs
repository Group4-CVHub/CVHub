using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CVHub_Test.Tests
{
    public class IntegrationTests : IClassFixture<TestFixture<CvHub.Startup>>
    {
        private HttpClient Client;

        public IntegrationTests(TestFixture<CvHub.Startup> fixture)
        {
            Client = fixture.Client;
        }


        [Fact]
        public async Task Get_Cv_Index_Should_Return_OK()
        {
            // Arrange
            var request = "/Cv/Index";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_Cv_CvList_Should_Return_OK()
        {
            // Arrange
            var request = "/Cv/CvList";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_Cv_By_Id_Should_Return_OK()
        {
            // Arrange
            var request = "/Cv/Get?id=1";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_DeleteCv_By_Id_Should_Return_OK()
        {
            // Arrange
            var request = "/Cv/DeleteCv?id=1";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_TemplateForm1_Should_Return_OK()
        {
            // Arrange
            var request = "/Template/Template1";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_User_ChangePassword_Pass_Wrong_Password_Should_Return_NotOk()
        {
            // Arrange
            var request = new
            {
                Url = "/User/ChangePassword",
                Body = new
                {
                    UserId = 1,
                    OldPassWord = 1234567,
                    NewPassWord = 123456
                }
            };

            // Act
            var response = await Client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = await response.Content.ReadAsStringAsync();

            // Assert
            response.IsSuccessStatusCode.Equals(false);
        }
    }
}

