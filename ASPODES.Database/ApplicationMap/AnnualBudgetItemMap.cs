using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
using System.Data.Entity.ModelConfiguration;

namespace ASPODES.Database
{
    public class AnnualBudgetItemMap:EntityTypeConfiguration<AnnualBudgetItem>
    {
        public AnnualBudgetItemMap()
        {
            ToTable("AnnualBudgetItem");

            HasKey( abi=>abi.AnnualBudgetItemId );

            HasRequired(abi => abi.AnnualBudget)
                .WithMany(ab=>ab.Items)
                .HasForeignKey(abi => abi.AnnualBudgetId)
                .WillCascadeOnDelete(true);

            HasRequired(abi => abi.Subject)
                .WithMany()
                .HasForeignKey(abi => abi.SubjectId)
                .WillCascadeOnDelete(false);

        }

    }
}
