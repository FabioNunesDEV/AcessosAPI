using Acessos.DTO.Auth;
using Acessos.DTO.usuario;
using Acessos.Exceptions;
using Acessos.Services;
using Microsoft.AspNetCore.Mvc;

namespace Acessos.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public IActionResult PostAuth([FromBody] AuthReadDTO dto)
    {
        return Requisicao.Manipulador(() =>
        {
            var token = _authService.ObterToken(dto);
            return new JsonResult(new { sucesso = true, token });
        });
    }
}
