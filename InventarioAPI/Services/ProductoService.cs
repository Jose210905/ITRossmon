using Dapper;
using InventarioAPI.Data;
using InventarioAPI.Interfaces;
using InventarioAPI.Models;

namespace InventarioAPI.Services;

public class ProductoService : IProductoService
{
    private readonly AppDbContext _context;

    public ProductoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Producto>> ObtenerTodos()
    {
        using var conn = _context.CreateConnection();
        return await conn.QueryAsync<Producto>("sp_ObtenerProductos", commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<Producto?> ObtenerPorId(int id)
    {
        using var conn = _context.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Producto>("sp_ObtenerProductoPorId",
            new { Id = id }, commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<int> Insertar(ProductoRequest request)
    {
        using var conn = _context.CreateConnection();
        return await conn.ExecuteScalarAsync<int>("sp_InsertarProducto", new
        {
            request.Nombre,
            request.Descripcion,
            request.Precio,
            request.Stock,
            request.CategoriaId
        }, commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task Actualizar(int id, ProductoRequest request)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync("sp_ActualizarProducto", new
        {
            Id = id,
            request.Nombre,
            request.Descripcion,
            request.Precio,
            request.Stock,
            request.CategoriaId
        }, commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task Eliminar(int id)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync("sp_EliminarProducto", new { Id = id },
            commandType: System.Data.CommandType.StoredProcedure);
    }
}