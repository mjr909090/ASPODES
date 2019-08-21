using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class AnnualTaskBudgetItemMap:EntityTypeConfiguration<AnnualTaskBudgetItem>
    {
        public AnnualTaskBudgetItemMap()
        {
            ToTable("AnnualTaskBudgetItem");
            HasKey( atbi=>atbi.AnnualTaskBudgetItemId );
            HasRequired(atbi => atbi.AnnualTask)
                .WithMany( at=>at.AnnualTaskBudgetItems )
                .HasForeignKey(atbi => atbi.AnnualTaskId)
                .WillCascadeOnDelete(true);

            HasRequired(atbi => atbi.Subject)
                .WithMany()
                .HasForeignKey(atbi => atbi.SubjectId)
                .WillCascadeOnDelete(false);


        }
    }
}
