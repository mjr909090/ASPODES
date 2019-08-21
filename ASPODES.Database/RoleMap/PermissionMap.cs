using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class PermissionMap : EntityTypeConfiguration<Permission>
    {
        public PermissionMap()
        {
            ToTable("Permission");

            //HasKey(pe => pe.PermissionId );

            HasKey(pe => new { pe.ResourceId, pe.RoleId });

            HasRequired(pe => pe.Role)
               .WithMany()
               .HasForeignKey(pe => pe.RoleId)
               .WillCascadeOnDelete(true);

            HasRequired(pe => pe.Resource)
                .WithMany()
                .HasForeignKey(pe => pe.ResourceId)
                .WillCascadeOnDelete(true);

        }
    }
}
