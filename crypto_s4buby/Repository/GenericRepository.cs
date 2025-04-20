using crypto_s4buby.Entities;
using Microsoft.EntityFrameworkCore;

namespace crypto_s4buby.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected CryptoDbContext _dbContext;
        protected DbSet<TEntity> _dbset;

        public GenericRepository(CryptoDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbset = _dbContext.Set<TEntity>();
        }

        public void DeleteById(int id)
        {
            TEntity? entity = _dbset.Find(id);
            if (entity != null)
            {
                _dbset.Remove(entity);
            }
        }

        public async Task DeleteByIdAsync(int id)
        {
            TEntity? entity = await _dbset.FindAsync(id);
            if (entity != null)
            {
                _dbset.Remove(entity);
            }
        }

        public IEnumerable<TEntity> Get(string[]? includedProperties = null)
        {
            IQueryable<TEntity> query = _dbset;
            if (includedProperties != null)
            {
                foreach (var includedProperty in includedProperties)
                {
                    query = query.Include(includedProperty);
                }
            }
            return query.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(string[]? includedProperties = null)
        {
            IQueryable<TEntity> query = _dbset;
            if (includedProperties != null)
            {
                foreach (var includedProperty in includedProperties)
                {
                    query = query.Include(includedProperty);
                }
            }
            return await query.ToListAsync();
        }

        public TEntity? GetById(int id, string[]? includedReferences = null, string[]? includedCollections = null)
        {
            TEntity? entity = _dbset.Find(id);
            if (entity == null)
            {
                return null;
            }

            if (includedReferences != null)
            {
                foreach (string reference in includedReferences)
                {
                    _dbContext.Entry(entity).Reference(reference).Load();
                }
            }

            if (includedCollections != null)
            {
                foreach (string collection in includedCollections)
                {
                    _dbContext.Entry(entity).Collection(collection).Load();
                }
            }

            return entity;
        }

        public async Task<TEntity?> GetByIdAsync(int id, string[]? includedReferences = null, string[]? includedCollections = null)
        {
            TEntity? entity = await _dbset.FindAsync(id);
            if (entity == null)
            {
                return null;
            }

            List<Task> tasks = new List<Task>();

            if (includedReferences != null)
            {
                foreach (string reference in includedReferences)
                {
                    tasks.Add(_dbContext.Entry(entity).Reference(reference).LoadAsync());
                }
            }

            if (includedCollections != null)
            {
                foreach (string collection in includedCollections)
                {
                    tasks.Add(_dbContext.Entry(entity).Collection(collection).LoadAsync());
                }
            }

            await Task.WhenAll(tasks);

            return entity;
        }

        public void Insert(TEntity entity)
        {
            _dbset.Add(entity);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await _dbset.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _dbset.Update(entity);
        }
    }
}
