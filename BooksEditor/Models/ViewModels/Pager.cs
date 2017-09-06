using System;

namespace BooksEditor.Models.ViewModels
{
    public class Pager
    {
        public Pager(int totalBooks, int? curPage, int? pageSize)
        {
            // количество книг (по умолчанию - 12)
            pageSize = pageSize ?? 12;

            var totalPages = (int)Math.Ceiling(totalBooks / (decimal)pageSize);
            var currentPage = curPage ?? 1;
            if (totalPages > 0 && currentPage > totalPages)
            {
                curPage = totalPages;
            }
            var startPage = currentPage - 2;
            var endPage = currentPage + 2;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 5)
                {
                    startPage = endPage - 4;
                }
            }

            TotalBooks = totalBooks;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
            CurrentPage = currentPage;
            PageSize = pageSize.Value;
        }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalBooks { get; set; }
    }
}