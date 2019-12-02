using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var todos = await _context.TodoItems
                .Include(item => item.TodoTags)
                .ToListAsync();
            return View(new TodoListViewModel {TodoItems = todos});
        }


        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(TodoItem newTodo)
        {
            if (!ModelState.IsValid) return View(newTodo);
            _context.TodoItems.Add(newTodo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int id)
        {
            return View(await GetTodoItemAsync(id));
        }

        private async Task<TodoItem> GetTodoItemAsync(int id)
        {
            return await _context.TodoItems.Include(item => item.TodoTags).FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var todoItem = await GetTodoItemAsync(id);
            var availableTags = _context.Tags.ToListAsync().Result
                .Select(tag => new SelectListItem(tag.Name, tag.Name))
                .ToList();

            var model = new TodoItemViewModel {TodoItem = todoItem, AvailableTags = availableTags};

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(TodoItemViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var tagsFromDb = new List<Tag>();
            foreach (var tagName in model.SelectedTags)
            {
                tagsFromDb.Add(await _context.Tags.FirstOrDefaultAsync(tag => tag.Name.Equals(tagName)));
            }
            model.TodoItem.AddTags(tagsFromDb);

            _context.TodoItems.Attach(model.TodoItem).State = EntityState.Modified;
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
}