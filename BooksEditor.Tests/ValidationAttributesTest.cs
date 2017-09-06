using Microsoft.VisualStudio.TestTools.UnitTesting;
using BooksEditor.Validation;

namespace BooksEditor.Tests
{
    [TestClass]
    public class ValidationAttributesTest
    {
        [TestMethod]
        public void Not_Have_Error_For_Valid_Authors_List()
        {
            // Arrange
            string authorsList = "Иван Иванов, Петр Петров, Сидор Сидоров";

            //Создаем экземпляр класса ValidAuthorsListAttribute для проверки на валидность authorsList
            var validation = new ValidAuthorsListAttribute();

            // Act
            // Выполняем проверку
            bool result = validation.IsValid(authorsList);
            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Have_Error_For_Invalid_Authors_List()
        {
            // Arrange
            string authorsList_error = "Иван Иванов, Петр Петров Сидор Сидоров";
            string authorsList_BigName = "Рошандиателлинешьяунневешенк Иванов";
            string authorsList_BigSurname = "Иван Аийильцикликирмицибайрактазийанкаграманоглу";

            //Создаем экземпляр класса ValidAuthorsListAttribute для проверки на валидность authorsList
            var validation = new ValidAuthorsListAttribute();

            // Act
            // Выполняем проверку
            bool result_error = validation.IsValid(authorsList_error);
            bool result_BigName = validation.IsValid(authorsList_BigName);
            bool result_BigSurname = validation.IsValid(authorsList_BigSurname);

            //Assert
            Assert.IsFalse(result_error);
            Assert.IsFalse(result_BigName);
            Assert.IsFalse(result_BigSurname);
        }
    }
}
