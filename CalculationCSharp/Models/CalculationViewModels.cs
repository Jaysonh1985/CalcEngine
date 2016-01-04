﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


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
        [Column(TypeName = "xml")]
        public String Input { get; set; }
        [Column(TypeName = "xml")]
        public String Output { get; set; }

    }

    public class CalculationRegression
    {
        public int Id { get; set; }
        [Required()]
        [StringLength(100, MinimumLength = 2)]
        public string Scheme { get; set; }
        [Required()]
        [StringLength(100, MinimumLength = 2)]
        public string Type { get; set; }
        public DateTime OriginalRunDate { get; set; }
        public DateTime LatestRunDate { get; set; }
        public string Reference { get; set; }
        [Column(TypeName = "xml")]
        public String Input { get; set; }
        [Column(TypeName = "xml")]
        public String OutputOld { get; set; }
        [Column(TypeName = "xml")]
        public String OutputNew { get; set; }
        [Column(TypeName = "xml")]
        public String Difference { get; set; }
        public String Pass { get; set; }

    }


    public class CalculationDBContext : DbContext
    {
        public DbSet<CalculationResult> CalculationResult { get; set; }
        public DbSet<CalculationRegression> CalculationRegression { get; set; }
    }
}