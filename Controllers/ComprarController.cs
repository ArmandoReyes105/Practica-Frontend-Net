using frontendnet.Models;
using frontendnet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frontendnet
{
    [Authorize(Roles = "Usuario")]
    public class ComprarController(ProductosClientService productos, IConfiguration configuration, CarritoClientService carritoClient) : Controller
    {
        public async Task<IActionResult> Index(string? s)
        {
            List<Producto>? lista = [];
            try
            {
                lista = await productos.GetAsync(s); 
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth"); 
            }

            ViewBag.Url = configuration["UrlWebAPI"];
            ViewBag.search = s; 
            return View(lista);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            try
            {
                await carritoClient.PostItemAsync(id);
                return Redirect("/carrito"); 
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    return RedirectToAction("Salir", "Auth"); 
            }

            return RedirectToAction("Index");
        }
    }
}