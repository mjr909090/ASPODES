using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;

namespace ASPODES.Database
{
    public class ProjectConsultationMap : EntityTypeConfiguration<ProjectConsultation>
    {
        public ProjectConsultationMap()
        {
            Map(m => 
            { 
                m.MapInheritedProperties();
                m.ToTable("ProjectConsultation");
            });
        }
    }
}
