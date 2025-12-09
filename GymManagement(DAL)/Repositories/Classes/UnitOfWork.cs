using GymManagement_DAL_.Data.DbContexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;


namespace GymManagementDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly GymDbContext _GymDbContext;

        public UnitOfWork(GymDbContext GymDbContext, ISessionRepository sessionRepository)
        {
            _GymDbContext = GymDbContext;
            SessionRepository = sessionRepository;
        }

        public ISessionRepository SessionRepository { get;}

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
