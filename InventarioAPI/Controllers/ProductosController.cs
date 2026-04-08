using InventarioAPI.Interfaces;
using InventarioAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventarioAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var productos = await _productoService.ObtenerTodos();
        return Ok(productos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var producto = await _productoService.ObtenerPorId(id);
        if (producto == null)
            return NotFound(new { mensaje = "Producto no encontrado" });

        return Ok(producto);
    }

    [HttpPost]
    public async Task<IActionResult> Insertar(ProductoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre) || request.Precio <= 0 || request.Stock < 0)
            return BadRequest(new { mensaje = "Datos del producto inválidos" });

        var id = await _productoService.Insertar(request);
        return Ok(new { id, mensaje = "Producto creado correctamente" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, ProductoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre) || request.Precio <= 0 || request.Stock < 0)
            return BadRequest(new { mensaje = "Datos del producto inválidos" });

        var existente = await _productoService.ObtenerPorId(id);
        if (existente == null)
            return NotFound(new { mensaje = "Producto no encontrado" });

        await _productoService.Actualizar(id, request);
        return Ok(new { mensaje = "Producto actualizado correctamente" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var existente = await _productoService.ObtenerPorId(id);
        if (existente == null)
            return NotFound(new { mensaje = "Producto no encontrado" });

        await _productoService.Eliminar(id);
        return Ok(new { mensaje = "Producto eliminado correctamente" });
    }
}