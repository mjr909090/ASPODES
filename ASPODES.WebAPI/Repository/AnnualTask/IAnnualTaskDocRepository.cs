using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPODES.Database;
using ASPODES.Model;

namespace ASPODES.WebAPI.Repository
{
    public interface IAnnualTaskDocRepository
    {
        AnnualTaskDoc Get( int id );

        IQueryable<AnnualTaskDoc> GetAnnualTaskDocList();

        AnnualTaskDoc AddAttachment( AnnualTaskDoc doc );
        AnnualTaskDoc AddBody(AnnualTaskDoc doc);
        AnnualTaskDoc AddAnnualReport(AnnualTaskDoc doc);
        //AnnualTaskDoc Update( AnnualTaskDoc doc );

        void Delete( int id);

        
    }
}
