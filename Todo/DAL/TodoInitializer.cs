using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Todo.Models;

namespace Todo.DAL
{
    public class TodoInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<TodoContext>
    {
        protected override void Seed(TodoContext context)
        {
            var items = new List<TodoItem>
            {
                new TodoItem { Name = "Test", Description = "Test", },
                new TodoItem { Name = "Foo", Description = "Bar", },
            };
            items.ForEach(item => context.Items.Add(item));
            context.SaveChanges();
        }
    }
}