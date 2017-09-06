using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using BooksEditor.Validation;

namespace BooksEditor.Models.Entities
{
    public class Book
    {
        [HiddenInput(DisplayValue = false)]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Введите название книги")]
        [StringLength(30, ErrorMessage = "Длина поля \"Название книги\" не может превышать 30 символов")]
        [Display(Name = "Книга:")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Укажите хотя бы одного автора")]
        [ValidAuthorsList(ErrorMessage = "Некорректное значение в поле 'Список авторов'")]
        [Display(Name = "Автор:")]
        [DataType(DataType.MultilineText)]
        public string AuthorsList { get; set; }

        public int AuthorId { get; set; }

        [Required (ErrorMessage = "Введите количество страниц (более 0 и менее 10000)")]
        [Range(1, 10000, ErrorMessage = "Введите количество страниц (более 0 и менее 10000)")]
        [Display(Name = "Количество страниц:")]
        public int? PageCount { get; set; }

        [Display(Name = "Издательство:")]
        public string PublishHouse { get; set; } 

        [Range(1800, int.MaxValue, ErrorMessage = "Год должен быть не ранее 1800")]
        [Display(Name = "Год публикации:")]
        public int? PublishYear { get; set; }
        // путь до изображения
        public string Cover { get; set; }
        public virtual Author Author { get; set; }

        [Display(Name = "Аннотация:")]
        [DataType(DataType.MultilineText)]
        public string Annotation { get; set; }
    }
}