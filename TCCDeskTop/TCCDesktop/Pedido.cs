using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCDesktop
{
    class Pedido
    {
        public int id { get; set; }
        public string Produtos { get; set; }
        public string status { get; set; }
        public float preco { get; set; }
        public string Bairro { get; set; }
        public string endereco { get; set;}
        public string formaPagamento { get; set; }
        public string troco { get; set; }
    }
}
