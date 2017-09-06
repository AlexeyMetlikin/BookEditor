using Microsoft.VisualStudio.TestTools.UnitTesting;
using BooksEditor.Models.ViewModels;
using BooksEditor.Models.Entities;
using BooksEditor.Models.Abstract;
using Moq;
using System.Linq;
using System.Collections.Generic;
using BooksEditor.Controllers;
using System.Web;
using System.IO;
using NSubstitute;
using System.Web.Mvc;

namespace BooksEditor.Tests
{
    [TestClass]
    public class BooksCoverTests
    {
        [TestMethod]
        public void Can_Upload_Image_Data()
        {
            // Arrange
            // Создаем поток для чтения файла
            string filePath = Path.GetFullPath("..\\..\\testFiles\\testImage.jpg");
            FileStream fileStream = new FileStream(filePath, FileMode.Open);

            Mock<HttpPostedFileBase> image = new Mock<HttpPostedFileBase>();

            image.Setup(file => file.ContentLength)
                 .Returns(25000);

            image.Setup(file => file.FileName)
                 .Returns("testImage.jpg");

            image.Setup(file => file.ContentType)
                 .Returns("image/jpeg");

            image.Setup(file => file.InputStream)
                 .Returns(fileStream);

            //Создаем HttpContext для контроллера
            var mockHttpContext = Substitute.For<HttpContextBase>();

            // создаем контроллер и инициализируем контекст для него
            BookController controller = new BookController(null);
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = mockHttpContext
            };

            // Act - Создаем экземпляр контроллера
            controller.UploadImage(image.Object);

            //Assert
            //Проверяем на null Session["removeImage"]
            Assert.IsNull(controller.Session["removeImage"]);
            //проверяем равенство путей файла и MIME-типов
            Assert.AreEqual(image.Object.ContentType.ToString(), controller.Session["imageMimeType"].ToString());
            Assert.AreEqual(image.Object.FileName, (string)controller.Session["imageName"]);

            fileStream.Close();
        }

        [TestMethod]
        public void Not_Upload_Null_Image_Data()
        {
            // Arrange
            //Создаем HttpContext для контроллера
            var mockHttpContext = Substitute.For<HttpContextBase>();

            // создаем контроллер и инициализируем контекст для него
            BookController controller = new BookController(null);
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = mockHttpContext
            };

            // Act - Создаем экземпляр контроллера
            controller.UploadImage(null);

            //Assert
            Assert.AreEqual(true, (bool)controller.Session["removeImage"]);
            Assert.IsNull(controller.Session["imageData"]);
            Assert.IsNull(controller.Session["imageMimeType"]);
        }
    }
}
