using GymManagement_DAL_.Data.DbContexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace GymManagementDAL.Repositories.Classes
{
    public class GenericRepository<IEntitiy> : IGenericRepository<IEntitiy> where IEntitiy : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;
        public GenericRepository(GymDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public IEntitiy? GetById(int id) => _dbContext.Set<IEntitiy>().Find(id);
        public void Add(IEntitiy entity) =>_dbContext.Set<IEntitiy>().Add(entity);
        
        public void Delete(IEntitiy entity) => _dbContext.Set<IEntitiy>().Remove(entity);
        
        public void Update(IEntitiy entity) => _dbContext.Set<IEntitiy>().Update(entity);
          

        public IEnumerable<IEntitiy> GetAll(Func<IEntitiy, bool>? condition = null)
        {
            if (condition is null)
            {
                return _dbContext.Set<IEntitiy>().AsNoTracking().ToList();
            }
            else
            {
                return _dbContext.Set<IEntitiy>().AsNoTracking().Where(condition).ToList();
            }
        }
    }
}
