﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Model;
namespace ASPODES.Database
{
    public class TemplateDocMap : EntityTypeConfiguration<TemplateDoc>
    {
        public TemplateDocMap()
        {
            ToTable("TemplateDoc");

            HasKey(t => t.DocId);

        }
    }

}
