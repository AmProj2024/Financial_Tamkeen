using Financial_Tamkeen.EmployeeManagement.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

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
        private readonly IConfiguration _configuration;

        public EmployeeController(AppDbContex DbContext, IConfiguration configuration)
        {
            this._DbContext = DbContext;
            _jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                IgnoreNullValues = true,
                MaxDepth = 10  // Specify the maximum depth to avoid excessive nesting
            };
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet("GetAllEmployee")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            try
            {
                // Retrieve the token from the request headers
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                // Validate the token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key"));
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration.GetValue<string>("Jwt:Issuer"),
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                // Validate the token and extract claims
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                var claims = claimsPrincipal.Claims;

                //  to retrieve all employees
                var employees = await _DbContext.Employee.ToListAsync();

                var serializedEmployees = System.Text.Json.JsonSerializer.Serialize(employees, _jsonOptions);

                return Content(serializedEmployees, "application/json");
            }
            catch (Exception)
            {
                return Unauthorized(); // Return Unauthorized if the token is invalid or expired
            }
        }
        
        [AllowAnonymous]
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

        // POST api/<EmployeeController>
        [Authorize]
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
