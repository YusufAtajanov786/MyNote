using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        private readonly RepoContext _repoContext;

        public BaseRepo(RepoContext repoContext)
        {
            this._repoContext = repoContext;
        }

        public void Create(T entity)
        {
            _repoContext.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _repoContext.Set<T>().Remove(entity);
        }

        public IQueryable<T> FindAll(bool tracking) => !tracking ?
                                                        _repoContext.Set<T>().AsNoTracking()
                                                        : _repoContext.Set<T>();


        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool tracking) => !tracking ?
                                                        _repoContext.Set<T>().Where(expression).AsNoTracking()
                                                        : _repoContext.Set<T>().Where(expression);

        public void Update(T entity)
        {
            _repoContext.Set<T>().Update(entity);
        }
    }
}
