﻿using eAgenda.Dominio.ModuloDespesa;
using System;

namespace eAgenda.Webapi.ViewModels.ModuloDespesa
{
    public class ListarDespesaViewModel //pegar dominios contato
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }

        public decimal Valor { get; set; }

        public DateTime Data { get; set; }

        public string FormaPagamento { get; set; }

    }
}