using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TODOListAppV2.Data;
using TODOListAppV2.Models;
using TODOListAppV2.ViewModels;

namespace TODOListAppV2.Controllers
{
    public class TodoListController : Controller
    {
        private readonly TodoContext _context;

        public TodoListController(TodoContext context)
        {
            _context = context;
        }

        // GET: TodoList
        public async Task<IActionResult> Index(string sortOrder, string currentFilter)
        {
            ViewData["NameSort"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DescriptionSort"] = sortOrder == "description_asc" ? "description_desc" : "description_asc";
            ViewData["CurrentFilter"] = currentFilter;


            var todoItems = from t in _context.TodoItem select t;
            if (!string.IsNullOrEmpty(currentFilter))
            {
                todoItems = todoItems.Where(
                    t => t.Name.Contains(currentFilter) || t.Description.Contains(currentFilter));
            }
            
            switch (sortOrder)
            {
                case "name_desc":
                    todoItems = todoItems.OrderByDescending(t => t.Name);
                    break;
                case "description_desc":
                    todoItems = todoItems.OrderByDescending(t => t.Description);
                    break;
                case "description_asc":
                    todoItems = todoItems.OrderBy(t => t.Description);
                    break;
                default:
                    todoItems = todoItems.OrderBy(t => t.Name);
                    break;
            }


            var filteredList = await todoItems.Include(t => t.TagAssignments).ToListAsync();
            var viewModel = new CreateTodoViewModel {TodoItems = filteredList};
            return View(viewModel);
        }

        // GET: TodoList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await GetTodoItemNoTracking(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }

        private async Task<TodoItem> GetTodoItemNoTracking(int? id)
        {
            var todoItem = await _context.TodoItem
                .Include(t => t.TagAssignments)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            return todoItem;
        }

        // GET: TodoList/Create
        public IActionResult Create()
        {
            PopulateSelectedTagsData();
            return View();
        }

        // POST: TodoList/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] TodoItem todoItem, string[] selectedTags)
        {
            if (!ModelState.IsValid) return View(todoItem);
            
            UpdateTodoTags(selectedTags, todoItem);
            _context.Add(todoItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: TodoList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await GetTodoItem(id);

            PopulateSelectedTagsData(todoItem);

            if (todoItem == null)
            {
                return NotFound();
            }
            return View(todoItem);
        }

        private async Task<TodoItem> GetTodoItem(int? id)
        {
            var todoItem = await _context.TodoItem
                .Include(t => t.TagAssignments)
                .SingleAsync(item => item.Id == id);
            return todoItem;
        }

        private void PopulateSelectedTagsData(TodoItem todoItem = null)
        {
            var allTags = _context.Tags;
            
            var todoTags = todoItem != null ? new HashSet<string>(todoItem.TagAssignments.Select(t => t.TagName)) : new HashSet<string>();
            var viewModel = allTags
                .Select(tag => new AvailableTagData {TagName = tag.Name, Selected = todoTags.Contains(tag.Name)})
                .ToList();

            ViewData["Tags"] = viewModel;
        }

        // POST: TodoList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedTags)
        {
            if (id == null) return NotFound();

            var todoItemToUpdate = await _context.TodoItem.Include(t => t.TagAssignments).FirstOrDefaultAsync(t => t.Id == id);
            if (await TryUpdateModelAsync(todoItemToUpdate, "", t => t.Name, t => t.Description, t => t.TagAssignments))
            {
                UpdateTodoTags(selectedTags, todoItemToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException e)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                                                 "Try again, and if the problem persists, " +
                                                 "see your system administrator.");
                }
            }
            UpdateTodoTags(selectedTags, todoItemToUpdate);
            PopulateSelectedTagsData(todoItemToUpdate);
            return View(todoItemToUpdate);
        }

        private void UpdateTodoTags(IEnumerable<string> selectedTags, TodoItem todoItem)
        {
            var newTags = new HashSet<string>(selectedTags);
            var currentTags = new HashSet<string>(todoItem.TagAssignments.Select(t => t.TagName));
            foreach (var tag in _context.Tags)
            {
                if (newTags.Contains(tag.Name))
                {
                    if (!currentTags.Contains(tag.Name))
                    {
                        todoItem.TagAssignments.Add(
                            new TagAssignment {TodoId = todoItem.Id, TagName = tag.Name});
                    }
                }
                else
                {
                    if (currentTags.Contains(tag.Name))
                    {
                        var tagAssignment = todoItem.TagAssignments.FirstOrDefault(t => t.TagName == tag.Name);
                        _context.Remove(tagAssignment);
                    }
                }
            }
        }

        // GET: TodoList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoItem = await GetTodoItemNoTracking(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            return View(todoItem);
        }

        // POST: TodoList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todoItem = await GetTodoItem(id);
            _context.RemoveRange(todoItem.TagAssignments);
            _context.TodoItem.Remove(todoItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

    public class CreateTodoViewModel
    {
        public IEnumerable<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
        public TodoItem TodoItem { get; set; }
    }
}
