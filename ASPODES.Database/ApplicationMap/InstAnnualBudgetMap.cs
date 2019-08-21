using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class InstAnnualBudgetMap:EntityTypeConfiguration<InstAnnualBudget>
    {
        public InstAnnualBudgetMap()
        {
            ToTable("InstAnnualBudget");
            HasKey( iab=>iab.InstAnnualBudgetId );

            HasRequired(iab => iab.InstBudget)
                .WithMany(ib=>ib.InstAnnualBudgets)
                .HasForeignKey(iab => iab.InstBudgetId)
                .WillCascadeOnDelete(true);
        }
    }
}
