using Financial_Tamkeen.EmployeeManagement.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for em  pty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Financial_Tamkeen.EmployeeManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly AppDbContex _DbContext;
        private readonly JsonSerializerOptions _jsonOptions;

        public EmployeeController(AppDbContex DbContext)
        {
            this._DbContext = DbContext;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                IgnoreNullValues = true,
                MaxDepth = 10  // Specify the maximum depth to avoid excessive nesting
            };

        }
        // GET: api/<EmployeeController>
        [HttpGet("GetAllEmployee")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _DbContext.Employee
            .ToListAsync();

            var serializedEmployees = System.Text.Json.JsonSerializer.Serialize(employees, _jsonOptions);

            return Content(serializedEmployees, "application/json");

            //    return Ok(employees);
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{GetEmployeeId}")]
        public async Task<ActionResult<Employee>>GetEmployeeId(int id)
        {

            var employee = await _DbContext.Employee
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            var serializedEmployees = System.Text.Json.JsonSerializer.Serialize(employee, _jsonOptions);

            return Content(serializedEmployees, "application/json");
        }


        [Authorize]
        // POST api/<EmployeeController>
        [HttpPost("CreateNEwEmployee")]
        
        public async Task<ActionResult<Employee>> Post(Employee Emp)
        {
            var employee =await this._DbContext.Employee.AddAsync(Emp);
            await this._DbContext.SaveChangesAsync();   
            return  Ok(employee);

        }

        // PUT api/<EmployeeController>/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            _DbContext.Entry(employee).State = EntityState.Modified;

            try
            {
                await _DbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_DbContext.Employee.Any(e => e.EmployeeId == id))
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

        // DELETE api/<EmployeeController>/5
        [Authorize]
        [HttpDelete("{DeleteEmployeeid}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _DbContext.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _DbContext.Employee.Remove(employee);
            await _DbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
