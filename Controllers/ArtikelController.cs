using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RESTApi.Models;

namespace RESTApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtikelController : ControllerBase
    {
        private readonly ArtDBContext _context;

        public ArtikelController(ArtDBContext context)
        {
            _context = context;
        }

        // GET: api/Artikel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artikel>>> GetArtikels(string naam, int? vanaf, int? tot, Kleur? kleur,ProductGroep? productGroep, string sorteer, int pagina)
        {
            return await Pagineer(Sorteer(FilterOpProductGroep(FilterOpKleur(FilterOpPotMaat(FilterOpNaam(_context.Artikels, naam), vanaf, tot), kleur), productGroep), sorteer), pagina, 5).ToListAsync();
        }

        // GET: api/Artikel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Artikel>> GetArtikel(string id)
        {
            var artikel = await _context.Artikels.FindAsync(id);

            if (artikel == null)
            {
                return NotFound();
            }

            return artikel;
        }

        // PUT: api/Artikel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtikel(string id, Artikel artikel)
        {
            if(ModelState.IsValid)
            {
                if (id != artikel.Code)
                {
                    return BadRequest();
                }

                _context.Entry(artikel).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtikelExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return NoContent();
        }

        // POST: api/Artikel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Artikel>> PostArtikel(Artikel artikel)
        {
            if(ModelState.IsValid)
            {
                if(artikel.Code == null)
                {
                    artikel.Code = RandomCode();
                }
                _context.Artikels.Add(artikel);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (ArtikelExists(artikel.Code))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return CreatedAtAction(nameof(GetArtikel), new { id = artikel.Code }, artikel);
        }

        // DELETE: api/Artikel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtikel(string id)
        {
            var artikel = await _context.Artikels.FindAsync(id);
            if (artikel == null)
            {
                return NotFound();
            }

            _context.Artikels.Remove(artikel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtikelExists(string id)
        {
            return _context.Artikels.Any(e => e.Code == id);
        }

        private static Random random = new Random();
        public static string RandomCode()
        {
            const string karakters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(karakters, 13)
                .Select(p => p[random.Next(p.Length)]).ToArray()); 
        }

        public IQueryable<Artikel> FilterOpNaam(IQueryable<Artikel> lijst, string naam)
        {
            if(naam == null || naam == "")
            {
                return lijst;
            }
            return lijst.Where(p => p.Naam.ToLower().Contains(naam.ToLower()));
        }

        public IQueryable<Artikel> FilterOpPotMaat(IQueryable<Artikel> lijst, int? vanaf, int? tot)
        {
            if(vanaf == null && tot == null)
            {
                return lijst;
            } else if(vanaf == null)
            {
                return lijst.Where(p => p.PotMaat <= tot);
            } else if(tot == null)
            {
                return lijst.Where(p => p.PotMaat >= vanaf);
            }
            return lijst.Where(p => p.PotMaat >= vanaf && p.PotMaat <= tot);
        }

        public IQueryable<Artikel> FilterOpKleur(IQueryable<Artikel> lijst, Kleur? kleur)
        {
            if(kleur == null)
            {
                return lijst;
            }
            return lijst.Where(p => p.kleur == kleur);
        }

        public IQueryable<Artikel> FilterOpProductGroep(IQueryable<Artikel> lijst, ProductGroep? productGroep)
        {
            if(productGroep == null)
            {
                return lijst;
            }
            return lijst.Where(p => p.productGroep == productGroep);
        }

        public IQueryable<Artikel> Sorteer(IQueryable<Artikel> lijst, string sorteer)
        {
            if(sorteer == "naamaflopend" || sorteer == null)
            {
                return lijst.OrderByDescending(p => p.Naam);
            } else if(sorteer == "naamoplopend")
            {
                return lijst.OrderBy(p => p.Naam);
            } else if(sorteer == "potmaataflopend")
            {
                return lijst.OrderByDescending(p => p.PotMaat);
            } else if(sorteer == "potmaatoplopend")
            {
                return lijst.OrderBy(p => p.PotMaat);
            } else if(sorteer == "planthoogteaflopend")
            {
                return lijst.OrderByDescending(p => p.PlantHoogte);
            } else if(sorteer == "planthoogteoplopend")
            {
                return lijst.OrderBy(p => p.PlantHoogte);
            } else
            {
                return lijst;
            }
        }

        public IQueryable<Artikel> Pagineer(IQueryable<Artikel> lijst, int pagina, int aantal)
        {
            if(pagina > -1)
            {
                return lijst.Skip(pagina * aantal).Take(aantal);
            }
            return lijst;
        }
    }
}
