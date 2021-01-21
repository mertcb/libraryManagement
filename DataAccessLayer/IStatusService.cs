using System.Collections.Generic;
using DataAccessLayer.Models;

namespace DataAccessLayer
{
    public interface IStatusService
    {
        IEnumerable<Status> GetAll();
        Status Get(int id);
        void Add(Status newStatus);
    }
}