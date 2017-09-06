var appendFile = false; //изображение не менялось

$(document).ready(function () {

    /* При наведении на книгу подсвечивать название */
    $('body').on('mouseover', '.js-book', function () {
        $(this).children('.js-delete-book').css('display', 'block');
        $(this).find('.js-book-title').css('color', 'red');
    });

    /* Окно подтверждения удаления книги */
    $('body').on('click', '.delete-book-button', function (e) {
       ConfirmDeleteBook(this, e);
    });

    /* Удалить книгу при подтверждении удаления */
    $('body').on('click', '.btn-primary', function () {
        $('#js-delete-book-form_' + $(this).data('bookId')).submit();
    })
    
    /* Возвращать цвет названия книги */
    $('body').on('mouseout', '.js-book', function () {
        $(this).children('.js-delete-book').css('display', 'none');
        $(this).find(".js-book-title").css("color", "black");
    });

    /* При клике на обложку книги - имитировать нажатие на название*/
    $('body').on('click', '.js-edit-book, .js-create-book', function () {
        $(this).next('.js-book-title').click();
    });

    /* Скрывать панель редактирования (создания) книги при нажатии на фон/кнопку закрытия */
    $('body').on('click', '#overlay, .js-close-book-button', function () {
        HideEditBookPanel();
    });

    /* При клике на кнопку выбора изображения книги имитировать нажатие input'а с типом file*/
    $('body').on('click', '#js-edit-cover', function () {
        $('#input-cover').click();
    });

    /* Удаление изображения книги */
    $('body').on('click', '#js-remove-cover', function () {
        ChangeCover(this, true);
    });

    /* Отображение выбранного изображения книги */
    $('body').on('change', '#input-cover', function () {
        ChangeCover(this, false);
    });

    /* Добавить автора в список */
    $('body').on('click', '.add-author-button', function () {
        AddAuthorToList();
    });

    /* Удалить автора из списка */
    $('body').on('click', '.delete-author-button', function () {
        $(this).prev().remove();
        $(this).remove();
        AuthorsListValidation();
    });

    /* Сохранить изменения книги на сервере */
    $('body').on('click', '.save-button', function (event) {
        SaveBook(event);
    })

    /* Плавная прокрутка страницы наверх */
    $('body').on('click', '#scroll-to-top', function () {
        $('html, body').animate({ scrollTop: 0 }, "slow");
        return false;
    })

    /* При скролле показать кнопку возврата в начало страницы */
    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('#scroll-to-top').fadeIn(100);
        } else {
            $('#scroll-to-top').hide();
        }
    });
});

//Валидация списка авторов 
function AuthorsListValidation() {
    var authorsList = $('#AuthorsList');

    if (authorsList.children().length == 0) {
        authorsList.addClass('input-validation-error authors-list-placeholder');
    }    
}

//Placeholder для списка авторов
function ShowAuthorsListPlaceholder() {
    var authorsList = $('#AuthorsList');

    if (authorsList.children().length == 0) {
        authorsList.addClass('authors-list-placeholder')
    }
}

//Добавляем классы для сортировки
function Sorting() {
    var id = this.id;

    //Если сортировка уже выполняется по элементу this
    if ($(this).hasClass('is-clicked')) {
        $(this).toggleClass(id + "-Asc")
               .toggleClass(id + "-Desc");
    }
    else {
        $('#year-sort').removeClass();
        $('#title-sort').removeClass();

        $(this).toggleClass('is-clicked')
               .toggleClass(id + "-Asc");
    }
}

//Модальное окно подтверждения удаления 
function ConfirmDeleteBook(button, event) {
    event.preventDefault();
    var dialogMessage = '<span>Вы уверены, что хотите удалить книгу "' + $(button).data('bookTitle') + '"?</span>';
    $('#modalDialog').find('.modal-body')
                     .html(dialogMessage);
    $('#modalDialog').find('.btn-primary')
                     .data('bookId', $(button).data('bookId'))
    $('#modalDialog').modal();
}

//Показать форму редактирования книги
function ShowEditBookPanel(data) {
    $('#bookInfo').html(data)
                  .fadeIn(500);
    $('#overlay').show();
    $('input[placeholder], textarea[placeholder]').placeholder();
    ShowAuthorsListPlaceholder();
}

