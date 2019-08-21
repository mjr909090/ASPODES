using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
namespace ASPODES.Database
{
    public class FieldMap : EntityTypeConfiguration<Field>
    {

        public FieldMap()
        {
            ToTable("Field");

            HasKey<string>(f => f.FieldName);

            //HasMany<SubField>(f => f.SubFields)
            //    .WithRequired(s => s.ParentField)
            //    .HasForeignKey(s => s.ParentName)
            //    .WillCascadeOnDelete(true);
 
        }
    }

}
