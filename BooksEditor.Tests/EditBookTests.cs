using Microsoft.VisualStudio.TestTools.UnitTesting;
using BooksEditor.Models.ViewModels;
using BooksEditor.Models.Entities;
using BooksEditor.Models.Abstract;
using Moq;
using System.Linq;
using System.Collections.Generic;
using BooksEditor.Controllers;
using System.Web;
using System.Web.SessionState;
using NSubstitute;
using System.Web.Mvc;
using System.Web.Routing;

namespace BooksEditor.Tests
{
    [TestClass]
    public class EditBookTests
    {
        [TestMethod]
        public void Can_Delete_Valid_Book()
        {
            //Arrange
            //создаем cookie
            var cookies = new HttpCookieCollection();

            //Создаем HttpContext и присваиваем cookies для Request и Response
            var mockHttpContext = Substitute.For<HttpContextBase>();
            mockHttpContext.Request.Cookies.Returns(cookies);
            mockHttpContext.Response.Cookies.Returns(cookies);

            // создаем Mock-контейнер
            Mock<IBookContainer> mock = new Mock<IBookContainer>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Title = "T1", PublishYear = 1959 },
                new Book { BookId = 2, Title = "T2", PublishYear = 1971  },
                new Book { BookId = 3, Title = "T3", PublishYear = 1985  },
                new Book { BookId = 4, Title = "T4", PublishYear = 1976  },
                new Book { BookId = 5, Title = "T5", PublishYear = 1951  }
            }.AsQueryable());

            //Создаем экземпляр контроллера и присваиваем HttpContext
            BookController controller = new BookController(mock.Object);
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = mockHttpContext
            };

            //Act - Берем из mock книгу с bookId = 1
            Book book = mock.Object.Books.FirstOrDefault(b => b.BookId == 1);

            //Act - Удаляем книгу
            controller.DeleteBook(book.BookId);

            //Проверяем, что для book 1 раз вызывался метод DeleteBook
            mock.Verify(books => books.DeleteBook(book.BookId), Times.Once());
        }

        [TestMethod]
        public void Can_Edit_Book()
        {
            // Arrange - создаем Mock-контейнер
            Mock<IBookContainer> mock = new Mock<IBookContainer>();
            mock.Setup(book => book.Books).Returns(new List<Book> {
                new Book {BookId = 1, Title = "P1"},
                new Book {BookId = 2, Title = "P2"},
                new Book {BookId = 3, Title = "P3"},
            }.AsQueryable());

            //Создаем экземпляр контроллера
            BookController controller = new BookController(mock.Object);

            // Act
            Book book1 = (Book)controller.EditBook(1).Model;
            Book book2 = (Book)controller.EditBook(2).Model;
            Book book3 = (Book)controller.EditBook(3).Model;

            // Assert
            Assert.AreEqual(1, book1.BookId);
            Assert.AreEqual(2, book2.BookId);
            Assert.AreEqual(3, book3.BookId);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Book()
        {
            // Arrange - создаем Mock-контейнер
            Mock<IBookContainer> mock = new Mock<IBookContainer>();
            mock.Setup(book => book.Books).Returns(new List<Book> {
                new Book {BookId = 1, Title = "P1"},
                new Book {BookId = 2, Title = "P2"},
                new Book {BookId = 3, Title = "P3"},
            }.AsQueryable());

            //Создаем экземпляр контроллера
            BookController controller = new BookController(mock.Object);

            // Act 
            var bookResult = controller.EditBook(4);

            // Assert 
            Assert.IsNull(bookResult);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            //создаем cookie
            var cookies = new HttpCookieCollection();

            //Создаем HttpContext и присваиваем cookies для Request и Response
            var mockHttpContext = Substitute.For<HttpContextBase>();
            mockHttpContext.Request.Cookies.Returns(cookies);
            mockHttpContext.Response.Cookies.Returns(cookies);

            //Определяем путь к виртуальному корневому каталогу
            mockHttpContext.Request.ApplicationPath.Returns("/"); 
            //Добавление пути файла к виртуальному пути (для использования UrlHelper'a)
            mockHttpContext.Response.ApplyAppPathModifier(It.IsAny<string>()).Returns("/mynewVirtualPath/");

            // Arrange - создаем Mock-контейнеры для книги и для автора
            Mock<IBookContainer> mock = new Mock<IBookContainer>();

            //Добавляем одного автора в mockAuthor
            Author author = new Author { AuthorId = 1, FirstName = "Иван", SecondName = "Иванов" };
            mock.Setup(auth => auth.Authors).Returns(new List<Author>
            {
                author
            }.AsQueryable());

            //Создаем экземпляр контроллера и присваиваем HttpContext
            BookController controller = new BookController(mock.Object);

            //Определяем контект контроллера
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = mockHttpContext
            };

            //Инициализируем Url
            controller.Url = new UrlHelper(new RequestContext(controller.HttpContext, new RouteData()));

            Book book = new Book { Title = "testBook", AuthorsList = "Иван Иванов, Дмитрий Петров"};

            // Act - вызываем Post-метод редактирования (сохранения) книги
            ActionResult result = controller.EditBook(book);

            // Assert
            //Проверяем количество вызванных SaveBook для книги
            mock.Verify(b => b.SaveBook(book), Times.Once());

            //Проверяем количество вызванных SaveNewAuthor для автора
            mock.Verify(auth => auth.SaveNewAuthor(It.IsAny<Author>()), Times.Exactly(2));

            // Проверяем тип результата
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            //Создаем HttpContext и присваиваем cookies для Request и Response
            var mockHttpContext = Substitute.For<HttpContextBase>();

            //Определяем путь к виртуальному корневому каталогу
            mockHttpContext.Request.ApplicationPath.Returns("/");
            //Добавление пути файла к виртуальному пути (для использования UrlHelper'a)
            mockHttpContext.Response.ApplyAppPathModifier(It.IsAny<string>()).Returns("/mynewVirtualPath/");

            // Arrange - создаем Mock-контейнеры для книги и для автора
            Mock<IBookContainer> mock = new Mock<IBookContainer>();

            //Создаем экземпляр контроллера и присваиваем HttpContext
            BookController controller = new BookController(mock.Object);

            //Определяем контект контроллера
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = mockHttpContext
            };

            //Инициализируем Url
            controller.Url = new UrlHelper(new RequestContext(controller.HttpContext, new RouteData()));

            // Создаем новую книгу
            Book book = new Book { Title = "testBook" };

            // Добавляем ошибку уровня модели
            controller.ModelState.AddModelError("Error", "Error in model");

            // Act - вызываем Post-метод редактирования (сохранения) книги
            ActionResult result = controller.EditBook(book);

            // Assert
            //Проверяем количество вызванных SaveBook для книги
            mock.Verify(b => b.SaveBook(It.IsAny<Book>()), Times.Never());

            //Проверяем количество вызванных SaveNewAuthor для автора
            mock.Verify(auth => auth.SaveNewAuthor(It.IsAny<Author>()), Times.Never());

            // Проверяем тип результата
            Assert.IsInstanceOfType(result, typeof(PartialViewResult));
        }
    }
}
