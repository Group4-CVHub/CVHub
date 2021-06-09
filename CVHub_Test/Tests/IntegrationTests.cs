using CVHub.Data;
using CVHub.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CVHub_Test.Tests
{
    public class IntegrationTests : IClassFixture<TestFixture<CvHub.Startup>>
    {
        private readonly HttpClient Client;

        private readonly ApplicationDbContext _db;

        public IntegrationTests(TestFixture<CvHub.Startup> fixture, ApplicationDbContext db)
        {
            Client = fixture.Client;
            _db = db;
        }

        [Fact, AllowAnonymous]
        public async Task Post_Cv_Create1_Should_Return_OK_And_New_Cv_With_Correct_User()
        {
            //Arrange
            var testCvTemp = new CvTemp
            {
                AboutMe = "test2",
                Title = "test2",
                User = _db.Users.Find(1),
                Template = _db.Templates.Find(1),
                TemplateId = 1
            };
            var testEdu = new Education
            {
                EducationStart = DateTime.Now,
                EducationStop = DateTime.Now,
                Name = "test2",
                Degree = "test2"
            };
            var testWork = new Work
            {
                Description = "test2",
                Name = "test2",
                WorkStart = DateTime.Now,
                WorkStop = DateTime.Now
            };
            List<Education> educations = new();
            List<Work> workPlaces = new();
            educations.Add(testEdu);
            workPlaces.Add(testWork);
            testCvTemp.Educations = educations;
            testCvTemp.WorkPlaces = workPlaces;
            var request = new
            {
                Url = "/Cv/Create1",
                Body = new
                {
                    testCvTemp
                }
            };

            Cv testCv = new() { AboutMe = testCvTemp.AboutMe, Educations = testCvTemp.Educations, WorkPlaces = testCvTemp.WorkPlaces, TemplateId = testCvTemp.TemplateId, Title = testCvTemp.Title, User = testCvTemp.User, Template = testCvTemp.Template };

            //Act
            var response = await Client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));

            //Assert
            response.IsSuccessStatusCode.Equals(true);
            Assert.Equal(_db.Cvs.Where(c => c.AboutMe == "test" && c.Title == "test").FirstOrDefault().User, testCv.User);

        }

        [Fact]
        public async Task Get_Cv_By_Id_Should_Return_Code_302()
        {
            // Arrange
            var request = "/Cv/Get?id=1";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.StatusCode.Equals(302);
        }

        [Fact]
        public async Task Get_DeleteCv_By_Id_Should_Return_Code_302()
        {
            // Arrange
            var request = "/Cv/DeleteCv?id=1";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.StatusCode.Equals(302);
        }

        [Fact]
        public async Task Get_Template_Template1_Should_Return_OK()
        {
            // Arrange
            var request = "/Template/Template1";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.IsSuccessStatusCode.Equals(true);
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

            // Assert
            response.IsSuccessStatusCode.Equals(false);
        }

        [Fact]
        public async Task Get_Template_Index_Should_Return_OK()
        {
            // Arrange
            var request = "/Template/Index";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.IsSuccessStatusCode.Equals(true);
        }

        [Fact]
        public async Task Get_Template_TemplateForm1_Should_Return_OK()
        {
            // Arrange
            var request = "/Template/TemplateForm1";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.IsSuccessStatusCode.Equals(true);
        }

        [Fact]
        public async Task Get_TestUser_With_Id_1_Should_Return_Code_302()
        {
            // Arrange
            var request = "/User/Update?id=1";

            // Act
            var response = await Client.GetAsync(request);

            // Assert
            response.StatusCode.Equals(302);

        }
    }
}

