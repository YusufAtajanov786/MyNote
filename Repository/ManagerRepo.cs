using Contracts;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ManagerRepo : IManagerRepo
    {
        private readonly RepoContext _repoContext;
        private IUserRepo _userRepo;
        public ManagerRepo(RepoContext repoContext)
        {
            this._repoContext = repoContext ?? throw new ArgumentNullException(nameof(repoContext));
        }

        public IUserRepo User
        {
            get
            {
                if(_userRepo == null)
                {
                    _userRepo = new UserRepo(_repoContext);
                }
                return _userRepo;
            }
        }

        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}
