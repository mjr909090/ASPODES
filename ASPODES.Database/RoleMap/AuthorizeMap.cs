using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class AuthorizeMap : EntityTypeConfiguration<Authorize>
    {
        public AuthorizeMap()
        {
            ToTable("Authorize");

            //HasKey(a => a.AuthorizeId);
            HasKey(a => new { a.RoleId, a.UserId } );

            HasRequired(a => a.Role)
                .WithMany()
                .HasForeignKey(a => a.RoleId)
                .WillCascadeOnDelete(false);

            HasRequired(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .WillCascadeOnDelete(true);

        }
    }
}
