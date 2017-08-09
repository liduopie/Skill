using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skill.Data;
using Skill.Models;
using System.Collections;
using System.Collections.Generic;

namespace Skill.Controllers
{
    public class PersonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Person
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
            var ps = from p in _context.Person//查询全部的人的对象
                     select new Person
                     {
                         Id=p.Id,
                         Name=p.Name,
                         Age=p.Age,
                         Birthday=p.Birthday,
                         Hobby=p.Hobby,
                         Address=p.Address,
                         LabelCount = p.PersonMasterLabel.Count(),
                         ProjectCount=p.ProjectPartakePerson.Count()
                     };
            if (!String.IsNullOrEmpty(searchString))
            {
                ps = ps.Where(s => s.Name.Contains(searchString));
            }
              int pageSize = 8;
            return View(await PaginatedList<Person>.CreateAsync(ps.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Person/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var person = await _context.Person//查询一个人的对象
    .SingleOrDefaultAsync(m => m.Id == id);
            var ps = from p in _context.Person//查询这个人的掌握和参与项目的数量
                     where p.Id==id
                     select new
                     {
                         Master = p.PersonMasterLabel.Count(),
                         Partake =p.ProjectPartakePerson.Count()
                     };
            foreach(var item in ps)//使用循环将结果放到人的对象中
            {
                person.LabelCount = item.Master;
                person.ProjectCount = item.Partake;
            }
            var label = from p in _context.Person//查询标签的使用以及次数
                    join b in _context.PersonUseLabel on p.Id equals b.PersonID
                    join c in _context.Lable on b.LabelID equals c.Id
                    where p.Id == 1
                    group b by new { wna = b.PersonID, sa = b.LabelID, sn = c.Name } into g
                    select new 
                    {
                        Id = g.Key.sa,
                        Name = g.Key.sn,
                        LabelUseCount = g.Count()
                    };
            List<Label> ll = new List<Label>();
            foreach (var item in label)
            {
                Label l = new Label();
                l.Id = item.Id;
                l.Name = item.Name;
                l.LabelUseCount = item.LabelUseCount;
                ll.Add(l);
            }
            ViewData["LL"] = ll;
            var project = from p in _context.Person//查询这个人掌握的项目
                          join b in _context.ProjectPartakePerson on p.Id equals b.PersonID
                          join c in _context.Project on b.ProjectID equals c.ID
                          where p.Id == id
                          select new
                          {
                              c.Name
                          };
            List<String> pr = new List<String>();//新建集合存储数据
            foreach (var item in project)//将项目名放入集合中
            {
                pr.Add(item.Name);
            }
            ViewData["ProjectName"] = pr;//传递给视图
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }
        // GET: Person/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Person/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Age,Birthday,Hobby,Phone,Address")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person.SingleOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: Person/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,Birthday,Hobby,Phone,Address")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
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
            return View(person);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Person
                .SingleOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Person.SingleOrDefaultAsync(m => m.Id == id);
            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PersonExists(int id)
        {
            return _context.Person.Any(e => e.Id == id);
        }
    }
}
