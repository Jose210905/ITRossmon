using Dapper;
using InventarioAPI.Data;
using InventarioAPI.Interfaces;
using InventarioAPI.Models;

namespace InventarioAPI.Services;

public class CategoriaService : ICategoriaService
{
    private readonly AppDbContext _context;

    public CategoriaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Categoria>> ObtenerTodas()
    {
        using var conn = _context.CreateConnection();
        return await conn.QueryAsync<Categoria>("sp_ObtenerCategorias",
            commandType: System.Data.CommandType.StoredProcedure);
    }

    public async Task<int> Insertar(CategoriaRequest request)
    {
        using var conn = _context.CreateConnection();
        return await conn.ExecuteScalarAsync<int>("sp_InsertarCategoria", new
        {
            request.Nombre,
            request.Descripcion
        }, commandType: System.Data.CommandType.StoredProcedure);
    }
}