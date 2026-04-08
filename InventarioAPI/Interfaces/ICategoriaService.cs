using InventarioAPI.Models;

namespace InventarioAPI.Interfaces;

public interface ICategoriaService
{
    Task<IEnumerable<Categoria>> ObtenerTodas();
    Task<int> Insertar(CategoriaRequest request);
}