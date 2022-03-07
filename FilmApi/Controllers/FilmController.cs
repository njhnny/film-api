using System.Collections.Generic;
using System.Threading.Tasks;
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


    [HttpPost]
    public async Task<ActionResult<Film>> Post(Film film)
    {
      _db.Films.Add(film);
      await _db.SaveChangesAsync();

      return CreatedAtAction("Post", new { id = film.FilmId }, film);
    }
  }
}
