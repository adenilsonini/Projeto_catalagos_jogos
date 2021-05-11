using Projeto_catalagos_jogos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_catalagos_jogos.Servicos
{
    public interface IJogos_Repository : IDisposable
    {
        Task<List<Jogos_View>> GetAllJogos(int pagina, int qtde);
        Task<Jogos_View> GetByIdJogos(int Id);
        Task<Jogos_View> Consultar_jogo(string Name, double Preco);
        Task<Jogos_View> Inserir(Jogos_input jogo);
        Task<int> Atualizar(int Id, Jogos_input jogo);
        Task<int> Atualizar_preco(int Id, Double preco);
        Task<int> Remover(int Id);

    }
}
