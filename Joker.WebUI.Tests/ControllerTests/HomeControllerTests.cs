using System.Web.Mvc;
using Joker.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Joker.WebUI.Tests.ControllerTests
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController _controller;

        [TestInitialize]
        public  void TestSetUp()
        {
            // Arrange 
            this._controller = new HomeController();    
        }

        [TestClass]
        public class TheIndexAction : HomeControllerTests
        {
            [TestMethod]
            public void WhenTheActionExecute_ReturnsViewNameIndex()
            {
                // Act
                var result = this._controller.Index() as ViewResult;

                // Assert
                Assert.AreEqual("", result.ViewName);
            }
        }
    }
}
