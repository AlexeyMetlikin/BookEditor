using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BooksEditor.Models.Abstract;
using BooksEditor.Models.Entities;
using BooksEditor.Models.ViewModels;
using BooksEditor.Models.Context;

namespace BooksEditor.Controllers
{
    public class BookController : Controller
    {
        // Интерфейсы для обращения к БД
        private IBookContainer _booksContainer;
        //private IAuthorContainer _authorsContainer;

        private EFDbContext db = new EFDbContext();

        public BookController(IBookContainer booksContainer)//, IAuthorContainer authorsContainer)
        {
            _booksContainer = booksContainer;
            //_authorsContainer = authorsContainer;
        }

        public ViewResult Index()
        {
            string str_pageSize = GetCookie("pageSize");    // берем из cookie количество книг на странице
            string str_pageNum = GetCookie("pageNum");    // берем из cookie номер страницы
            int? pageSize = null;
            int page = 1;
            if (str_pageSize != null)   //Если в cookie было значение - выставляем его
            {
                pageSize = int.Parse(str_pageSize);
            }
            if (str_pageNum != null)    //Если в cookie было значение - выставляем его
            {
                page = int.Parse(str_pageNum);
            }

            ViewBag.order = GetCookie("order");             // получаем сортировку из cookie
            ViewBag.orderType = GetCookie("orderType");     // получаем порядок сортировки из cookie

            string order = null;
            if (ViewBag.order != null && ViewBag.orderType != null) //Если в cookie было значение - выставляем его
            {
                order = ViewBag.order + ViewBag.orderType;
            }

            Pager pager = new Pager(_booksContainer.Books.Count(), page, pageSize);  // формируем paging 
            return View(new BookViewModel { Pager = pager, Books = _booksContainer.Books.ToList(), Order = order });
        }

        [HttpGet]
        public PartialViewResult BooksList(int? page, int? pageSize, string order, string orderType = "Asc")
        {
            //Если есть порядок сортировки и сортировка уже осуществляется по order
            if (GetCookie("order") == order)
            {
                orderType = GetCookie("orderType") == "Asc" ? "Desc" : "Asc";
            }
            //Если сортировка и порядок заполнены - кладем в cookie
            if (order != null && orderType != null)
            {
                SetCookie("order", order);
                SetCookie("orderType", orderType);
            }
            else    //Иначе пытаемся считать из cookie
            {
                order = GetCookie("order");
                orderType = GetCookie("orderType");
            }
            // кладем в cookie количество книг на странице
            if (pageSize != null)
            {
                SetCookie("pageSize", pageSize.ToString());
            }
            else    //иначе пытаемся получить размер страницы из cookie
            {
                string str_pageSize = GetCookie("pageSize");
                if (str_pageSize != null)
                {
                    pageSize = int.Parse(str_pageSize);
                }
            }
            // кладем в cookie номер страницы
            if (page != null)
            {
                SetCookie("pageNum", page.ToString());
            }
            else    //иначе пытаемся получить размер страницы из cookie
            {
                string str_pageNum = GetCookie("pageNum");
                if (str_pageNum != null)
                {
                    page = int.Parse(str_pageNum);
                }
            }

            ViewBag.order = order;
            ViewBag.orderType = orderType;
            Pager pager = new Pager(_booksContainer.Books.Count(), page, pageSize); // формируем paging 
            return PartialView("BooksList", new BookViewModel { Pager = pager, Books = _booksContainer.Books.ToList(), Order = order + orderType });
        }

        public PartialViewResult CreateBook()
        {
            return PartialView("EditBook", new Book());
        }

        [HttpGet]
        public PartialViewResult EditBook(int? book_id)
        {
            Book edit_book = _booksContainer.Books.FirstOrDefault(book => book.BookId == book_id);
            if (edit_book != null)
            {
                return PartialView("EditBook", _booksContainer.Books.Where(book => book.BookId == book_id).FirstOrDefault());
            }
            return null;
        }

