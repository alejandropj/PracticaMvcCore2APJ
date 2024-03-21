using Microsoft.EntityFrameworkCore;
using PracticaMvcCore2APJ.Data;
using PracticaMvcCore2APJ.Models;

namespace PracticaMvcCore2APJ.Repositories
{
    public class RepositoryLibros
    {
        private LibrosContext context;
        public RepositoryLibros(LibrosContext context)
        {
            this.context = context;
        }
        public async Task<List<Libro>> GetLibrosAsync()
        {
            List<Libro> libros = await this.context.Libros.ToListAsync();
            return libros;
        }     
        public async Task<Libro> FindLibroById(int idLibro)
        {
            Libro libro = await this.context.Libros.FirstOrDefaultAsync(x => x.IdLibro == idLibro);
            return libro;
        }        
        public async Task<List<Libro>> FindLibroByGenero(int idGenero)
        {
            List<Libro> libros = await this.context.Libros.Where(x => x.IdGenero == idGenero).ToListAsync();
            return libros;
        }
        public async Task<List<Genero>> GetGenerosAsync()
        {
            List<Genero> generos = await this.context.Generos.ToListAsync();
            return generos;
        }
        public async Task<Usuario> FindUsuarioAsync(int idUsuario)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == idUsuario);
        }
        public async Task<Usuario> LogInUsuarioAsync(string email, string password)
        {
            Usuario usuario = await this.context.Usuarios.FirstOrDefaultAsync
                (x => x.Email == email && x.Password == password);
            return usuario;
        }
    }
}
