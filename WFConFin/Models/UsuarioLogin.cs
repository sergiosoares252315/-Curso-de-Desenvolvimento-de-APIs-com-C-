using System;
using System.ComponentModel.DataAnnotations;

namespace WFConFin.Models;

public class UsuarioLogin
{
    [Required(ErrorMessage = " O campo {0} é obrigatório")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres")]
    public string Login { get; set; }

    [Required(ErrorMessage = " O campo Senha é obrigatório")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres")]
    public string Password { get; set; }
}
