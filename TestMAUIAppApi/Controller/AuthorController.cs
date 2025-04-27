using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using SharedModels.Models.DTO.OutputDTO;
using TestMAUIAppApi.Data;

namespace TestMAUIAppApi.Controller;

//[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]

public class AuthorController(DataContext dataContext) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<AuthorOutputDTO>>> GetAll()
    {
        var authors = await (from author in dataContext.Authors.AsNoTracking()
                             select new AuthorOutputDTO
                             {
                                 Id = author.Id,
                                 AuthorFullName = $"{author.FirstName} {author.LastName}"
                             }).ToListAsync();
        return Ok(authors);
    }

    // GET api/<AuthorController>/5
    [HttpGet("{id}")]
    public virtual async Task<ActionResult<Author>> GetById(int id)
    {
        var record = await dataContext.Authors.FindAsync(id);

        if (record == null)
            return NotFound(record);

        return Ok(record);
    }

    // POST api/<AuthorController>
    [HttpPost]
    public virtual async Task<ActionResult<Author>> Post([FromBody] Author newRecord)
    {
        if (!ModelState.IsValid)
            return BadRequest(newRecord);


        await dataContext.Authors.AddAsync(newRecord);
        await dataContext.SaveChangesAsync();

        return Ok(newRecord);
    }

    // PUT api/<AuthorController>/5
    [HttpPut("{id}")]
    public virtual async Task<ActionResult<Author>> Put(int id, [FromBody] Author updatedRecord)
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
    public virtual async Task<ActionResult<Author>> Delete(int id)
    {
        var record = await dataContext.Authors.FindAsync(id);
        if (record == null)
            return NotFound(record);

        dataContext.Authors.Remove(record);
        await dataContext.SaveChangesAsync();

        return Ok(record);
    }
}
