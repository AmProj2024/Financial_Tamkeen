using Financial_Tamkeen.EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Financial_Tamkeen.EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly AppDbContex _DbContext;
        public EmployeeController(AppDbContex DbContext)
        {
            this._DbContext = DbContext;


        }
        // GET: api/<EmployeeController>
        [HttpGet("GetAllEmployee")]
        public async Task<ActionResult<Employee>> GetAllEmployee()
        {
            //   return new string[] { "value1", "value2" };

            var Employess = await this._DbContext.Employee.ToListAsync();
            return Ok(Employess);
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{GetEmployeeId}")]
        public async Task<ActionResult<Employee>>GetEmployeeId(int id)
        {

            var Employee = await this._DbContext.Employee.FirstOrDefaultAsync(x=>x.EmployeeId == id);
            return Ok(Employee);
            // return "value";
        }



        // POST api/<EmployeeController>
        [HttpPost("CreateNEwEmployee")]
        
        public async Task<ActionResult<Employee>> Post(Employee Emp)
        {
            var employee =await this._DbContext.Employee.AddAsync(Emp);
            await this._DbContext.SaveChangesAsync();   
            return  Ok(employee);

        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {


        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{DeleteEmployeeid}")]
        public  void Delete(int id)
        {
            var Emp = this._DbContext.Employee.FirstOrDefault(x=>x.EmployeeId==id);
            if (Emp!=null)
            {
                var lll =  _DbContext.Employee.Update(Emp);

            }
            this._DbContext.SaveChanges();

        }
    }
}
