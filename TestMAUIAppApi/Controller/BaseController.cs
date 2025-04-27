using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestMAUIAppApi.Data;

namespace TestMAUIAppApi.Controller
{
    public class BaseController<Tkey, T>(DataContext dataContext) : ControllerBase where T : class
    {
        private readonly DataContext dataContext = dataContext;

        protected string AuthorizationToken
        {
            get
            {
                string authorizationToken = string.Empty;

                var httpContext = HttpContext;
                if (httpContext != null)
                {
                    authorizationToken = httpContext.Request.Headers.Authorization.ToString();
                }

                return authorizationToken;
            }
        }

        // GET: api/<AuthorController>
        [HttpGet]
        public virtual async Task<ActionResult<List<T>>> GetAll()
        {
            if (string.IsNullOrWhiteSpace(AuthorizationToken))
            {
                return Unauthorized();
            }

            return Ok(await dataContext.Set<T>().AsNoTracking().ToListAsync());
        }

        // GET api/<AuthorController>/5
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<T>> GetById(Tkey id)
        {
            var record = await dataContext.Set<T>().FindAsync(id);

            if (record == null)
                return NotFound(record);

            return Ok(record);
        }

        // POST api/<AuthorController>
        [HttpPost]
        public virtual async Task<ActionResult<T>> Post([FromBody] T newRecord)
        {
            if (!ModelState.IsValid)
                return BadRequest(newRecord);


            await dataContext.Set<T>().AddAsync(newRecord);
            await dataContext.SaveChangesAsync();

            return Ok(newRecord);
        }

        // PUT api/<AuthorController>/5
        [HttpPut("{id}")]
        public virtual async Task<ActionResult<T>> Put(Tkey id, [FromBody] T updatedRecord)
        {
            try
            {
                dataContext.Entry(updatedRecord).State = EntityState.Modified;

                await dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
           
            return Ok(updatedRecord);
        }

        // DELETE api/<AuthorController>/5
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<T>> Delete(Tkey id)
        {
            var record = await dataContext.Set<T>().FindAsync(id);
            if (record == null)
                return NotFound(record);

            dataContext.Set<T>().Remove(record);
            await dataContext.SaveChangesAsync();

            return Ok(record);
        }
    }
}
