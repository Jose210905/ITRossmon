using InventarioAPI.Interfaces;
using InventarioAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventarioAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { mensaje = "Usuario y contraseña son requeridos" });

        var token = await _authService.Login(request);
        if (token == null)
            return Unauthorized(new { mensaje = "Credenciales incorrectas" });

        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { mensaje = "Usuario y contraseña son requeridos" });

        var resultado = await _authService.Register(request);
        if (!resultado)
            return BadRequest(new { mensaje = "El usuario ya existe" });

        return Ok(new { mensaje = "Usuario registrado correctamente" });
    }
}