using System;
using System.Collections.Generic;
using System.Linq;
using Todo.DAL;
using Todo.Models;

namespace Todo.Repository
{
    public class TodoRepository : IRepository<TodoItem>
    {
        private readonly TodoContext _context;

        public TodoRepository()
        {
            _context = new TodoContext();
        }

        public TodoRepository(TodoContext context)
        {
            _context = context;
        }

        IEnumerable<TodoItem> IRepository<TodoItem>.GetAll()
        {
            var items = new List<TodoItem>();

            try
            {
                var t = from task in _context.Items select task;
                items = t.OrderBy(i => i.IsComplete).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return items;
        }

        TodoItem IRepository<TodoItem>.GetById(int id)
        {
            TodoItem item = null;

            try
            {
                item = _context.Items.SingleOrDefault(x => x.ID == id);
            }
            catch (Exception)
            {
                throw new Exception("ID doesn't exist");
            }

            return item;
        }

        void IRepository<TodoItem>.Create(TodoItem item)
        {
            try
            {
                _context.Items.Add(item);
            }
            catch (Exception)
            {
                throw new Exception("Failed to create item");
            }
        }

        void IRepository<TodoItem>.Update(TodoItem item)
        {
            try
            {
                _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }
            catch (Exception)
            {
                throw new Exception("Failed to update item");
            }
        }

        void IRepository<TodoItem>.Delete(int id)
        {
            try
            {
                var item = _context.Items.SingleOrDefault(x => x.ID == id);
                if (!(item is null))
                {
                    _context.Items.Remove(item);
                }
                else
                {
                    throw new Exception("Item doesn't exist");
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to delete item");
            }
        }

        void IRepository<TodoItem>.Save()
        {
            _context.SaveChanges();
        }

        void IRepository<TodoItem>.Dispose()
        {
            _context.Dispose();
        }
    }
}