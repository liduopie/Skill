using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skill.Data;
using Skill.Models;

namespace Skill.Controllers
{
    [Produces("application/json")]
    [Route("api/PersonMasterLabels1")]
    public class PersonMasterLabels1Controller : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonMasterLabels1Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PersonMasterLabels1
        [HttpGet]
        public IEnumerable<PersonMasterLabel> GetPersonMasterLabel()
        {
            return _context.PersonMasterLabel;
        }

        // GET: api/PersonMasterLabels1/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonMasterLabel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var personMasterLabel = await _context.PersonMasterLabel.SingleOrDefaultAsync(m => m.PersonID == id);

            if (personMasterLabel == null)
            {
                return NotFound();
            }

            return Ok(personMasterLabel);
        }

        // PUT: api/PersonMasterLabels1/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonMasterLabel([FromRoute] int id, [FromBody] PersonMasterLabel personMasterLabel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != personMasterLabel.PersonID)
            {
                return BadRequest();
            }

            _context.Entry(personMasterLabel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonMasterLabelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PersonMasterLabels1
        [HttpPost]
        public async Task<IActionResult> PostPersonMasterLabel([FromBody] PersonMasterLabel personMasterLabel)
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
                    throw;
                }
            }

            return CreatedAtAction("GetPersonMasterLabel", new { id = personMasterLabel.PersonID }, personMasterLabel);
        }

        // DELETE: api/PersonMasterLabels1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonMasterLabel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var personMasterLabel = await _context.PersonMasterLabel.SingleOrDefaultAsync(m => m.PersonID == id);
            if (personMasterLabel == null)
            {
                return NotFound();
            }

            _context.PersonMasterLabel.Remove(personMasterLabel);
            await _context.SaveChangesAsync();

            return Ok(personMasterLabel);
        }

        private bool PersonMasterLabelExists(int id)
        {
            return _context.PersonMasterLabel.Any(e => e.PersonID == id);
        }
    }
}