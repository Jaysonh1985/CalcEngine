// Copyright (c) 2016 Project AIM
using System;
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
        public string Scheme { get; set; }
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

    public class Codes
    {
        public int Id { get; set; }
        public string Group { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
    }

    public class ProjectBoard
    {
        public int ID { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        [Column(TypeName = "xml")]
        public string Configuration { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class CalcConfiguration
    {
        public int ID { get; set; }
        public string Scheme { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        [Column(TypeName = "xml")]
        public string Configuration { get; set; }
        public DateTime UpdateDate { get; set; }
        public decimal Version { get; set; }
    }

    public class CalcRelease
    {   [Key]
        public int ID { get; set; }
        public int CalcID { get; set; }
        public string Scheme { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        [Column(TypeName = "xml")]
        public string Configuration { get; set; }
        public DateTime UpdateDate { get; set; }
        public decimal Version { get; set; }
    }

    public class CalcHistory
    {
        [Key]
        public int ID { get; set; }
        public int CalcID { get; set; }
        public string Scheme { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        [Column(TypeName = "xml")]
        public string Configuration { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Comment { get; set; }
        [Required]
        public decimal Version { get; set; }
    }

    public class CalcRegressionInputs
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int CalcID { get; set; }
        public string Scheme { get; set; }
        public string Type { get; set; }
        public string Reference { get; set; }
        [Column(TypeName = "xml")]
        public String Input { get; set; }
        public String Comment { get; set;}
        public Nullable<DateTime> OriginalRunDate { get; set; }
        public Nullable<DateTime> LatestRunDate { get; set; }  
        [Column(TypeName = "xml")]
        public String OutputOld { get; set; }
        [Column(TypeName = "xml")]
        public String OutputNew { get; set; }
        [Column(TypeName = "xml")]
        public String Difference { get; set; }
        public String Pass { get; set; }
    }
    public class Scheme
    {
        [Key]
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class UserSession
    {
        [Key]
        public int ID { get; set; }
        public string Username { get; set; }
        public string Section { get; set; }
        public int Record { get; set; }
        public DateTime StartTime { get; set; }
    }

    public class CalculationDBContext : DbContext
    {
        public DbSet<CalculationResult> CalculationResult { get; set; }
        public DbSet<CalculationRegression> CalculationRegression { get; set; }
        public DbSet<Codes> Codes { get; set; }
        public DbSet<ProjectBoard> ProjectBoard { get; set; }
        public DbSet<CalcConfiguration> CalcConfiguration { get; set; }
        public DbSet<CalcRelease> CalcRelease { get; set; }
        public DbSet<CalcHistory> CalcHistory { get; set; }
        public DbSet<CalcRegressionInputs> CalcRegressionInputs { get; set; }
        public DbSet<Scheme> Schemes { get; set; }
        public DbSet<UserSession> UserSession { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalcConfiguration>().Property(x => x.Version).HasPrecision(16, 3);
            modelBuilder.Entity<CalcHistory>().Property(x => x.Version).HasPrecision(16, 3);
        }
    }
}