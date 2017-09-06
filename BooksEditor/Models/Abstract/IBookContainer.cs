using System.Linq;
using BooksEditor.Models.Entities;

namespace BooksEditor.Models.Abstract
{
    public interface IBookContainer
    {
        IQueryable<Book> Books { get; }

        IQueryable<Author> Authors { get; }
        //void SaveBook();
        void SaveBook(Book book);
        Book DeleteBook(int bookId);

        void SaveNewAuthor(Author author);
    }
}