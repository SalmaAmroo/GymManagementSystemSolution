using GymManagement_DAL_.Data.DbContexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly GymDbContext _GymDbContext;

        public UnitOfWork(GymDbContext GymDbContext)
        {
            this._GymDbContext = GymDbContext;
        }
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var EntityType = typeof(TEntity);
            if (_repositories.TryGetValue(EntityType, out var Repo)) 
                return (IGenericRepository<TEntity>) Repo;

            var NewRepo = new GenericRepository<TEntity>(_GymDbContext);
            _repositories[EntityType] = NewRepo;
            return NewRepo;



        }

        public int SaveChanges()
        {
           return _GymDbContext.SaveChanges();
        }
    }
}
