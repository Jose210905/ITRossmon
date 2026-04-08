using Dapper;
using InventarioAPI.Data;
using InventarioAPI.Interfaces;
using InventarioAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InventarioAPI.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string?> Login(LoginRequest request)
    {
        using var conn = _context.CreateConnection();
        var usuario = await conn.QueryFirstOrDefaultAsync<Usuario>("sp_ObtenerUsuarioPorUsername",
            new { Username = request.Username },
            commandType: System.Data.CommandType.StoredProcedure);

        if (usuario == null) return null;

        // verificar password
        var hash = HashPassword(request.Password);
        if (hash != usuario.PasswordHash) return null;

        return GenerarToken(usuario);
    }

    public async Task<bool> Register(RegisterRequest request)
    {
        using var conn = _context.CreateConnection();

        // revisar si ya existe
        var existe = await conn.QueryFirstOrDefaultAsync<Usuario>("sp_ObtenerUsuarioPorUsername",
            new { Username = request.Username },
            commandType: System.Data.CommandType.StoredProcedure);

        if (existe != null) return false;

        var hash = HashPassword(request.Password);
        await conn.ExecuteAsync("sp_InsertarUsuario", new
        {
            Username = request.Username,
            PasswordHash = hash,
            Rol = "User"
        }, commandType: System.Data.CommandType.StoredProcedure);

        return true;
    }

    private string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    private string GenerarToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Username),
            new Claim(ClaimTypes.Role, usuario.Rol)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}