//Скрыть форму редактирования книги
function HideEditBookPanel() {
    $("#bookInfo").hide();
    $("#bookInfo").empty();
    $("#overlay").hide();
}

//Добавить автора в список
function AddAuthorToList() {
    var name_inp = $('#FirstName');
    var surname_inp = $('#SecondName');

    //Если валидация успешна - добавляем автора
    if (name_inp.val() && surname_inp.val() &&
        $(name_inp).hasClass('valid') && $(surname_inp).hasClass('valid')) {
        var name = FirstToUpper(name_inp.val().trim());
        var surname = FirstToUpper(surname_inp.val().trim());
        var new_author = '<span>' + name + ' ' + surname + '</span>\
                          <a class="delete-author-button"></a>';

        var authorsList = $('#AuthorsList')
        if (authorsList.hasClass('authors-list-placeholder')) {
            authorsList.empty()
                       .removeClass('authors-list-placeholder');
        }
        authorsList.append(new_author)
                   .removeClass('input-validation-error');

        name_inp.val('');
        surname_inp.val('');
    }
}

//Сделать первую букву строки заглавной
function FirstToUpper(str) {
    if (!str) {
        return str;
    }
    else {
        return str[0].toUpperCase() + str.slice(1);
    }
}

//clearCover - флаг удаления изображения
function ChangeCover(elem, clearCover) {
    var img = document.getElementById('popup-img-cover');

    // Удаляем изображение
    if (clearCover) {
        img.src = 'Content/images/default/noCover.png';
        $('#input-cover').prop('value', null);
        appendFile = null;
    }
        // показываем выбранное изрображение
    else if (elem.files && elem.files[0]) {
        img.src = URL.createObjectURL(elem.files[0]);
        appendFile = elem.files[0];
    }
}

function UpdateBooksList(data) {
    $('#booksList').html(data);
    HideEditBookPanel();
}

function SaveBook(event) {
    event.preventDefault();
    var formData = new FormData();

    //получаем массив всех авторов (в теге span)
    var allAuthors = $('#AuthorsList').find('span');

    var autortsList = '';

    // Формируем список авторов в переменной autortsList через запятую
    $.each(allAuthors, function (index, author) {
        if (index == allAuthors.length - 1) {
            autortsList += author.textContent;
        }
        else {
            autortsList += author.textContent + ',';
        }
    });

    //Получаем значение всех input'ов - параметров книги
    var book =
        {
            'BookId': $('#BookId').val(),
            'Title': $('#Title').val(),
            'AuthorsList': autortsList,
            'PageCount': $("#PageCount").val(),
            'PublishHouse': $('#PublishHouse').val(),
            'PublishYear': $('#PublishYear').val(),
            'Annotation': $('#Annotation').val(),
            'Cover': $('#popup-img-cover').attr('src'),
            'AuthorId': 0,
            'Author': null
        }

    //Если изображение не менялось - сразу вызываем метод редактирования книги
    if (appendFile == false) {
        SetDataToServer(book);
    }
    else {
        if (appendFile !== null) {
            formData.set('image', appendFile);
        }

        //Загружаем добавленный файл на сервер
        $.ajax({
            type: 'POST',
            url: '/Book/UploadImage',
            data: formData,
            contentType: false,
            processData: false,
            success: function () {
                // Если удалось загрузить изображение - вызываем сохранение книги
                appendFile = false;
                SetDataToServer(book);
            },
            error: function () {
                alert("Не удалось загрузить изображение!");
            }
        });
    }
}

    // Отправка значений полей input'ов на сервер в json формате
function SetDataToServer(book) {

    $.ajax({
        type: 'POST',
        url: '/Book/EditBook',
        data: JSON.stringify(book),
        contentType: 'application/json; charset=utf-8',
        success: function (result) {

            //Если форма прошла валидацию, книга успешно обновлена (добавлена)
            if (~result.indexOf('class="list-of-books"')) {
                // обновляем список книг
                $('#booksList').html(result);
                //скрываем окно редактирования 
                HideEditBookPanel();
            }
                //Если форма не прошла валидацию
            else {
                //Возвращаем форму с ошибками валидации 
                $('#bookInfo').html(result);
                AuthorsListValidation();
            }

        },
        error: function (xhr, status) {
            alert('Ошибка при сохранении книги!');
        }
    });
}