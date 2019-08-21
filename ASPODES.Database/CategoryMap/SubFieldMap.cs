using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
namespace ASPODES.Database
{
    public class SubFieldMap : EntityTypeConfiguration<SubField>
    {

        public SubFieldMap()
        {
            ToTable("SubField");

            HasKey<string>(s => s.SubFieldName);

            HasRequired(s => s.ParentField)
               .WithMany()
               .HasForeignKey(s => s.ParentName)
               .WillCascadeOnDelete(true);
                                                
        }
    }

}
