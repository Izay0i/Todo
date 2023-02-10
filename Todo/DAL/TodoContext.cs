using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Todo.Models;

namespace Todo.DAL
{
    public class TodoContext : DbContext
    {
        public TodoContext() : base("TodoContext") { }

        public DbSet<TodoItem> Items { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}