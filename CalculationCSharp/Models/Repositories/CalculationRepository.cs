using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Models.Repositories
{
    public class CalculationRepository : Repository<CalculationResult>
    {

        public List<CalculationResult> GetByScheme(String Scheme)
        {
            return DbSet.Where(a => a.Scheme.Contains(Scheme)).ToList();
        }

    }
}