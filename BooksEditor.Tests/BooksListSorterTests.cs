using Microsoft.VisualStudio.TestTools.UnitTesting;
using BooksEditor.Models.ViewModels;
using BooksEditor.Models.Entities;
using BooksEditor.Models.Abstract;
using Moq;
using System.Linq;
using System.Collections.Generic;
using BooksEditor.Controllers;
using System.Web;
using NSubstitute;
using System.Web.Mvc;

namespace BooksEditor.Tests
{
    [TestClass]
    public class BooksListSorterTests
    {
        [TestMethod]
        public void Can_Show_Sorted_YearAsc_List()
        {
            //Arrange
            //создаем cookie и присваиваем параметру pageSize значение 2
            var cookies = new HttpCookieCollection();
            cookies.Add(new HttpCookie("pageSize", "3"));

            //Создаем HttpContext и присваиваем cookies для Request и Response
            var mockHttpContext = Substitute.For<HttpContextBase>();
            mockHttpContext.Request.Cookies.Returns(cookies);
            mockHttpContext.Response.Cookies.Returns(cookies);

            // создаем Mock-контейнер и добавляем книги
            Mock<IBookContainer> mock = new Mock<IBookContainer>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { Title = "T1", PublishYear = 1959 },
                new Book { Title = "T2", PublishYear = 1971  },
                new Book { Title = "T3", PublishYear = 1985  },
                new Book { Title = "T4", PublishYear = 1976  },
                new Book { Title = "T5", PublishYear = 1951  }
            }.AsQueryable());
          
            //Создаем экземпляр контроллера
            BookController controller = new BookController(mock.Object);

