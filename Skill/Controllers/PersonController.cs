using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skill.Data;
using Skill.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

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
            string sortOrder, string currentFilter, string searchString, int? page)
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

            //查询全部的人的对象
            var personAll = from p in _context.Person
                            select new Person
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Age = p.Age,
                                Birthday = p.Birthday,
                                Hobby = p.Hobby,
                                Address = p.Address,
                                LabelCount = p.PersonMasterLabel.Count(),
                                ProjectCount = p.ProjectPartakePerson.Count()
                            };

            if (!String.IsNullOrEmpty(searchString))
            {
                personAll = personAll.Where(s => s.Name.Contains(searchString));
            }

            int pageSize = 8;

            return View(await PaginatedList<Person>.CreateAsync(personAll.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Person/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // 查询一个人的对象
            var person = await _context.Person
                .SingleOrDefaultAsync(m => m.Id == id);

            var labelAll = (from l in _context.Lable select l).ToList();
            ViewData["labelAll"] = labelAll;

            //查询这个人的掌握标签和参与项目的数量
            var masterLabelPartakeProject = from p in _context.Person
                                            where p.Id == id
                                            select new
                                            {
                                                Master = p.PersonMasterLabel.Count(),
                                                Partake = p.ProjectPartakePerson.Count()
                                            };

            //使用循环将结果放到人的对象中
            foreach (var item in masterLabelPartakeProject)
            {
                person.LabelCount = item.Master;
                person.ProjectCount = item.Partake;
            }

            //查询标签的使用以及次数
            var label = from pm in _context.PersonMasterLabel
                        join pu in _context.PersonUseLabel on pm.PersonID equals pu.PersonID
                        join c in _context.Lable on pm.LabelID equals c.Id
                        group pu by new { wna = pu.PersonID, sa = pm.LabelID, cname = c.Name } into g
                        select new
                        {
                            cname = g.Key.cname,
                            PersonID = g.Key.wna,
                            LabelID = g.Key.sa
                        };

            List<Label> labelList = new List<Label>();

            foreach (var item in label)
            {
                Label l = new Label();
                l.Id = item.LabelID;
                l.Name = item.cname;
                labelList.Add(l);
            }

            ViewData["LabelList"] = labelList;

            //查询这个人完成的项目
            var project = from p in _context.Person
                          join b in _context.ProjectPartakePerson on p.Id equals b.PersonID
                          join c in _context.Project on b.ProjectID equals c.ID
                          where p.Id == id
                          select new
                          {
                              c.Name
                          };

            //新建集合存储数据
            List<String> pr = new List<String>();

            //将项目名放入集合中
            foreach (var item in project)
            {
                pr.Add(item.Name);
            }

            //传递给视图
            ViewData["ProjectName"] = pr;

            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }
        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Details([FromBody]PersonMasterLabel personMasterLabel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.PersonMasterLabel.Add(personMasterLabel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PersonMasterLabelExists(personMasterLabel.PersonID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }

            //查询标签的使用以及次数
            var label = from pm in _context.PersonMasterLabel
                        join pu in _context.PersonUseLabel on pm.PersonID equals pu.PersonID
                        join c in _context.Lable on pm.LabelID equals c.Id
                        group pu by new { wna = pu.PersonID, sa = pm.LabelID, cname = c.Name } into g
                        select new
                        {
                            cname = g.Key.cname,
                            PersonID = g.Key.wna,
                            LabelID = g.Key.sa
                        };

            List<Label> labelList = new List<Label>();

            foreach (var item in label)
            {
                Label l = new Label();
                l.Id = item.LabelID;
                l.Name = item.cname;
                labelList.Add(l);
            }
            
            ViewData["LabelList"] = labelList;
            ViewData["Personid"] = personMasterLabel.PersonID;
            return PartialView("_PartialPage", ViewData["Personid"]);
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
                ViewData["edit"] = null;
            }

            var person = await _context.Person.SingleOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                ViewData["edit"] = null;
            }
            ViewData["edit"] = person;
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

        private bool PersonMasterLabelExists(int id)
        {
            return _context.PersonMasterLabel.Any(e => e.PersonID == id);
        }
    }
}
