using System.Linq;
using BooksEditor.Models.Abstract;
using BooksEditor.Models.Entities;

namespace BooksEditor.Models.Context
{
    public class EFBookContainer : IBookContainer
    {
        private EFDbContext _context = new EFDbContext();
        public IQueryable<Book> Books
        {
            get { return _context.Books; }
        }

        public IQueryable<Author> Authors
        {
            get { return _context.Authors; }
        }

        public void SaveBook(Book book)
        {
            if (book.BookId == 0)
            {
                _context.Books.Add(book);
            }
            else
            {
                Book dbBook = _context.Books.Find(book.BookId);
                if (dbBook != null)
                {
                    dbBook.Title = book.Title;
                    dbBook.AuthorId = book.AuthorId;
                    dbBook.AuthorsList = book.AuthorsList;
                    dbBook.PageCount = book.PageCount;
                    dbBook.PublishHouse = book.PublishHouse;
                    dbBook.PublishYear = book.PublishYear;
                    dbBook.Cover = book.Cover;
                    dbBook.Annotation = book.Annotation;
                }
            }
            _context.SaveChanges();
        }

        public Book DeleteBook(int bookId)
        {
            Book book = _context.Books.Find(bookId);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
            return book;
        }

        public void SaveNewAuthor(Author author)
        {
            Author dbAuthor = _context.Authors.FirstOrDefault(a => a.FirstName == author.FirstName && a.SecondName == author.SecondName);
            //Если автор отсутствует в БД - добавляем
            if (dbAuthor == null)
            {
                _context.Authors.Add(author);
                _context.SaveChanges();
            }
        }
    }
}