// Copyright (c) 2016 Project AIM
using CalculationCSharp.Areas.Configuration.Models;
using Newtonsoft.Json;
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
    public class CalcConfiguration
    {
        public int ID { get; set; }
        [Required]
        public string Scheme { get; set; }
        [Required]
        public string Name { get; set; }
        public string User { get; set; }
        [Column(TypeName = "xml")]
        public string Configuration { get; set; }
        public DateTime UpdateDate { get; set; }
        public decimal Version { get; set; }
    }

    public class CalcRelease
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
        public decimal Version { get; set; }
    }

    public class CalcHistory
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int CalcID { get; set; }
        [Required]
        public string Scheme { get; set; }
        [Required]
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
        public String Comment { get; set; }
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

    public class ProjectBoards
    {
        [Key]
        public int BoardId { get; set; }
        public string Client { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        [Column(TypeName = "xml")]
        public string Configuration { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual ICollection<ProjectColumns> ProjectColumns { get; set; }
    }

    public class ProjectColumns
    {

        public ProjectColumns()
        {
            ProjectStories = new List<ProjectStories>();
        }
        [Key]
        public int ColumnId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UpdateDate { get; set; }
        [Column(TypeName = "xml")]
        public virtual ProjectBoards ProjectBoard { get; set; }
        public virtual ICollection<ProjectStories> ProjectStories { get; set; }
    }

    public class ProjectStories
    {
        [Key]
        public int StoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Requested { get; set; }
        public string Moscow { get; set; }
        public string User { get; set; }
        public string Timebox { get; set; }
        public string AcceptanceCriteria { get; set; }
        public string RAG { get; set; }
        public string StartDate { get; set; }
        public string RequestedDate { get; set; }
        public int SLADays { get; set; }
        public string DueDate { get; set; }
        public string ElapsedTime { get; set; }
        public string Complexity { get; set; }
        public string Effort { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual ProjectColumns ProjectColumns { get; set; }

        public ProjectStories()
        {
            ProjectComments = new List<ProjectComments>();
            ProjectTasks = new List<ProjectTasks>();
            ProjectUpdates = new List<ProjectUpdates>();
        }

        public virtual ICollection<ProjectComments> ProjectComments { get; set; }
        public virtual ICollection<ProjectTasks> ProjectTasks { get; set; }
        public virtual ICollection<ProjectUpdates> ProjectUpdates { get; set; }

    }
    public class ProjectTasks
    {

        [Key]
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskUser { get; set; }
        public string RemainingTime { get; set; }
        public string Status { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual ProjectStories ProjectStories { get; set; }
    }
    public class ProjectUpdates
    {
        [Key]
        public int UpdateId { get; set; }
        public string UpdateField { get; set; }
        public string UpdateValue { get; set; }
        public string UpdateDateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual ProjectStories ProjectStories { get; set; }
    }
    public class ProjectComments
    {
        [Key]
        public int CommentId { get; set; }
        public string CommentName { get; set; }
        public string CommentType { get; set; }
        public string CommentDateTime { get; set; }
        public string CommentUser { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual ProjectStories ProjectStories { get; set; }
    }

    public class FileRepository
    {
        [Key]
        public int FileID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public int FileSize { get; set; }
    }
    public class Scheme
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(4)]
        public string Code { get; set; }
        [Required]
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
        public DbSet<ProjectBoards> ProjectBoards { get; set; }
        public DbSet<ProjectColumns> ProjectColumns { get; set; }
        public DbSet<ProjectStories> ProjectStories { get; set; }
        public DbSet<ProjectTasks> ProjectTasks { get; set; }
        public DbSet<ProjectComments> ProjectComments { get; set; }
        public DbSet<ProjectUpdates> ProjectUpdates { get; set; }
        public DbSet<CalcConfiguration> CalcConfiguration { get; set; }
        public DbSet<CalcRelease> CalcRelease { get; set; }
        public DbSet<CalcHistory> CalcHistory { get; set; }
        public DbSet<CalcRegressionInputs> CalcRegressionInputs { get; set; }
        public DbSet<FileRepository> FileRepository { get; set; }
        public DbSet<Scheme> Schemes { get; set; }
        public DbSet<UserSession> UserSession { get; set; }
       
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalcConfiguration>().Property(x => x.Version).HasPrecision(16, 3);
            modelBuilder.Entity<CalcHistory>().Property(x => x.Version).HasPrecision(16, 3);
            Configuration.ProxyCreationEnabled = false;

            //one-to-many 
            modelBuilder.Entity<ProjectStories>()
            .HasOptional<ProjectColumns>(s => s.ProjectColumns)
            .WithMany(s => s.ProjectStories);

            //modelBuilder.Entity<ProjectTasks>()
            //.HasOptional<ProjectStories>(s => s.ProjectStories)
            //.WithMany(s => s.ProjectTasks);

            //modelBuilder.Entity<ProjectComments>()
            //.HasOptional<ProjectStories>(s => s.ProjectStories)
            //.WithMany(s => s.ProjectComments);

            //modelBuilder.Entity<ProjectUpdates>()
            //.HasOptional<ProjectStories>(s => s.ProjectStories)
            //.WithMany(s => s.ProjectUpdates);


        }

    }
}