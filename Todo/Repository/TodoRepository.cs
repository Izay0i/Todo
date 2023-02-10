using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                items = _context.Items.ToList();
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return item;
        }

        void IRepository<TodoItem>.Create(TodoItem item)
        {
            try
            {
                _context.Items.Add(item);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        void IRepository<TodoItem>.Update(TodoItem item)
        {
            try
            {
                _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                    throw new Exception("Item doesn't exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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