            //Эмулируем httpContext для контроллера
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = mockHttpContext
            };
            int pageNum = 2;

            //Act - вызываем сортировк
            //в качестве количества книг на странице передаем null - значение будет браться из cookie (по умолчанию равно 12, если не найдено в cookie) 
            BookViewModel bookViewModel = (BookViewModel)controller.BooksList(pageNum, null, "Year", "Asc").Model;
            bookViewModel.SortBooks();
            Book[] books = bookViewModel.Books.ToArray();

            //Assert
            Assert.AreEqual(books.Count(), 2);
            Assert.AreEqual(books[0].Title, "T4");
            Assert.AreEqual(books[1].Title, "T3");
        }
        [TestMethod]
        public void Can_Show_Sorted_YearDesc_List()
        {
            //Arrange
            //создаем cookie и присваиваем параметру pageSize значение 2
            var cookies = new HttpCookieCollection();
            cookies.Add(new HttpCookie("pageSize", "2"));

            //Создаем HttpContext и присваиваем cookies для Request и Response
            var mockHttpContext = Substitute.For<HttpContextBase>();
            mockHttpContext.Request.Cookies.Returns(cookies);
            mockHttpContext.Response.Cookies.Returns(cookies);

            // создаем Mock-контейнер и добавляем книги
            Mock<IBookContainer> mock = new Mock<IBookContainer>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { Title = "T1", PublishYear = 1959 },
                new Book { Title = "T2", PublishYear = 1971  },
                new Book { Title = "T3", PublishYear = 1985  },
                new Book { Title = "T4", PublishYear = 1976  },
                new Book { Title = "T5", PublishYear = 1951  }
            }.AsQueryable());

            //Создаем экземпляр контроллера
            BookController controller = new BookController(mock.Object);
            int pageSize = 4;

            //Эмулируем httpContext для контроллера
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = mockHttpContext
            };

            //Act - вызываем сортировку 
            //В качестве страницы передаем null - значение будет браться из cookie (по умолчанию равно 1, если не найдено в cookie) 
            BookViewModel bookViewModel = (BookViewModel)controller.BooksList(null, pageSize, "Year", "Desc").Model;
            bookViewModel.SortBooks();
            Book[] books = bookViewModel.Books.ToArray();

            //Assert
            Assert.AreEqual(books.Count(), 4);
            Assert.AreEqual(books[0].Title, "T3");
            Assert.AreEqual(books[1].Title, "T4");
            Assert.AreEqual(books[2].Title, "T2");
            Assert.AreEqual(books[3].Title, "T1");
        }

        [TestMethod]
        public void Can_Show_Sorted_TitleAsc_List()
        {
            //Arrange
            //создаем cookie
            var cookies = new HttpCookieCollection();
            cookies.Add(new HttpCookie("pageSize", "2"));
            cookies.Add(new HttpCookie("pageNum", "3"));

            //Создаем HttpContext и присваиваем cookies для Request и Response
            var mockHttpContext = Substitute.For<HttpContextBase>();
            mockHttpContext.Request.Cookies.Returns(cookies);
            mockHttpContext.Response.Cookies.Returns(cookies);

            // создаем Mock-контейнер и добавляем книги
            Mock<IBookContainer> mock = new Mock<IBookContainer>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { Title = "Apple", PublishYear = 1959 },
                new Book { Title = "App", PublishYear = 1971  },
                new Book { Title = "Wars", PublishYear = 1985  },
                new Book { Title = "Soccer", PublishYear = 1976  },
                new Book { Title = "Maps", PublishYear = 1951  },
                new Book { Title = "Hockey", PublishYear = 1951  }
            }.AsQueryable());

            //Создаем экземпляр контроллера
            BookController controller = new BookController(mock.Object);
            int pageNum = 1;
            int pagesize = 5;

            //Эмулируем httpContext для контроллера
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = mockHttpContext
            };

            //Act - вызываем сортировку
            BookViewModel bookViewModel = (BookViewModel)controller.BooksList(pageNum, pagesize, "Title", "Asc").Model;
            bookViewModel.SortBooks();
            Book[] books = bookViewModel.Books.ToArray();

            //Assert
            Assert.AreEqual(books.Count(), 5);
            Assert.AreEqual(books[0].Title, "App");
            Assert.AreEqual(books[1].Title, "Apple");
            Assert.AreEqual(books[2].Title, "Hockey");
            Assert.AreEqual(books[3].Title, "Maps");
            Assert.AreEqual(books[4].Title, "Soccer");
        }
        [TestMethod]
        public void Can_Show_Sorted_TitleDesc_List()
        {
            //Arrange
            //создаем cookie
            var cookies = new HttpCookieCollection();

            //Создаем HttpContext и присваиваем cookies для Request и Response
            var mockHttpContext = Substitute.For<HttpContextBase>();
            mockHttpContext.Request.Cookies.Returns(cookies);
            mockHttpContext.Response.Cookies.Returns(cookies);

            // создаем Mock-контейнер и добавляем книги
            Mock<IBookContainer> mock = new Mock<IBookContainer>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { Title = "Медный всадник", PublishYear = 1998 },
                new Book { Title = "Преступление и наказание", PublishYear = 1987  },
                new Book { Title = "Атлант расправил плечи", PublishYear = 2010  },
                new Book { Title = "Таис Афинская", PublishYear = 1976  },
                new Book { Title = "Всадник без головы", PublishYear = 1964  }
            }.AsQueryable());

            //Создаем экземпляр контроллера
            BookController controller = new BookController(mock.Object);
            int pageSize = 3;
            int pageNum = 2;

            //Эмулируем httpContext для контроллера
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = mockHttpContext
            };            

            //Act - вызываем сортировку
            BookViewModel bookViewModel = (BookViewModel)controller.BooksList(pageNum, pageSize, "Title", "Desc").Model;
            bookViewModel.SortBooks();
            Book[] books = bookViewModel.Books.ToArray();

            //Assert
            Assert.AreEqual(books.Count(), 2);
            Assert.AreEqual(books[0].Title, "Всадник без головы");
            Assert.AreEqual(books[1].Title, "Атлант расправил плечи");
        }

        [TestMethod]
        public void Save_State_After_Page_Updating()
        {
            //Arrange
            //создаем cookie и при
            var cookies = new HttpCookieCollection();
            cookies.Add(new HttpCookie("pageSize", "2"));
            cookies.Add(new HttpCookie("pageNum", "2"));
            cookies.Add(new HttpCookie("order", "Year"));
            cookies.Add(new HttpCookie("orderType", "Desc"));

            //Создаем HttpContext и присваиваем cookies для Request и Response
            var mockHttpContext = Substitute.For<HttpContextBase>();
            mockHttpContext.Request.Cookies.Returns(cookies);
            mockHttpContext.Response.Cookies.Returns(cookies);

            // создаем Mock-контейнер и добавляем книги
            Mock<IBookContainer> mock = new Mock<IBookContainer>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { Title = "Медный всадник", PublishYear = 1998 },
                new Book { Title = "Преступление и наказание", PublishYear = 1987  },
                new Book { Title = "Атлант расправил плечи", PublishYear = 2010  },
                new Book { Title = "Таис Афинская", PublishYear = 1976  },
                new Book { Title = "Всадник без головы", PublishYear = 1964  }
            }.AsQueryable());

            //Создаем экземпляр контроллера
            BookController controller = new BookController(mock.Object);

            //Эмулируем httpContext для контроллера
            controller.ControllerContext = new ControllerContext
            {
                Controller = controller,
                HttpContext = mockHttpContext
            };

            //Act - вызываем Index
            BookViewModel bookViewModel = (BookViewModel)controller.Index().Model;
            bookViewModel.SortBooks();
            Book[] books = bookViewModel.Books.ToArray();

            //Assert
            Assert.AreEqual(books.Count(), 2);
            Assert.AreEqual(books[0].Title, "Преступление и наказание");
            Assert.AreEqual(books[1].Title, "Таис Афинская");
        }
    }
}
