using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TODOListApp.Data;
using TODOListApp.Models;

namespace TODOListApp.Controllers
{
    public class TodoListController : Controller
    {
        private readonly AppDbContext _context;

        public TodoListController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var todos = await _context.TodoItems.ToListAsync();
            return View(new TodoViewModel { TodoItems = todos });
        }


        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(TodoItem item)
        {
            if (!ModelState.IsValid) return View(item);
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int id)
        {
            return View(await GetTodoItemAsync(id));
        }

        private async Task<TodoItem> GetTodoItemAsync(int id)
        {
            return await _context.TodoItems.FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View(await GetTodoItemAsync(id));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(TodoItem item)
        {
            if (!ModelState.IsValid) return View(item);
            _context.TodoItems.Attach(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            return View(await GetTodoItemAsync(id));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Delete(TodoItem item)
        {
            if (!ModelState.IsValid) return View(item);
            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

    public class TodoViewModel
    {
        public IEnumerable<TodoItem> TodoItems { get; set; }
        public TodoItem NewTodo { get; set; }
    }
}