﻿using Acessos.Models;
using System.Text.Json.Serialization;

namespace Acessos.DTO;

public class UsuarioReadDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
}
