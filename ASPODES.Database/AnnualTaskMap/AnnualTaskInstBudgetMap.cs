using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class AnnualTaskInstBudgetMap : EntityTypeConfiguration<AnnualTaskInstBudget>
    {
        public AnnualTaskInstBudgetMap()
        {
            ToTable("AnnualTaskInstBudget");

            HasKey( atib=> new{atib.AnnualTaskId, atib.InstituteId } );

            HasRequired( atib=>atib.AnnualTask )
                .WithMany( at=>at.AnnualTaskInstBudgets)
                .HasForeignKey( atib=>atib.AnnualTaskId )
                .WillCascadeOnDelete(true);

            HasRequired( atib=>atib.Institute)
                .WithMany()
                .HasForeignKey( atib=>atib.InstituteId)
                .WillCascadeOnDelete(false);
        }
    }
}
