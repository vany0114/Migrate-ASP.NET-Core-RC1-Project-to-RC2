using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLot.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        IEnumerable<TEntity> List();

        TEntity GetById(Guid id);

        void Insert(TEntity entity);

        void Update(TEntity entity);

        void Delete(Guid id);
    }
}
