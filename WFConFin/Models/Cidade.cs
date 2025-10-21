using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WFConFin.Models;

public class Cidade
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo nome é obrigatório")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O campo Estado é obrigatório")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "O campo Estado deve ter 2 caracteres")]
    public string EstadoSigla { get; set; }

    public Cidade()
    {
        Id = Guid.NewGuid();
    }

    //Relacionamento Entity Framework
    // JsonIgnore para não trazer o resultado nulo!
    [JsonIgnore]
    public Estado Estado { get; set; }
}
