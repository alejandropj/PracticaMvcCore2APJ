using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2APJ.Models;
using PracticaMvcCore2APJ.Repositories;

namespace PracticaMvcCore2APJ.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryLibros repo;
        public LibrosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> List()
        {
            List<Libro> libros = await this.repo.GetLibrosAsync();
            return View(libros);
        }        
        public async Task<IActionResult> LibrosGenero(int idGenero)
        {
            List<Libro> libros = await this.repo.FindLibroByGenero(idGenero);
            return View(libros);
        } 
        public async Task<IActionResult> Details(int idLibro)
        {
            Libro libro = await this.repo.FindLibroById(idLibro);
            return View(libro);
        }
    }
}
