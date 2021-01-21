using System.Collections.Generic;
using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public interface ILibraryCardService
    {
        IEnumerable<LibraryCard> GetAll();
        LibraryCard Get(int id);
        void Add(LibraryCard newCard);
    }
}