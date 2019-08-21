using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.DTO.Application
{
    public class AddInstTotalWithAnnualBudget
    {
        public AddInstTotalWithAnnualBudget()
        {
            AnnualBudgets = new List<double>();
        }
        public int? InstBudgetId { get; set; }
        public string ApplicationId { get; set; }
        public int? InstituteId { get; set; }
        public double? Amount { get; set; }

        public IEnumerable<double> AnnualBudgets { get; set; }
    }
}
