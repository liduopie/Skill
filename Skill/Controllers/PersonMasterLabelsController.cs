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
    public class PersonMasterLabelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonMasterLabelsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: PersonMasterLabels
        public async Task<IActionResult> Index(
    string sortOrder,
    string currentFilter,
    string searchString,
    int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var applicationDbContext = _context.PersonMasterLabel.Include(p => p.Label).Include(p => p.Person);
            //var master = from a in _context.PersonMasterLabel
            //                           join b in _context.Lable on a.LabelID equals b.Id
            //                           join c in _context.Person on a.PersonID equals c.Id
            //                           select new
            //                           {
            //                               BName = b.Name,
            //                               CName = c.Name
            //                           };

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    applicationDbContext = applicationDbContext.Where(s => s.Person.Name.Contains(searchString));
            //}
            int pageSize = 8;
            return View(await PaginatedList<PersonMasterLabel>.CreateAsync(applicationDbContext.AsNoTracking(), page ?? 1, pageSize));

            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: PersonMasterLabels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personMasterLabel = await _context.PersonMasterLabel
                .Include(p => p.Label)
                .Include(p => p.Person)
                .SingleOrDefaultAsync(m => m.PersonID == id);
            if (personMasterLabel == null)
            {
                return NotFound();
            }

            return View(personMasterLabel);
        }

        // GET: PersonMasterLabels/Create
        public IActionResult Create()
        {
            ViewData["LabelID"] = new SelectList(_context.Lable, "Id", "Name");
            ViewData["PersonID"] = new SelectList(_context.Person, "Id", "Name");
            return View();
        }

        // POST: PersonMasterLabels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonID,LabelID")] PersonMasterLabel personMasterLabel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personMasterLabel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["LabelID"] = new SelectList(_context.Lable, "Id", "Name", personMasterLabel.LabelID);
            ViewData["PersonID"] = new SelectList(_context.Person, "Id", "Name", personMasterLabel.PersonID);
            return View(personMasterLabel);
        }

        //// GET: PersonMasterLabels/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var personMasterLabel = await _context.PersonMasterLabel.SingleOrDefaultAsync(m => m.PersonID == id);
        //    if (personMasterLabel == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["LabelID"] = new SelectList(_context.Lable, "Id", "Name", personMasterLabel.LabelID);
        //    ViewData["PersonID"] = new SelectList(_context.Person, "Id", "Name", personMasterLabel.PersonID);
        //    return View(personMasterLabel);
        //    //ViewData["LabelID"] = new SelectList(_context.Lable, "Id", "Name");
        //    //ViewData["PersonID"] = new SelectList(_context.Person, "Id", "Name");
        //    //return View();
        //}

        //// POST: PersonMasterLabels/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("PersonID,LabelID")] PersonMasterLabel personMasterLabel)
        //{
        //    if (id != personMasterLabel.PersonID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(personMasterLabel);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PersonMasterLabelExists(personMasterLabel.PersonID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    ViewData["LabelID"] = new SelectList(_context.Lable, "Id", "Name", personMasterLabel.LabelID);
        //    ViewData["PersonID"] = new SelectList(_context.Person, "Id", "Name", personMasterLabel.PersonID);
        //    return View(personMasterLabel);
        //}

        // GET: PersonMasterLabels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personMasterLabel = await _context.PersonMasterLabel
                .Include(p => p.Label)
                .Include(p => p.Person)
                .SingleOrDefaultAsync(m => m.PersonID == id);
            if (personMasterLabel == null)
            {
                return NotFound();
            }

            return View(personMasterLabel);
        }

        // POST: PersonMasterLabels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personMasterLabel = await _context.PersonMasterLabel.SingleOrDefaultAsync(m => m.PersonID == id);
            _context.PersonMasterLabel.Remove(personMasterLabel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PersonMasterLabelExists(int id)
        {
            return _context.PersonMasterLabel.Any(e => e.PersonID == id);
        }
    }
}
