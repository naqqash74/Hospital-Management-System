using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hospital_Management_System.Data.Repository
{
    public class HospitalRepository<T> : IHospitalRepository<T> where T : class
    {
        private readonly HospitalDBContext _dbContext;
        private DbSet<T> _dbSet;
        public HospitalRepository(HospitalDBContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public async Task<T> CreateAsync(T dbRecord)
        {
            _dbSet.Add(dbRecord);
            await _dbContext.SaveChangesAsync();
            return dbRecord;
        }

        public async Task<bool> DeleteAsync(T dbRecord)
        {

            _dbSet.Remove(dbRecord);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, bool useNoTracking = true)
        {
            if (useNoTracking)
                return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            else
                return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<T> UpdateAsync(T dbRecord)
        {
            _dbContext.Update(dbRecord);

            await _dbContext.SaveChangesAsync();

            return dbRecord;
        }
    }
}
