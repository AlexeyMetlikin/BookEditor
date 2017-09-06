using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BooksEditor.Models.Entities
{
    public class Author
    {
        public Author()
        {
            this.Books = new HashSet<Book>();
        }
        // ID автора
        public int AuthorId { get; set; }
        
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Заполните имя автора (не более 20 символов)")]
        [RegularExpression("^[a-zA-Zа-яА-Я]+$", ErrorMessage = "Некорректное имя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [StringLength(20, MinimumLength = 2, ErrorMessage = "Заполните фамилию автора (не более 20 символов)")]
        [RegularExpression("^[a-zA-Zа-яА-Я]+$", ErrorMessage = "Некорректная фамилия")]
        [Display(Name = "Фамилия")]
        public string SecondName { get; set; }
        //Книги автора
        public virtual ICollection<Book> Books { get; set; }

    }
}