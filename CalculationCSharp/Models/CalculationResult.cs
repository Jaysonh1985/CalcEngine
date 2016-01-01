using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationCSharp.Models
{
    public class CalculationResult
    {
        public int Id { get; set; }
        [Required()]
        [StringLength(100,MinimumLength =2)]
        public string User { get; set; }
        [Required()]
        [StringLength(100, MinimumLength = 2)]
        public string Scheme { get; set; }
        [Required()]
        [StringLength(100, MinimumLength = 2)]
        public string Type { get; set; }
        public DateTime RunDate { get; set; }
        public string Reference { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }

    }



    public class CalculationDBContext : DbContext
    {
        public DbSet<CalculationResult> Calculation { get; set; }
    }
}