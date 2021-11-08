using Contracts;
using Entities;
using Entities.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepo: BaseRepo<User>, IUserRepo
    {
        private readonly RepoContext _repoContext;

        public UserRepo(RepoContext repoContext)
            : base(repoContext)
        {
            this._repoContext = repoContext;
        }

        public async Task<User> LoginAsync(string login, string password, bool tracking, CancellationToken cancellationToken)
        {
            return await FindByCondition(x => x.Login.Equals(login) && x.Password.Equals(password), tracking)
                .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
