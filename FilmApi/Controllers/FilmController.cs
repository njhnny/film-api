using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmApi.Models;

namespace FilmApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FilmController : ControllerBase
  {
    private readonly FilmContext _db;

    public FilmController(FilmContext db)
    {
      _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Film>>> Get()
    {
      return await _db.Films.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Film>> GetFilm(int id)
    {
      var film = await _db.Films.FindAsync(id);

      if (film == null)
      {
        return NotFound();
      }

      return film;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Film film)
    {
      if (id != film.FilmId)
      {
        return BadRequest();
      }

      _db.Entry(film).State = EntityState.Modified;

      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!FilmExists(id))
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

    [HttpPost]
    public async Task<ActionResult<Film>> Post(Film film)
    {
      _db.Films.Add(film);
      await _db.SaveChangesAsync();

      return CreatedAtAction("Post", new { id = film.FilmId }, film);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFilm(int id)
    {
      var film = await _db.Films.FindAsync(id);
      if (film == null)
      {
        return NotFound();
      }

      _db.Films.Remove(film);
      await _db.SaveChangesAsync();

      return NoContent();
    }

    private bool FilmExists(int id)
    {
      return _db.Films.Any(e => e.FilmId == id);
    }
  }
}
