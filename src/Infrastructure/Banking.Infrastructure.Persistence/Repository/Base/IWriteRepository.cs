namespace Banking.Infrastructure.Persistence.Repository.Base
{
    public interface IWriteRepository<TEntity> where TEntity : class
    {
        void Insert(TEntity entity);

        void InsertRange(List<TEntity> entities);

        void Update(TEntity entity);

        void UpdateRange(List<TEntity> entities);

        void Delete(TEntity entity);

        void DeleteRange(List<TEntity> entities);
    }
}