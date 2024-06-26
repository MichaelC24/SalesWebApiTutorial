using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebApiTutorial.Data;
using SalesWebApiTutorial.Models;

namespace SalesWebApiTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrders(int id)
        {
            var orders = await _context.Orders.FindAsync(id);

            if (orders == null)
            {
                return NotFound();
            }

            return orders;
        }
        // GET: api/orders/status/{status}
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrderByStatus(string status)
        {
            return await _context.Orders.Where(x => x.Status.ToUpper() == status).ToListAsync(); // try to use linq first before trying c# functions
            /*
            List<Order> newOrders = new List<Order>();
            var orders = await GetOrders();

            foreach (var item in orders.Value!)
            {
                if (item.Status.ToUpper() == "NEW")
                {

                    newOrders.Add(item);

                }
                                 
                //continue;
            }
            
            
            return newOrders;
            */ 
        }


        //  PUT: api/Orders/Shipped/5
        [HttpPut("shipped/{id}")] //tells http which method to use

        public async Task<IActionResult> ShippedOrder(int id, Order order)
        {
            order.Status = "Shipped"; //this sets the status of the order to Shift 
            return await PutOrders(id, order); // now we just await the execution of the PutOrders method and then return it
            
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrders(int id, Order orders)
        {
            if (id != orders.Id)
            {
                return BadRequest();
            }

            _context.Entry(orders).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) // stops 2 people from updating the same info at the same time similar to merge conflicts in Git. 
            {
                if (!OrdersExists(id)) 
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrders(Order orders)
        {
            orders.Status = "NEW"; // changes what ever is in the status column to NEW
            _context.Orders.Add(orders);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrders", new { id = orders.Id }, orders);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrders(int id)
        {
            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
