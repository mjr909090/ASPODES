using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class InstBudgetMap:EntityTypeConfiguration<InstBudget>
    {
        public InstBudgetMap()
        {
            ToTable("InstBudget");

            HasKey(ib => ib.InstBudgetId);

            HasRequired(ib => ib.Institute)
                .WithMany()
                .HasForeignKey(ib => ib.InstituteId)
                .WillCascadeOnDelete(false);

            HasRequired(ib => ib.Application)
                .WithMany()
                .HasForeignKey(ib => ib.ApplicationId)
                .WillCascadeOnDelete(true);

        }
    }
}
