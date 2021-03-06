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
    public class LabelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LabelController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Label
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

            var label = from s in _context.Lable
                         select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                label = label.Where(s => s.Name.Contains(searchString));
            }
            int pageSize = 8;
            return View(await PaginatedList<Label>.CreateAsync(label.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Label/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var label = await _context.Lable
                .SingleOrDefaultAsync(m => m.Id == id);
            var person = from p in _context.Person//查询人员使用标签次数
                         join b in _context.PersonUseLabel on p.Id equals b.PersonID
                         join c in _context.Lable on b.LabelID equals c.Id
                         where c.Id == id
                         group b by new { wna = b.PersonID, sa = b.LabelID, sn = p.Name } into g
                         select new
                         {
                             Id = g.Key.wna,
                             Name = g.Key.sn,
                             LabelUseCount = g.Count()
                         };
            List<Person> pp = new List<Person>();
            foreach (var item in person)
            {
                Person p = new Person();
                p.Id = item.Id;
                p.Name = item.Name;
                p.LabelUseCount = item.LabelUseCount;
                pp.Add(p);
            }
            ViewData["PP"] = pp;
            if (label == null)
            {
                return NotFound();
            }
            return View(label);
        }

        // GET: Label/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Label/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Synopsis,Applicable,Category")] Label label)
        {
            if (ModelState.IsValid)
            {
                _context.Add(label);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(label);
        }

        // GET: Label/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var label = await _context.Lable.SingleOrDefaultAsync(m => m.Id == id);
            if (label == null)
            {
                return NotFound();
            }
            return View(label);
        }

        // POST: Label/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Synopsis,Applicable,Category")] Label label)
        {
            if (id != label.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(label);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabelExists(label.Id))
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
            return View(label);
        }

        // GET: Label/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var label = await _context.Lable
                .SingleOrDefaultAsync(m => m.Id == id);
            if (label == null)
            {
                return NotFound();
            }

            return View(label);
        }

        // POST: Label/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var label = await _context.Lable.SingleOrDefaultAsync(m => m.Id == id);
            _context.Lable.Remove(label);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool LabelExists(int id)
        {
            return _context.Lable.Any(e => e.Id == id);
        }
    }
}
