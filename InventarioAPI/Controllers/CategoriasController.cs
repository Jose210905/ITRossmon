using InventarioAPI.Interfaces;
using InventarioAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodas()
    {
        var categorias = await _categoriaService.ObtenerTodas();
        return Ok(categorias);
    }

    [HttpPost]
    public async Task<IActionResult> Insertar(CategoriaRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
            return BadRequest(new { mensaje = "El nombre de la categoría es requerido" });

        var id = await _categoriaService.Insertar(request);
        return Ok(new { id, mensaje = "Categoría creada correctamente" });
    }
}