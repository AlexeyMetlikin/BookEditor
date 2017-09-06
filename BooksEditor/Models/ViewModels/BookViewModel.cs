using System.Collections.Generic;
using BooksEditor.Models.Entities;
using System.Linq;

namespace BooksEditor.Models.ViewModels
{
    public class BookViewModel
    {
        public List<Book> Books { get; set; }
        public string Order { get; set; }
        public Pager Pager { get; set; }

        public void SortBooks() //Сортировка книг
        {
            switch (Order)
            {
                case "YearAsc":
                    Books = Books.OrderBy(book => book.PublishYear)
                                 .Skip((Pager.CurrentPage - 1) * Pager.PageSize)
                                 .Take(Pager.PageSize)
                                 .ToList();
                    break;
                case "YearDesc":
                    Books = Books.OrderByDescending(book => book.PublishYear)
                                 .Skip((Pager.CurrentPage - 1) * Pager.PageSize)
                                 .Take(Pager.PageSize)
                                 .ToList();
                    break;
                case "TitleAsc":
                    Books = Books.OrderBy(book => book.Title)
                                 .Skip((Pager.CurrentPage - 1) * Pager.PageSize)
                                 .Take(Pager.PageSize)
                                 .ToList();
                    break;
                case "TitleDesc":
                    Books = Books.OrderByDescending(book => book.Title)
                                 .Skip((Pager.CurrentPage - 1) * Pager.PageSize)
                                 .Take(Pager.PageSize)
                                 .ToList();
                    break;
                default:
                    Books = Books.Skip((Pager.CurrentPage - 1) * Pager.PageSize)
                                 .Take(Pager.PageSize)
                                 .ToList();
                    break;
            }
        }
    }
}