using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Todo.Models;
using Todo.Repository;

namespace Todo.Controllers
{
    public class TodoItemController : Controller
    {
        private readonly IRepository<TodoItem> _repo;

        private void _WriteTsv<T>(IEnumerable<T> data, TextWriter output)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output.Write(prop.DisplayName);
                output.Write("\t");
            }

            output.WriteLine();

            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output.Write(prop.Converter.ConvertToString(prop.GetValue(item)));
                    output.Write("\t");
                }

                output.WriteLine();
            }
        }

        public void ExportListFromTsv()
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", $"attachment;filename=Exported_Tasks_{DateTime.Now}.xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            var items = from t in _repo.GetAll()
                        select new
                        {
                            Name = t.Name,
                            Description = t.Description,
                            IsComplete = t.IsComplete
                        };
            
            _WriteTsv(items, Response.Output);
            Response.End();
        }

        public TodoItemController(IRepository<TodoItem> repo)
        {
            _repo = repo;
        }

        // GET: TodoItem
        public ActionResult Index()
        {
            return View(_repo.GetAll());
        }

        // GET: TodoItem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TodoItem todoItem = _repo.GetById((int)id);
            if (todoItem == null)
            {
                return HttpNotFound();
            }
            return View(todoItem);
        }

        // GET: TodoItem/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TodoItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description,IsComplete")] TodoItem todoItem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _repo.Create(todoItem);
                    _repo.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(todoItem);
        }

        // GET: TodoItem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TodoItem todoItem = _repo.GetById((int)id);
            if (todoItem == null)
            {
                return HttpNotFound();
            }
            return View(todoItem);
        }

        // POST: TodoItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,IsComplete")] TodoItem todoItem)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(todoItem);
                _repo.Save();
                return RedirectToAction("Index");
            }
            return View(todoItem);
        }

        // GET: TodoItem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TodoItem todoItem = _repo.GetById((int)id);
            if (todoItem == null)
            {
                return HttpNotFound();
            }
            return View(todoItem);
        }

        // POST: TodoItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.Delete(id);
            _repo.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
