using Microsoft.AspNetCore.Mvc;
using PracticaMvcCore2APJ.Models;
using PracticaMvcCore2APJ.Repositories;

namespace PracticaMvcCore2APJ.ViewComponents
{
    public class MenuGenerosViewComponent:ViewComponent
    {
        private RepositoryLibros repo;
        public MenuGenerosViewComponent(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = await this.repo.GetGenerosAsync();
            return View(generos);
        }
    }
}
