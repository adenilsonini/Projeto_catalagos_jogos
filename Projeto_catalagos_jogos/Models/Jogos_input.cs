using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_catalagos_jogos.Models
{
    public class Jogos_input
    { 
        [Required]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "O nome do jogo deve conter no mínimo 3 e no máximo 250 caracter.")]
        public string Nome_jogo { get; set; }
        public string Distribuidora { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 5, ErrorMessage = "O nome da Desenvolvedora deve conter no mínimo 5 e no máximo 250 caracter.")]
        public string Desenvolvedora { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Informe o Ano lançamento do Jogo ?")]
        public int Ano_lancamento { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Informe a classificacao do jogo ?")]
        public string Classificacao { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Informe o audio do jogo ?")]
        public string Audio { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "O Preço deve ser entre R$ 1,00 a 1.000,00 ?")]
        public Double Preco { get; set; }
    }
}
