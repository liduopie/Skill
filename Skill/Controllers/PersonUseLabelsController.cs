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
    public class PersonUseLabelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonUseLabelsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: PersonUseLabels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PersonUseLabel.Include(p => p.Lable).Include(p => p.Person);
            return View(await applicationDbContext.ToListAsync());
        }
        // GET: PersonUseLabels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personUseLabel = await _context.PersonUseLabel
                .Include(p => p.Lable)
                .Include(p => p.Person)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (personUseLabel == null)
            {
                return NotFound();
            }

            return View(personUseLabel);
        }

        // GET: PersonUseLabels/Create
        public IActionResult Create()
        {
            ViewData["LabelID"] = new SelectList(_context.Lable, "Id", "Name");
            ViewData["PersonID"] = new SelectList(_context.Person, "Id", "Name");
            return View();
        }

        // POST: PersonUseLabels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PersonID,LabelID,UserTime")] PersonUseLabel personUseLabel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personUseLabel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["LabelID"] = new SelectList(_context.Lable, "Id", "Name", personUseLabel.LabelID);
            ViewData["PersonID"] = new SelectList(_context.Person, "Id", "Name", personUseLabel.PersonID);
            return View(personUseLabel);
        }

        // GET: PersonUseLabels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personUseLabel = await _context.PersonUseLabel.SingleOrDefaultAsync(m => m.ID == id);
            if (personUseLabel == null)
            {
                return NotFound();
            }
            ViewData["LabelID"] = new SelectList(_context.Lable, "Id", "Name", personUseLabel.LabelID);
            ViewData["PersonID"] = new SelectList(_context.Person, "Id", "Name", personUseLabel.PersonID);
            return View(personUseLabel);
        }

        // POST: PersonUseLabels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PersonID,LabelID,UserTime")] PersonUseLabel personUseLabel)
        {
            if (id != personUseLabel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personUseLabel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonUseLabelExists(personUseLabel.ID))
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
            ViewData["LabelID"] = new SelectList(_context.Lable, "Id", "Name", personUseLabel.LabelID);
            ViewData["PersonID"] = new SelectList(_context.Person, "Id", "Name", personUseLabel.PersonID);
            return View(personUseLabel);
        }

        // GET: PersonUseLabels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personUseLabel = await _context.PersonUseLabel
                .Include(p => p.Lable)
                .Include(p => p.Person)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (personUseLabel == null)
            {
                return NotFound();
            }

            return View(personUseLabel);
        }

        // POST: PersonUseLabels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personUseLabel = await _context.PersonUseLabel.SingleOrDefaultAsync(m => m.ID == id);
            _context.PersonUseLabel.Remove(personUseLabel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PersonUseLabelExists(int id)
        {
            return _context.PersonUseLabel.Any(e => e.ID == id);
        }
    }
}
