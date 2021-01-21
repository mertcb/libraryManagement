using System.Collections.Generic;
using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public interface IBookService
    {
        IEnumerable<Book> GetAll();
        IEnumerable<Book> GetByAuthor(string author);
        IEnumerable<Book> GetByIsbn(string isbn);
        Book Get(int id);
        void Add(Book newBook);
    }
}