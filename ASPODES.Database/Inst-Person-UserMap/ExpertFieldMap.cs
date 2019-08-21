using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    class ExpertFieldMap:EntityTypeConfiguration<ExpertField>
    {
        public ExpertFieldMap()
        {
            ToTable("ExpertField");

            HasKey(ef => ef.ExpertFieldId);

            HasRequired(ef => ef.SubField)
                .WithMany()
                .HasForeignKey(ef => ef.SubFieldId)
                .WillCascadeOnDelete(true);

            HasRequired(ef => ef.User)
                .WithMany(u=>u.ExpertFields)
                .HasForeignKey(ef => ef.UserId)
                .WillCascadeOnDelete(true);
        }
    }
}
