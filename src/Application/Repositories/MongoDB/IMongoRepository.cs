using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.MongoDB;

public interface IMongoRepository<TEntity>
{
    void Add(TEntity entity);
    void Delete(string id);
    void Update(TEntity entity);
    TEntity Get(Expression<Func<TEntity, bool>> filter);
    IList<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null);
}
