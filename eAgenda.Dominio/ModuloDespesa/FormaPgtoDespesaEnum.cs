﻿using System.ComponentModel;

namespace eAgenda.Dominio.ModuloDespesa
{
    public enum FormaPgtoDespesaEnum
    {
        [Description("Pix")]
        PIX = 0,

        [Description("Dinheiro")]
        Dinheiro= 1,

        [Description("Cartão de Crédito")]
        CartaoCredito = 2
    }

}
