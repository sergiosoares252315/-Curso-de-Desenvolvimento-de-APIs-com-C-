using System;
using System.Collections.Generic;

namespace WFConFin.Models;

public class PaginacaoResponse<T> where T : class
{
    public IEnumerable<T> Dados { get; set; }
    public long TotalLinhas { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }

    public PaginacaoResponse(IEnumerable<T> dados, long totalLinhas, int skip, int take)
    {
        this.Dados = dados;
        this.TotalLinhas = totalLinhas;
        this.Skip = skip;
        this.Take = take;
    }
}
