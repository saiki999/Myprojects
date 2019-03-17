using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieRentalAPI.Controllers;
using MovieRentalAPI;
using System.Net.Http;
using UserDataAccess;
using System.Net;

namespace TestAPI
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void GetUserTest()
        {
            var controller = new UsersController();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            var response = controller.Get(9999);

            User user;
            Assert.IsTrue(response.TryGetContentValue<User>(out user));
            Assert.AreEqual(user.Id, 9999);

        }

        [TestMethod]
        public void PostUserTest()
        {
            var controller = new UsersController();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            var path = "http://test/user/";
            controller.Request = new HttpRequestMessage { RequestUri = new Uri(path) };

            User user = new User();
            user.FName = "Steve";
            user.LName = "Jobs";
            user.Password = "stv123";
            user.ConfirmPassword = "stv123";
            user.EmailId = "stv@gmail.com";
            user.Age = 55;
            user.Phone = 342567897;
            user.Address = "Seattle";
            user.Type = "User";
            var response=controller.Post(user);
           
            Assert.AreEqual(response.Headers.Location, path + user.Id.ToString());

            //var response = controller.Get(9999);

            //Assert.AreEqual(9999, response.)

        }

        [TestMethod]
        public void PutUserTest()
        {
            var controller = new UsersController();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            var path = "http://test/user/";
            controller.Request = new HttpRequestMessage { RequestUri = new Uri(path) };

            User user = new User();
            user.FName = "Steve";
            user.LName = "Jobs";
            user.Password = "stv123";
            user.ConfirmPassword = "stv123";
            user.EmailId = "stv@gmail.com";
            user.Age = 55;
            user.Phone = 342567897;
            user.Address = "India"; //changed field
            user.Type = "User";
            var response = controller.Put(12221, user);
            User responseUser;
            response.TryGetContentValue<User>(out responseUser);

            Assert.AreEqual(user.Address, responseUser.Address);

            //var response = controller.Get(9999);

            //Assert.AreEqual(9999, response.)

        }

        [TestMethod]
        public void DeleteUserTest()
        {
            var controller = new UsersController();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            var response = controller.Delete(7777);
            
            User user = new User();

            //Assert.AreEqual(user.Id, 12221);
          
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK );


            // var response = controller.Get(9999);

            //Assert.AreEqual(9999, response.)

        }
    }
}
