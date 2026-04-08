using InventarioAPI.Models;

namespace InventarioAPI.Interfaces;

public interface IProductoService
{
    Task<IEnumerable<Producto>> ObtenerTodos();
    Task<Producto?> ObtenerPorId(int id);
    Task<int> Insertar(ProductoRequest request);
    Task Actualizar(int id, ProductoRequest request);
    Task Eliminar(int id);
}