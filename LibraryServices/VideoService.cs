using System.Collections.Generic;
using System.Linq;
using DataAccessLayer;
using DataAccessLayer.Models;
using lms.DataAccessLayer;

namespace LibraryServices
{
    public class VideoService : IVideoService
    {
        private readonly LibraryDbContext _context;

        public VideoService(LibraryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Video> GetAll()
        {
            return _context.Videos;
        }

        public IEnumerable<Video> GetByDirector(string director)
        {
            return _context.Videos.Where(a => a.Director.Contains(director));
        }

        public void Add(Video newVideo)
        {
            _context.Add(newVideo);
            _context.SaveChanges();
        }

        public Video Get(int id)
        {
            return _context.Videos.FirstOrDefault(v => v.Id == id);
        }
    }
}