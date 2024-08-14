using CmciSiteAPI.Models;
using Microsoft.AspNetCore.Authorization;
using MySql.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace CmciSiteAPI.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class BaseDeDonneesController : ControllerBase
    { 
        CmciDBContext Contextdb;
        private readonly IConfiguration _configuration;
        public BaseDeDonneesController(CmciDBContext contextdb, IConfiguration configuration) 
        {
            Contextdb = contextdb;
            _configuration = configuration;
        }

        [HttpGet("cmciebolowa/books")]
        public async Task<ActionResult<List<Booksdb>>> getAllBooks()
        {
            var listBooks = await Contextdb.Books.ToListAsync();
            return listBooks;

        }

        [HttpGet("cmciebolowa/books/{id}")]
        public async Task<ActionResult<Booksdb>> getBook(int id)
        {
            var book = await Contextdb.Books.FindAsync(id);
            return book == null ? NotFound() : book;
        }

        [HttpPost("cmciebolowa/books")]
        public async Task<ActionResult<Booksdb>> EnregistrerBooks(Booksdb book)
        {
            Contextdb.Books.Add(book);
            await Contextdb.SaveChangesAsync();
            return Ok(book);
        }

        [HttpPut("cmciebolowa/books")]
        public async Task<ActionResult<Booksdb>> ModifiererBooks(Booksdb book)
        {
            var oldValue = await Contextdb.Books.FindAsync(book.Id);

            if (oldValue != null) 
            {
                if(book.ImgSource != null)
                    oldValue.ImgSource = book.ImgSource;
                if (book.Name != null)
                    oldValue.Name = book.Name;
                if (book.Prix > 0)
                    oldValue.Prix = book.Prix;
                if (book.PrixAchat > 0)
                    oldValue.PrixAchat = book.PrixAchat;

                await Contextdb.SaveChangesAsync();
                return Ok(oldValue);
            }
            else
            {
                return BadRequest("Le livre que vous essayez de modifier n'existe pas");

            }            

        }

        [HttpDelete("cmciebolowa/books")]
        public async Task<ActionResult<Booksdb>> DeleteBook(int id) 
        {
            var book = await Contextdb.Books.FindAsync(id);

            if(book == null)
                return BadRequest("Le livre que vous essayez de modifier n'existe pas");

            Contextdb.Remove(book);
            await Contextdb.SaveChangesAsync();
            return Ok(book);
        
        }


        //-------------------------- requetes pour la tables des administrateurs --------------------------------

        [HttpGet("cmciebolowa/administrateur/{id}")]
        public async Task<ActionResult<Admindb>> getAdmin(int id)
        {
            var admin = await Contextdb.administrateurs.FindAsync(id);
            if(admin != null)
            {
                return Ok(admin);

            }
            return NotFound();
        }

        [HttpPut("cmciebolowa/administrateurs")]
        public async Task<ActionResult<Booksdb>> ModifiererAdmin(Admindb admin)
        {
            var oldValue = await Contextdb.administrateurs.FindAsync(admin.Id);

            if (oldValue != null)
            {
                if (admin.Name != null)
                    oldValue.Name = admin.Name;
                if (admin.Name != null)
                    oldValue.Pwd = admin.Pwd;

                await Contextdb.SaveChangesAsync();
                return Ok(oldValue);
            }
            else
            {
                return BadRequest("L'administrateur que vous essayez de modifier n'existe pas");

            }

        }

        [HttpDelete("cmciebolowa/administrateurs")]
        public async Task<ActionResult<Admindb>> DeleteAdmin(int Id)
        {
            var admin = await Contextdb.administrateurs.FindAsync(Id);

            if (admin == null)
                return BadRequest("Le livre que vous essayez de modifier n'existe pas");

            Contextdb.Remove(admin);
            await Contextdb.SaveChangesAsync();
            return Ok($"L'administrateur {admin.Name} ayant pour mot de passe {admin.Pwd} a bien ete supprime");

        }

        //-------------------------- Creer le token  --------------------------------


    }
}
