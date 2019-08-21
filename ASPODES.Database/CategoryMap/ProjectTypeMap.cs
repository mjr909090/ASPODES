using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
namespace ASPODES.Database
{
    public class ProjectTypeMap : EntityTypeConfiguration<ProjectType>
    {

        public ProjectTypeMap()
        {
            ToTable("ProjectType");

            HasKey(p => p.ProjectTypeId);

        }
    }

}
