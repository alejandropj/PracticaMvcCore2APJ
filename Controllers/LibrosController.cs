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

                libros.Remove(libro);

                if (libros.Count == 0)
                {
                    this.memoryCache.Remove("CARRITO");
                    this.memoryCache.Remove("TOTAL");
                }
                else
                {
                    this.memoryCache.Set("CARRITO", libros);
                }
            }
            List<Libro> carrito = this.memoryCache.Get<List<Libro>>("CARRITO");
            if(carrito != null)
            {
                int suma = carrito.Sum(x => x.Precio);
                this.memoryCache.Set("TOTAL", suma);
            }
            return View();
        }

        public async Task<IActionResult> ComprarLibros()
        {
            List<Libro> carrito = this.memoryCache.Get<List<Libro>>("CARRITO");
            if (carrito != null)
            {
                return RedirectToAction("VistaPedidos");
            }
            else
            {
                ViewData["MENSAJE"] = "No puedes comprar nada";
            }
                return View();
        }
        public async Task<IActionResult> VistaPedidos()
        {
            int idUser = int.Parse(HttpContext.User.FindFirst("Id").Value);
            List<VistaPedido> vistaPedidos = await this.repo.GetVistaPedidoAsync(idUser);
            return View(vistaPedidos);
        }
    }
}
