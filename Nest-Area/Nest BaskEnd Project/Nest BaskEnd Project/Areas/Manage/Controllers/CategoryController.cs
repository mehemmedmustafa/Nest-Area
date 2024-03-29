﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest_BaskEnd_Project.DAL;
using Nest_BaskEnd_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nest_BaskEnd_Project.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
        private AppDbContext _context { get;  }
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories.Include(c=>c.Products).ToListAsync();
            return View(categories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (_context.Categories.FirstOrDefault(c => c.Name.ToLower().Trim() == category.Name.ToLower().Trim()) != null) return RedirectToAction(nameof(Index));
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id )
        {
           Category categories = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (categories == null) return NotFound();
            return View(categories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            Category existcategory = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (existcategory == null) return NotFound();
            existcategory.Logo = category.Logo;
            existcategory.Name = category.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public  IActionResult Delete(int id)
        {
            Category category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            category.IsDeleted = true; 
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult PermaDelete(int id)
        {
            Category category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Repair(int id)
        {
            Category category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            category.IsDeleted = false;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
