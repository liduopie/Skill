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
    public class ProjectPartakePersonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectPartakePersonController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: ProjectPartakePerson
        public async Task<IActionResult> Index(int? id)
        {

            if (id==null)
            {
                var applicationDbContext = _context.ProjectPartakePerson
                                       .Include(p => p.Person).Include(p => p.Project);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var applicationDbContext = _context.ProjectPartakePerson.Where(p => p.ProjectID == id)
                                       .Include(p => p.Person).Include(p => p.Project);
                return View(await applicationDbContext.ToListAsync());
            }
            
            

            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProjectPartakePerson/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectPartakePerson = await _context.ProjectPartakePerson
                .Include(p => p.Person)
                .Include(p => p.Project)
                .SingleOrDefaultAsync(m => m.ProjectID == id);
            if (projectPartakePerson == null)
            {
                return NotFound();
            }

            return View(projectPartakePerson);
        }

        // GET: ProjectPartakePerson/Create
        public IActionResult Create()
        {
            ViewData["PersonID"] = new SelectList(_context.Person, "Id", "Address");
            ViewData["ProjectID"] = new SelectList(_context.Project, "ID", "Name");
            return View();
        }

        // POST: ProjectPartakePerson/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectID,PersonID")] ProjectPartakePerson projectPartakePerson)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectPartakePerson);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["PersonID"] = new SelectList(_context.Person, "Id", "Address", projectPartakePerson.PersonID);
            ViewData["ProjectID"] = new SelectList(_context.Project, "ID", "Name", projectPartakePerson.ProjectID);
            return View(projectPartakePerson);
        }

        // GET: ProjectPartakePerson/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectPartakePerson = await _context.ProjectPartakePerson.SingleOrDefaultAsync(m => m.ProjectID == id);
            if (projectPartakePerson == null)
            {
                return NotFound();
            }
            ViewData["PersonID"] = new SelectList(_context.Person, "Id", "Address", projectPartakePerson.PersonID);
            ViewData["ProjectID"] = new SelectList(_context.Project, "ID", "Name", projectPartakePerson.ProjectID);
            return View(projectPartakePerson);
        }

        // POST: ProjectPartakePerson/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectID,PersonID")] ProjectPartakePerson projectPartakePerson)
        {
            if (id != projectPartakePerson.ProjectID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectPartakePerson);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectPartakePersonExists(projectPartakePerson.ProjectID))
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
            ViewData["PersonID"] = new SelectList(_context.Person, "Id", "Address", projectPartakePerson.PersonID);
            ViewData["ProjectID"] = new SelectList(_context.Project, "ID", "Name", projectPartakePerson.ProjectID);
            return View(projectPartakePerson);
        }

        // GET: ProjectPartakePerson/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectPartakePerson = await _context.ProjectPartakePerson
                .Include(p => p.Person)
                .Include(p => p.Project)
                .SingleOrDefaultAsync(m => m.ProjectID == id);
            if (projectPartakePerson == null)
            {
                return NotFound();
            }

            return View(projectPartakePerson);
        }

        // POST: ProjectPartakePerson/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectPartakePerson = await _context.ProjectPartakePerson.SingleOrDefaultAsync(m => m.ProjectID == id);
            _context.ProjectPartakePerson.Remove(projectPartakePerson);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProjectPartakePersonExists(int id)
        {
            return _context.ProjectPartakePerson.Any(e => e.ProjectID == id);
        }
    }
}
