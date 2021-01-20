using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryServices
{
    public class LibraryAssetService : ILibraryAsset
    {
        private LibraryContext _context;

        public LibraryAssetService(LibraryContext context)
        {
            _context = context;
        }

        public void Add(LibraryAsset asset)
        {
            _context.Add(asset);
            _context.SaveChanges();

        }

        public IEnumerable<LibraryAsset> GetAll()
        {
            return _context.LibraryAssets
                .Include(asset => asset.Status)
                .Include(asset => asset.Location);
        }

        public LibraryAsset GetById(int id)
        {
            return GetAll()
                .FirstOrDefault(asset => asset.Id == id);
        }

        public LibraryBranch GetCurrentLocation(int id)
        {
            return GetById(id).Location;
        }

        public string GetDeweyIndex(int id)
        {
            if (_context.Books.Any(book => book.Id == id))
            {
                return _context.Books.FirstOrDefault(book => book.Id == id).DeweyIndex;
            }

            return "";
        }

        public string GetIsbn(int id)
        {
            if (_context.Books.Any(book => book.Id == id))
            {
                return _context.Books.FirstOrDefault(book => book.Id == id).ISBN;
            }

            return "";
        }

        public string GetTitle(int id)
        {
            return _context.LibraryAssets.FirstOrDefault(asset => asset.Id == id).Title;
        }

        public string GetType(int id)
        {
            var isBook = _context.LibraryAssets.OfType<Book>().Where(asset => asset.Id == id).Any();

            return isBook ? "Book" : "Video" ?? "Unknown";
        }

        public string GetAuthorOrDirector(int id)
        {
            var isBook = _context.LibraryAssets.OfType<Book>().Where(asset => asset.Id == id).Any();

            return isBook
                ? _context.Books.FirstOrDefault(asset => asset.Id == id).Author
                : _context.Videos.FirstOrDefault(asset => asset.Id == id).Director ?? "Unknown";
        }
    }
}
