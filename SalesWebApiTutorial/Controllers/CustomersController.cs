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
            var customer = await _context.Customers.FindAsync(id); // makes a variable the waits until the FindAsync returns data

            if (customer == null) //checks to make sure customer contains data
            {
                return NotFound(); //returns error 404
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPut("{id}")] // tells c# that we are going to use the PutCustomer method
        public async Task<IActionResult> PutCustomer(int id, Customer customer) //updates customer only uses IActionResult because this doesnt return any data.
        {
            if (id != customer.Id) //searchs for the customer
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified; // forces whatever data is inputted into the customer varaible to appear as modified. 

            try
            {
                await _context.SaveChangesAsync(); // trys to save the changes
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

            return NoContent(); // if this is returned means the update worked 
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost] 
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer) //adds customer and since Customer is in the ActionResult it means it can return data 
        {
            _context.Customers.Add(customer); //add is like staging in GIT

            await _context.SaveChangesAsync(); //actually causes the database to change.

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer); //uses the GetCustomer method then passes in a id
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")] //this id has to match the one that the method takes in and the property in your list of properties.
        public async Task<IActionResult> DeleteCustomer(int id) //Deletes customer
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer); //add the delete the row you specfied with the customerId
            await _context.SaveChangesAsync(); //actually deletes the row you specified with the customerId

            return NoContent();
        }

        private bool CustomerExists(int id) 
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
