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
    [Route("api/[controller]")] //http://localhost:5000/api/customers/
                                //^-------------------^ points to the application
                                //                    ^^------------^^ points to the location within the application
    [ApiController] //tells c# which type of controller to use
    public class CustomersController : ControllerBase //ControllerBase provides the controller type
    {
        private readonly AppDbContext _context; // readonly means you can initialize when you create it or when you use it in a constructor

        public CustomersController(AppDbContext context) //Constructor initializes the _connect variable
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet] // its an Http Method it identifies what method is going to be called
                  //there is a few methods that fall under http 
                  //Get: Reading Data
                  //Post: Add Data
                  //Put: Change Data
                  //Delete: Removes data
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer() //gets all customers 
                                                                             //Task: its the class tells you when the data is returned
                                                                             //ActionResult: its a class that has a bunch of different responses in this case its not doing anything but you can build in logic to return a message if there is an error
                                                                             //IEnumerable: is an interface to make sure all the data that comes through is the type of Customer and it has to be a collection
        {
            return await _context.Customers.ToListAsync(); // wont return data until its done pulling all of the data and saving it to a list
        }

        // GET: api/Customers/5
        [HttpGet("{id}")] //this one is adding in the id onto the end of the http link http://localhost:5000/api/customers/5
        public async Task<ActionResult<Customer>> GetCustomer(int id) //gets rid of IEnumerable because it doesnt return a list
        {
            var customer = await _context.Customers.FindAsync(id); 

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")] 
        public async Task<IActionResult> PutCustomer(int id, Customer customer) //updates customer
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)//add customer
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id) //Deletes customer
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id) 
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
