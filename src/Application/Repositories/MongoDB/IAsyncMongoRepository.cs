using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.MongoDB;

public interface IAsyncMongoRepository<TEntity>
{
    Task AddAsync(TEntity entity);
    Task DeleteAsync(string id);
    Task UpdateAsync(TEntity entity);
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter);
    Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null);
}
