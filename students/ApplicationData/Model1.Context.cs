﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace students.ApplicationData
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class studentsEntities : DbContext
    {
        public studentsEntities()
            : base("name=studentsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<attendance> attendance { get; set; }
        public DbSet<classrooms> classrooms { get; set; }
        public DbSet<courses> courses { get; set; }
        public DbSet<degrees> degrees { get; set; }
        public DbSet<enrollments> enrollments { get; set; }
        public DbSet<feedback> feedback { get; set; }
        public DbSet<grades> grades { get; set; }
        public DbSet<instructors> instructors { get; set; }
        public DbSet<majors> majors { get; set; }
        public DbSet<organization_memberships> organization_memberships { get; set; }
        public DbSet<schedules> schedules { get; set; }
        public DbSet<semesters> semesters { get; set; }
        public DbSet<student_organizations> student_organizations { get; set; }
        public DbSet<students> students { get; set; }
        public DbSet<subjects> subjects { get; set; }
        public DbSet<sysdiagrams> sysdiagrams { get; set; }
    }
}
