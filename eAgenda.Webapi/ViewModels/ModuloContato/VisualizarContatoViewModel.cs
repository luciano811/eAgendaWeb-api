﻿using System;
using System.Collections.Generic;

namespace eAgenda.Webapi.ViewModels.ModuloContato
{
    public class VisualizarContatoViewModel
    {
        public VisualizarContatoViewModel()
        {
        }

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Empresa { get; set; }
        public string Cargo { get; set; }
    }
}