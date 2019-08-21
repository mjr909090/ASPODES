using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class ProjectDocMap:EntityTypeConfiguration<ProjectDoc>
    {
        public ProjectDocMap()
        {
            ToTable("ProjectDoc");

            HasKey(pd => pd.ProjectDocId);

            HasRequired(pd => pd.Project)
                .WithMany( p=>p.Docs )
                .HasForeignKey(pd => pd.ProjectId)
                .WillCascadeOnDelete(true);

        }
    }
}
