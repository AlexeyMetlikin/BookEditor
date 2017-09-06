using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace BooksEditor.Validation
{
    public class ValidAuthorsListAttribute : ValidationAttribute
    {
        public ValidAuthorsListAttribute()
        {

        }
        public override bool IsValid(object value)
        {
            string name = null, surname = null;
            //Результат валидации
            bool result = true;             
            //Регулярное выражение для проверки имени/фамилии автора
            Regex reg = new Regex("^[a-zA-Zа-яА-Я]+$");
            if (value != null)
            {
                //Для каждого автора в списке
                foreach (string auth in value.ToString().Split(','))  
                {
                    string author = auth.TrimStart();
                    //Если больше двух слов для одного автора
                    if (author.Split(' ').Length > 2)   
                    {
                        result = false;
                    }
                    else
                    {
                        name = author.Split(' ')[0];
                        surname = author.Split(' ')[1];
                        //Если автор не соответствует условияем
                        if (!reg.IsMatch(name) || name.Length > 20 || name.Length < 2 ||
                            !reg.IsMatch(surname) || surname.Length > 20 || surname.Length < 2)
                        {
                            result = false;
                        }
                    }
                }
            }
            return result;
        }
    }
}