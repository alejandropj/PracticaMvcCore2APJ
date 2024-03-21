using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PracticaMvcCore2APJ.Models;
using PracticaMvcCore2APJ.Repositories;

namespace PracticaMvcCore2APJ.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryLibros repo;
        private IMemoryCache memoryCache;
        public LibrosController(RepositoryLibros repo, IMemoryCache memoryCache)
        {
            this.repo = repo;
            this.memoryCache = memoryCache;
        }
        public async Task<IActionResult> List(int? idCarrito)
        {
            if (idCarrito != null)
            {
                List<Libro> librosCarrito;
                if(this.memoryCache.Get("CARRITO") == null)
                {
                    librosCarrito = new List<Libro>();
                }
                else
                {
                    librosCarrito = this.memoryCache.Get<List<Libro>>("CARRITO");
                }
                Libro libro = await this.repo.FindLibroById(idCarrito.Value);
                librosCarrito.Add(libro);
                this.memoryCache.Set("CARRITO", librosCarrito);
            }
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

        public async Task<IActionResult> CarritoLibros(int? idEliminar)
        {
            if (idEliminar != null)
            {
                List<Libro> libros = 
                    this.memoryCache.Get<List<Libro>>("CARRITO");
                Libro libro = libros.Find(z => z.IdLibro == idEliminar.Value);
            }
        }
    }
}
