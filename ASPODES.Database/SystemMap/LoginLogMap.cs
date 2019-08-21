using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class LoginLogMap:EntityTypeConfiguration<LoginLog>
    {
        public LoginLogMap()
        {
            ToTable("LoginLog");
            HasKey(ll => ll.LoginLogId);

            HasRequired(ll => ll.User)
                .WithMany()
                .HasForeignKey(ll => ll.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}
