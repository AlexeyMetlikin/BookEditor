using System.Data.Entity;
using System.IO;
using BooksEditor.Models.Context;
using BooksEditor.Models.Entities;

namespace BooksEditor.Models
{
    public class BookDbInit : DropCreateDatabaseIfModelChanges<EFDbContext>
    {
        protected override void Seed(EFDbContext db)
        {
            db.Authors.Add(new Author { FirstName = "Александр", SecondName = "Дюма" });
            db.Authors.Add(new Author { FirstName = "Жуль", SecondName = "Верн" });
            db.Authors.Add(new Author { FirstName = "Стивен", SecondName = "Кинг" });
            db.Authors.Add(new Author { FirstName = "Чарльз", SecondName = "Диккенс" });
            db.Authors.Add(new Author { FirstName = "Лев", SecondName = "Толстой" });
            db.Authors.Add(new Author { FirstName = "Александр", SecondName = "Пушкин" });
            db.Authors.Add(new Author { FirstName = "Семен", SecondName = "Липкин" });
            db.Authors.Add(new Author { FirstName = "Валентина", SecondName = "Потапова" });

            db.Books.Add(new Book { Title = "Руслан и Людмила", AuthorsList = "Александр Пушкин", AuthorId = 6, PageCount = 5127, PublishYear = 2001 });
            db.Books.Add(new Book { Title = "Евгений Онегин", AuthorsList = "Александр Пушкин", AuthorId = 6, PageCount = 1251, PublishHouse = "ACT", PublishYear = 1956 });
            db.Books.Add(new Book { Title = "Сияние", AuthorsList = "Стивен Кинг", AuthorId = 3, PageCount = 1027, PublishHouse = "Бином", PublishYear = 1978 });
            db.Books.Add(new Book { Title = "Дети капитана Гранта", AuthorsList = "Жуль Верн", AuthorId = 2, PageCount = 5127, PublishHouse = "Астра", PublishYear = 2001 });
            db.Books.Add(new Book { Title = "Граф Монте-Кристо", AuthorsList = "Александр Дюма", AuthorId = 1, PageCount = 1251, PublishYear = 1956 });
            db.Books.Add(new Book { Title = "Борис Годунов", AuthorsList = "Александр Пушкин", AuthorId = 6, PageCount = 1027, PublishHouse = "Бином", PublishYear = 1978 });
            db.Books.Add(new Book { Title = "Крошка Доррит", AuthorsList = "Чарльз Диккенс", AuthorId = 4, PageCount = 5127, PublishHouse = "Астра", PublishYear = 2001 });
            db.Books.Add(new Book { Title = "Махабхарата. Рамаяна", AuthorsList = "Семен Липкин, Валентина Потапова", AuthorId = 7, PageCount = 1251, PublishHouse = "ACT", PublishYear = 1956 });
            db.Books.Add(new Book { Title = "Противостояние", AuthorsList = "Стивен Кинг", AuthorId = 3, PageCount = 1027, PublishHouse = "Бином", PublishYear = 1978 });
            db.Books.Add(new Book { Title = "Медный всадник", AuthorsList = "Александр Пушкин", AuthorId = 6, PageCount = 5127, PublishYear = 2001 });
            db.Books.Add(new Book { Title = "Три мушкетера", AuthorsList = "Александр Дюма", AuthorId = 1, PageCount = 1251, PublishHouse = "ACT", PublishYear = 1956 });
            db.Books.Add(new Book { Title = "Под куполом", AuthorsList = "Стивен Кинг", AuthorId = 3, PageCount = 1027, PublishHouse = "Бином", PublishYear = 1978 });
            db.Books.Add(new Book { Title = "Жребий", AuthorsList = "Стивен Кинг", AuthorId = 3, PageCount = 5127, PublishYear = 2001 });

            base.Seed(db);
        }
    }
}