using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class AnnualBudgetMap : EntityTypeConfiguration<AnnualBudget>
    {
        public AnnualBudgetMap()
        {
            ToTable("AnnualBudget");
            
            HasKey(ab => ab.AnnualBudgetId);

            HasRequired(ab => ab.Application)
                .WithMany(a=>a.AnnualBudgets)
                .HasForeignKey(ab => ab.ApplicationId)
                .WillCascadeOnDelete(true);

        }
    }
}
