using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TODOListApp.Data;
using TODOListApp.Models;

namespace TODOListApp.Controllers
{
    public class TagsController : Controller
    {
        private readonly AppDbContext _dbContext;

        public TagsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Create()
        {
            var tags = await _dbContext.Tags.ToListAsync();
            return View(new TagsViewModel {AvailableTags = tags});
        }

        [HttpPost]
        public async Task<IActionResult> Create(Tag newTag)
        {
            if (!ModelState.IsValid) return View(new TagsViewModel { AvailableTags = await _dbContext.Tags.ToListAsync() , NewTag = newTag});
            _dbContext.Tags.Add(newTag);
            await _dbContext.SaveChangesAsync();
            ModelState.Clear();
            return View(new TagsViewModel { AvailableTags = await _dbContext.Tags.ToListAsync() });
        }
    }
}