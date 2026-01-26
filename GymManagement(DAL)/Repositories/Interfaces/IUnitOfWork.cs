using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        public ISessionRepository SessionRepository { get;}
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity , new();
        IMemberShipRepository MemberShipRepository { get; }
        IBookingRepository BookingRepository { get; }
        int SaveChanges();
    }
}
