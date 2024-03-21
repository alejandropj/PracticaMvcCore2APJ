using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PracticaMvcCore2APJ.Extensions;
using PracticaMvcCore2APJ.Filters;
using PracticaMvcCore2APJ.Models;
using PracticaMvcCore2APJ.Repositories;
using System.Net.Http;

namespace PracticaMvcCore2APJ.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryLibros repo;
        private IMemoryCache memoryCache;
        public LibrosController(RepositoryLibros repo, IMemoryCache memoryCache)
        {
            this.repo = repo;
            this.repo = repo;
            this.memoryCache = memoryCache;
        }
        public async Task<IActionResult> List(int? idCarrito)
        {
            if (idCarrito != null)
            {
                List<Libro> librosCarrito;
                if(HttpContext.Session.GetObject<List<Libro>>("CARRITO") == null)
                {
                    librosCarrito = new List<Libro>();
                }
                else
                {
                    librosCarrito = HttpContext.Session.GetObject<List<Libro>>("CARRITO");
                }
                Libro libro = await this.repo.FindLibroById(idCarrito.Value);
                librosCarrito.Add(libro);
                HttpContext.Session.SetObject("CARRITO", librosCarrito);
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
                    HttpContext.Session.GetObject<List<Libro>>("CARRITO");
                Libro libro = libros.Find(z => z.IdLibro == idEliminar.Value);

                libros.Remove(libro);

                if (libros.Count == 0)
                {
                    HttpContext.Session.Remove("CARRITO");
                    HttpContext.Session.SetString("TOTAL","0");
                }
                else
                {
                    HttpContext.Session.SetObject("CARRITO", libros);
                }
            }
            List<Libro> carrito = HttpContext.Session.GetObject<List<Libro>>("CARRITO");
            if(carrito != null)
            {
                int suma = carrito.Sum(x => x.Precio);
                HttpContext.Session.SetString("TOTAL", suma.ToString());
            }
            else
            {
                HttpContext.Session.SetString("TOTAL", "0");
            }
            return View();
        }

        [AuthorizeUsuarios]
        public async Task<IActionResult> Comprar()
        {
            List<Libro> carrito = HttpContext.Session.GetObject<List<Libro>>("CARRITO");
            if (carrito != null)
            {
                int idUser = int.Parse(HttpContext.User.FindFirst("Id").Value);
                await this.repo.RealizarCompra(carrito, idUser);
                HttpContext.Session.Remove("CARRITO");
                HttpContext.Session.Remove("TOTAL");
                return RedirectToAction("VistaPedidos");
            }
            else
            {
                ViewData["MENSAJE"] = "No puedes comprar nada";
            }
                return View();
        }
        [AuthorizeUsuarios]
        public async Task<IActionResult> VistaPedidos()
        {
            int idUser = int.Parse(HttpContext.User.FindFirst("Id").Value);
            List<VistaPedido> vistaPedidos = await this.repo.GetVistaPedidoAsync(idUser);
            return View(vistaPedidos);
        }
    }
}
