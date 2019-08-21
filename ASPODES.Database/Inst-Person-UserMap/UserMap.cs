using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class UserMap:EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("User");

            HasKey(u => u.UserId);

            HasOptional(u => u.Institute)
                .WithMany()
                .HasForeignKey(u => u.InstituteId)
                .WillCascadeOnDelete(false);
            
        }
    }
}