        [HttpPost]
        public ActionResult EditBook(Book book)
        {
            FillBookCover(book);
            if (book.Cover != null && Session["imageMimeType"] != null && !((string)Session["imageMimeType"]).Contains("image"))  //Если тип - не image, добавляем ошибку уровня модели
            {
                ModelState.AddModelError("imageTypeError", "Выбранный файл не является изображением");
            }
            if (!string.IsNullOrEmpty(book.PublishYear.ToString()) && book.PublishYear > DateTime.Now.Year) //Если год больше текущего, добавляем ошибку уровня модели
            {
                ModelState.AddModelError("InvalidYear", "Год не может превышать текущий");
            }
            if (ModelState.IsValid)
            {
                foreach (string auth in book.AuthorsList.Split(','))    //Для каждого автора из списка авторов
                {
                    Author author = new Author();
                    author.FirstName = auth.Trim().Split(' ')[0];
                    author.SecondName = auth.Trim().Split(' ')[1];

                    //Если автор отсутствует в БД - добавляем
                    _booksContainer.SaveNewAuthor(author);
                    
                    //Берем первого автора в качестве основного
                    if (book.AuthorId == 0)                             
                    {
                        book.AuthorId = _booksContainer.Authors.FirstOrDefault(a => a.FirstName == author.FirstName && a.SecondName == author.SecondName).AuthorId;
                    }
                }
                //Сохраняем книгу
                _booksContainer.SaveBook(book);
                //Добавляем сообщение о результате обновления (добавления) книги
                if (book.BookId == 0)
                {
                    TempData["message"] = string.Format("Книга '{0}' была добавлена", book.Title);
                }
                else
                {
                    TempData["message"] = string.Format("Книга '{0}' была обновлена", book.Title);
                }
                Session["removeImage"] = null;
                return BooksList(null, null, null, null);
            }
            return PartialView("EditBook", book);
        }

        /* Заполнить изображение и его тип для книги */
        private void FillBookCover(Book book)
        {
            string fileName = (string)Session["imageName"];
            if (fileName != null)
            {
                book.Cover = fileName;
            }
            else if ((Session["removeImage"] != null && (bool)Session["removeImage"] == true) ||
                      book.Cover == Url.Content("~/Content/images/default/noCover.png"))
            {
                book.Cover = null;
            }
            Session["removeImage"] = null;
            Session["imageName"] = null;
        }

        //Добавление изображения на сервер
        [HttpPost]
        public void UploadImage(HttpPostedFileBase image)   
        {
            if (image != null && image.ContentLength > 0)
            {
                string filePath = CheckFileOnServer(Server.MapPath("~/Content/images/Covers/"), image.FileName);
                image.SaveAs(filePath);
                Session["imageName"] = image.FileName;
                Session["imageMimeType"] = image.ContentType;
            }
            else
            {
                Session["removeImage"] = true;
            }
        }

        //Проверка наличия на сервере файла с таким именем
        private string CheckFileOnServer(string path, string fileName)
        {
            string fullPath = System.IO.Path.Combine(path, fileName);
            if (System.IO.File.Exists(fullPath))
            {
                string extension = System.IO.Path.GetExtension(fullPath);
                fileName = System.IO.Path.GetFileNameWithoutExtension(fullPath);
                int i = 1;
                while (System.IO.File.Exists(fullPath))
                {
                    fullPath = System.IO.Path.Combine(path, fileName + " (" + i.ToString() + ")" + extension);
                }
            }            
            return fullPath;
        }

        // Записываем в cookie под именем name значение mean
        private void SetCookie(string name, string mean)
        {
            Response.Cookies.Set(new HttpCookie(name, mean));
        }

        // Считываем из cookie значение под именем name
        private string GetCookie(string name)
        {
            HttpCookie result = Request.Cookies.Get(name);
            return result == null ? null : result.Value;
        }

        //Удаление книги
        [HttpPost]
        public ActionResult DeleteBook(int book_id)
        {
            Book deletedBook = _booksContainer.DeleteBook(book_id);
            //Если книга была удалена - добавляем сообщение для пользователя
            if (deletedBook != null)
            {
                TempData["message"] = string.Format("Книга '{0}' была удалена", deletedBook.Title);
            }
            return BooksList(null, null, null, null);
        }
    }
}