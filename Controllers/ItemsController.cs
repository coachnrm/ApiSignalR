using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrudSignalR.Data;
using CrudSignalR.Hubs;
using CrudSignalR.Models;


namespace CrudSignalR.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IHubContext<ItemsHub> _hubContext;

        public ItemsController(MyDbContext context, IHubContext<ItemsHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return await _context.Items.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            // Notify clients about the new item
            await _hubContext.Clients.All.SendAsync("ItemAdded", item);

            return CreatedAtAction(nameof(GetItems), new { id = item.Id }, item);
        }

        // Implement other CRUD methods (PUT, DELETE) similarly
        // HttpPut method to update an existing item
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                // Notify clients about the updated item
                await _hubContext.Clients.All.SendAsync("ItemUpdated", item);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        // HttpDelete method to delete an item
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            // Notify clients about the deleted item
            await _hubContext.Clients.All.SendAsync("ItemDeleted", id);

            return NoContent();
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}