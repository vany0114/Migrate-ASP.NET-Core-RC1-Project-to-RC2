namespace ParkingLot.Data.Repository
{
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly ParkingLotContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository()
        {
        }

        public Repository(ParkingLotContext context)
        {
            this._context = context;
            this._dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> List()
        {
            return _dbSet.ToList();
        }

        public TEntity GetById(Guid id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        public void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var entityToDelete = GetById(id);
            _dbSet.Remove(entityToDelete);
            _context.SaveChanges();
        }
    }
}
