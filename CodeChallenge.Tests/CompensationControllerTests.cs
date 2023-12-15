
using System.Net;
using System.Net.Http;
using System.Text;
using CodeChallenge.DTOs;
using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var compensation = new CompensationAddDTO()
            {
                EmployeeId = employeeId,
                Salary = 140000,
                EffectiveDate = new System.DateTime(2020, 1, 1)
            };
            var requestContent = new JsonSerialization().ToJson(compensation);

            var expectedFirstName = "John";
            var expectedLastName = "Lennon";
            var expectedSalary = 140000;
            var expectedEffectiveDate = new System.DateTime(2020, 1, 1);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(newCompensation.Employee);
            Assert.AreEqual(expectedFirstName, newCompensation.Employee.FirstName);
            Assert.AreEqual(expectedLastName, newCompensation.Employee.LastName);
            Assert.AreEqual(expectedSalary, newCompensation.Salary);
            Assert.AreEqual(expectedEffectiveDate, newCompensation.EffectiveDate);
        }

        [TestMethod]
        public void CreateCompensation_Returns_NotFound()
        {
            // Arrange
            var employeeId = "invalid_id";
            var compensation = new CompensationAddDTO()
            {
                EmployeeId = employeeId,
                Salary = 140000,
                EffectiveDate = new System.DateTime(2020, 1, 1)
            };
            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void GetCompensationByEmployeeId_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";
            var expectedSalary = 140000;
            var expectedEffectiveDate = new System.DateTime(2020, 1, 1);

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/employee/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var compensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(compensation.Employee);
            Assert.AreEqual(expectedFirstName, compensation.Employee.FirstName);
            Assert.AreEqual(expectedLastName, compensation.Employee.LastName);
            Assert.AreEqual(expectedSalary, compensation.Salary);
            Assert.AreEqual(expectedEffectiveDate, compensation.EffectiveDate);
        }

        [TestMethod]
        public void GetCompensationByEmployeeId_Returns_NotFound()
        {
            // Arrange
            var employeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/employee/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
