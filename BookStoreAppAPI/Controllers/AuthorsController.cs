using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreAppAPI.Data;
using BookStoreAppAPI.Models.Author;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections;
using BookStoreAppAPI.Static;

namespace BookStoreAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper mapper;
        private readonly ILogger<AuthorsController> logger;

        public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
        {
            
            try
            {
                var authors = await _context.Authors.ToListAsync();

                var authorsDtos = mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authors);
                return Ok(authorsDtos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting authors");
                return StatusCode(500, Messages.Error500Message);
            }
            
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
        {

            try
            {
                var author = await _context.Authors.FindAsync(id);

                if (author == null)
                {
                    return NotFound();
                }

                var authorDto = mapper.Map<AuthorReadOnlyDto>(author);
                return Ok(authorDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting authors");
                return StatusCode(500, Messages.Error500Message);
            }

            
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
        {
            if (id != authorDto.Id)
            {
                return BadRequest();
            }

            var author = await _context.Authors.FindAsync(id);
            
            if(author == null)
            {
                return NotFound();
            }

            mapper.Map(authorDto, author);
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AuthorExists(id))
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

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
        {
           var author = mapper.Map<Author>(authorDto);

            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> AuthorExists(int id)
        {
            return await _context.Authors.AnyAsync(e => e.Id == id);
        }
    }
}
