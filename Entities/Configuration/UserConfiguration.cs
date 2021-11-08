using Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = 1,
                    FirstName = "Admin",
                    LastName = "Admin",
                    Login = "admin",
                    Password = "admin",
                    Role = EnumRole.SuperUser,
                    IsDeleted = false
                },
                  new User
                  {
                      Id = 2,
                      FirstName = "Admin",
                      LastName = "Admin",
                      Login = "admin1",
                      Password = "admin1",
                      Role = EnumRole.Admin,
                      IsDeleted = false
                  },
                    new User
                    {
                        Id = 3,
                        FirstName = "User",
                        LastName = "User",
                        Login = "user",
                        Password = "user",
                        Role = EnumRole.User,
                        IsDeleted = false
                    }
                );
        }
    }
}
