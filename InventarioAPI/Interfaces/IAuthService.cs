using InventarioAPI.Models;

namespace InventarioAPI.Interfaces;

public interface IAuthService
{
    Task<string?> Login(LoginRequest request);
    Task<bool> Register(RegisterRequest request);
}