namespace crypto_s4buby.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(string[]? includedProperties = null);
        Task<IEnumerable<TEntity>> GetAsync(string[]? includedProperties = null);
        TEntity? GetById(int id, string[]? includedReferences = null, string[]? includedCollections = null);
        Task<TEntity?> GetByIdAsync(int id, string[]? includedReferences = null, string[]? includedCollections = null);
        void Insert(TEntity entity);
        Task InsertAsync(TEntity entity);
        void Update(TEntity entity);
        void DeleteById(int id);
        Task DeleteByIdAsync(int id);
    }
}
