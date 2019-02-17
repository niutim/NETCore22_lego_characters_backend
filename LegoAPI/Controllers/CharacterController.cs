using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LegoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LegoAPI.Controllers
{
    [Route("api/character")]
    public class CharacterController : ControllerBase
    {
        private readonly CharacterContext _context;

        public CharacterController(CharacterContext context)
        {
            _context = context;

            if (_context.CharacterItems.Count() == 0)
            {
                // Create a new Character item if collection is empty,
                // which means you can't delete all CharacterItems.
                _context.CharacterItems.Add(new CharacterItem { Name = "Emmet", BasedOn="The Lego movie" });
                _context.CharacterItems.Add(new CharacterItem { Name = "CoolTag", BasedOn="The Lego movie" });
                _context.CharacterItems.Add(new CharacterItem { Name = "Vitruvius", BasedOn="The Lego movie" });
                _context.CharacterItems.Add(new CharacterItem { Name = "StarLord", BasedOn = "Marvel / Lego Avengers" });
                _context.CharacterItems.Add(new CharacterItem { Name = "Nebula", BasedOn = "Marvel / Lego Avengers" });
                _context.CharacterItems.Add(new CharacterItem { Name = "Batman", BasedOn="DC Comics / Lego Batman" });
                _context.CharacterItems.Add(new CharacterItem { Name = "Joker", BasedOn = "DC Comics / Lego Batman" });
                _context.CharacterItems.Add(new CharacterItem { Name = "Bilbo Sacket", BasedOn = "The Hobbit" });
                _context.CharacterItems.Add(new CharacterItem { Name = "Frodo Sacket", BasedOn = "Lord of the ring" });
                _context.CharacterItems.Add(new CharacterItem { Name = "Gollum", BasedOn = "The Hobbit / Lord of the ring" });
                _context.CharacterItems.Add(new CharacterItem { Name = "Chase McCain", BasedOn = "Lego city" });
                _context.SaveChanges();
            }
        }

        // GET: api/character
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacterItem>>> GetCharacterItems()
        {
            return await _context.CharacterItems.ToListAsync();
        }

        // GET: api/Character/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterItem>> GetCharacterItem(long id)
        {
            var characterItem = await _context.CharacterItems.FindAsync(id);

            if (characterItem == null)
            {
                return NotFound();
            }

            return characterItem;
        }

        // POST: api/character
        [HttpPost]
        public async Task<ActionResult<CharacterItem>> PostCharacterItem(CharacterItem item)
        {
            _context.CharacterItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCharacterItem), new { id = item.Id }, item);
        }

        // PUT: api/character/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacterItem(long id, CharacterItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/character/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacterItem(long id)
        {
            var todoItem = await _context.CharacterItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.CharacterItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}