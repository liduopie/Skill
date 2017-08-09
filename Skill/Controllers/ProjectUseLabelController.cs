using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Skill.Data;
using Skill.Models;

namespace Skill.Controllers
{
    public class ProjectUseLabelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectUseLabelController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: ProjectUseLabel
        public async Task<IActionResult> Index(int? id)
        {
            if (id==null)
            {
                var applicationDbContext = _context.ProjectUseLabel.Include(p => p.Label).Include(p => p.Project);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.ProjectUseLabel.Where(p => p.ProjectID == id).Include(p => p.Label).Include(p => p.Project);
                return View(await applicationDbContext.ToListAsync());
            }
            
        }

        // GET: ProjectUseLabel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectUseLabel = await _context.ProjectUseLabel
                .Include(p => p.Label)
                .Include(p => p.Project)
                .SingleOrDefaultAsync(m => m.ProjectID == id);
            if (projectUseLabel == null)
            {
                return NotFound();
            }

            return View(projectUseLabel);
        }

        // GET: ProjectUseLabel/Create
        public IActionResult Create()
        {
            ViewData["LabelID"] = new SelectList(_context.Lable, "Id", "Applicable");
            ViewData["ProjectID"] = new SelectList(_context.Project, "ID", "Name");
            return View();
        }

        // POST: ProjectUseLabel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectID,LabelID")] ProjectUseLabel projectUseLabel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectUseLabel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["LabelID"] = new SelectList(_context.Lable, "Id", "Applicable", projectUseLabel.LabelID);
            ViewData["ProjectID"] = new SelectList(_context.Project, "ID", "Name", projectUseLabel.ProjectID);
            return View(projectUseLabel);
        }

        // GET: ProjectUseLabel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectUseLabel = await _context.ProjectUseLabel.SingleOrDefaultAsync(m => m.ProjectID == id);
            if (projectUseLabel == null)
            {
                return NotFound();
            }
            ViewData["LabelID"] = new SelectList(_context.Lable, "Id", "Applicable", projectUseLabel.LabelID);
            ViewData["ProjectID"] = new SelectList(_context.Project, "ID", "Name", projectUseLabel.ProjectID);
            return View(projectUseLabel);
        }

        // POST: ProjectUseLabel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectID,LabelID")] ProjectUseLabel projectUseLabel)
        {
            if (id != projectUseLabel.ProjectID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectUseLabel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectUseLabelExists(projectUseLabel.ProjectID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["LabelID"] = new SelectList(_context.Lable, "Id", "Applicable", projectUseLabel.LabelID);
            ViewData["ProjectID"] = new SelectList(_context.Project, "ID", "Name", projectUseLabel.ProjectID);
            return View(projectUseLabel);
        }

        // GET: ProjectUseLabel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectUseLabel = await _context.ProjectUseLabel
                .Include(p => p.Label)
                .Include(p => p.Project)
                .SingleOrDefaultAsync(m => m.ProjectID == id);
            if (projectUseLabel == null)
            {
                return NotFound();
            }

            return View(projectUseLabel);
        }

        // POST: ProjectUseLabel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectUseLabel = await _context.ProjectUseLabel.SingleOrDefaultAsync(m => m.ProjectID == id);
            _context.ProjectUseLabel.Remove(projectUseLabel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProjectUseLabelExists(int id)
        {
            return _context.ProjectUseLabel.Any(e => e.ProjectID == id);
        }
    }
}
