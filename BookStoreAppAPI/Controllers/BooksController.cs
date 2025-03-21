﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreAppAPI.Data;
using AutoMapper;
using BookStoreAppAPI.Models.Book;
using AutoMapper.QueryableExtensions;
using BookStoreAppAPI.Static;
using BookStoreAppAPI.Models.Author;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper mapper;
        private readonly ILogger<BooksController> logger;

        public BooksController(BookStoreDbContext context, IMapper mapper, ILogger<BooksController> logger)
        {
            _context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Books
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
        {
            var booksDtos = await _context.Books
                .Include(q => q.Author)
                .ProjectTo<BookReadOnlyDto>(mapper.ConfigurationProvider)
                .ToListAsync();
            return Ok(booksDtos);
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
        {
           
                var book = await _context.Books
                    .Include(q => q.Author)
                    .ProjectTo<BookDetailsDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (book == null)
                {
                    return NotFound();
                }

                return book;
            }
            

        // PUT: api/Books/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
        {
            if (id != bookDto.Id)
            {
                return BadRequest();
            }

            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            mapper.Map(bookDto, book);
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BookExistsAsync(id))
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

        // POST: api/Books
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BookCreateDto>> PostBook(BookCreateDto bookDto)
        {
            var book = mapper.Map<Book>(bookDto);

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> BookExistsAsync(int id)
        {
            return await _context.Books.AnyAsync(e => e.Id == id);
        }
    }
}
