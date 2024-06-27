using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebApiTutorial.Data;
using SalesWebApiTutorial.Models;
///
/// ORDERLINES IS ALL OF THE ITEMS AND THEIR QUANTITY AND IT ALSO TELLS US WHICH ORDER THEY ARE CONNECTED TO.
///

namespace SalesWebApiTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderlinesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderlinesController(AppDbContext context)
        {
            _context = context;
        }
        private async Task<IActionResult> Total(int id) // IActionResult means that we arent returning any data only messages like 404,500,200
        {
            var order = await _context.Orders.FindAsync(id); //finds a order that is passed in and saves it to the order variable

            if (order == null) //checks to make sure that the above returned data.
            {
                return NotFound();
            }

            order.Total = (from ol in _context.OrderLines

                           join i in _context.Items
                              on ol.ItemId equals i.Id
                           where ol.Id == id
                           select (ol.Quantity * i.Price)).Sum();

            //dont need this because the changes are being tracked since we read the order and its in the cache //_context.Entry(order).State = EntityState.Modified; // this takes the your entry "order" and changes its state to modified
            // so even if they rerun this for one id and the total remains the same it will still 
            //return data if we didnt have this it would throw an error

            await _context.SaveChangesAsync();
            return Ok();
        }
        // GET: api/Orderlines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orderline>>> GetOrderLines()
        {
            return await _context.OrderLines.ToListAsync();
        }

        // GET: api/Orderlines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Orderline>> GetOrderline(int id)
        {
            var orderline = await _context.OrderLines.FindAsync(id);

            if (orderline == null)
            {
                return NotFound();
            }

            return orderline;
        }

        // PUT: api/Orderlines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderline(int id, Orderline orderline)
        {
            if (id != orderline.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderline).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await Total(orderline.Id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderlineExists(id))
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

        // POST: api/Orderlines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Orderline>> PostOrderline(Orderline orderline)
        {
            _context.OrderLines.Add(orderline);
            await _context.SaveChangesAsync();
            await Total(orderline.Id);

            return CreatedAtAction("GetOrderline", new { id = orderline.Id }, orderline);
        }

        // DELETE: api/Orderlines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderline(int id)
        {
            var orderline = await _context.OrderLines.FindAsync(id);
            if (orderline == null)
            {
                return NotFound();
            }

            _context.OrderLines.Remove(orderline);
            await _context.SaveChangesAsync();
            await Total(orderline.Id);

            return NoContent();
        }
       

        private bool OrderlineExists(int id)
        {
            return _context.OrderLines.Any(e => e.Id == id); //checks to see if there is any data in the id column and then
                                                             //the method will return true if there is data there
        }
    }
}
