using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projeto_catalagos_jogos.Models;
using Projeto_catalagos_jogos.Servicos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Projeto_catalagos_jogos.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {

        private readonly IJogos_Repository _Jogos_Repository;

        public JogosController(IJogos_Repository Jogos_Repository)
        {
            _Jogos_Repository = Jogos_Repository;
        }

        /// <summary>
        /// Lista todos os jogos cadastrados no banco de dados
        /// </summary>
        /// <remarks>
        /// Não é possível retornar o jogos sem paginação
        /// </remarks>
        /// <param name="pagina">Indica qual página está sendo consultada: Mínimo 1</param>
        /// <param name="qtde">Indica a quantidade de registros por página: Mínimo 1 e máximo 5</param>
        /// <response code="200">Retorna a lista de jogos</response>
        /// <response code="204">Retorna se não houver nenhum jogos cadastrado</response>
      
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jogos_View>>> GetAllJogos([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int qtde = 5)
        {
            var jogo = await _Jogos_Repository.GetAllJogos(pagina, qtde);

            if (jogo.Count() == 0)
                return BadRequest("Ocorre um erro na Pesquisa."); ;

            return Ok(jogo);
        }

        /// <summary>
        /// Pesquisa jogos por Id
        /// </summary>
        /// <remarks>
        /// O Id do jogo é obrigatório na Consulta
        /// </remarks>
        ///
        [HttpGet("{Id}")]
        public async Task<ActionResult<Jogos_View>> GetByIdJogos([FromRoute] int Id)
        {
            var jogo = await _Jogos_Repository.GetByIdJogos(Id);
            if (jogo == null)
                return BadRequest("Não foi encontrado o jogo com Id informado.");

            return Ok(jogo);
        }

        /// <summary>
        /// Rotina para Cadastra Novo jogo
        /// </summary>
        /// <remarks>
        /// O campo de Nome, Desenvolvedora, Ano Lancamento, Classificação, Audio e Preço são obrigatórios.
        /// O nome do jogo deve conter no mínimo 3 e no máximo 250 caracter.
        /// O nome da Desenvolvedora deve conter no mínimo 5 e no máximo 250 caracter.
        /// O campo Ano lançamento Só aceita número
        /// O nome da Classificação deve conter no mínimo 1 e no máximo 50 caracter.
        /// O nome da Audio deve conter no mínimo 1 e no máximo 50 caracter.
        /// O preço deve ser de no mínimo R$ 1,00 e no máximo 1000,00
        /// </remarks>
        /// 
        [HttpPost]
        public async Task<ActionResult<Jogos_View>> InserirJogo([FromBody] Jogos_input jogos_Input)
        {
            var jogo = await _Jogos_Repository.Inserir(jogos_Input);

            if (jogo == null)
                return BadRequest("Cadastro não realizado.");
           
            return Ok(jogo);
        }

        /// <summary>
        /// Rotina para Alterar o Cadastro do Jogo
        /// </summary>
        /// <remarks>
        /// Obrigatórios Informar o Id para alterar as informações do jogo
        /// </remarks>
        /// 
        [HttpPut("{Id}")]
        public async Task<ActionResult> AtualizarJogos([FromRoute] int Id, [FromBody] Jogos_input jogos_Input)
        {
            int ret = await _Jogos_Repository.Atualizar(Id, jogos_Input);
            if (ret != 0)
                return Ok("Cadastro atualizado com sucesso.");
                
            return Ok("O Jogo informado não esta cadastrado no sistema.");
        }

        /// <summary>
        /// Rotina para alterar somente o preço do jogo
        /// </summary>
        /// <remarks>
        /// Obrigatórios informar o ID e o Preço do jogo
        /// </remarks>
        /// 
        [HttpPatch("{Id}/preco/{preco:double}")]
        public async Task<ActionResult> Atualizar_preco([FromRoute] int Id, [FromRoute] int preco)
        {
            int ret = await _Jogos_Repository.Atualizar_preco(Id, preco);
            if (ret != 0)
                return Ok("O Preço do jogo foi atualizado.");
            
            return Ok("O Jogo informado não esta cadastrado no sistema.");
        }

        /// <summary>
        /// Rotina para Excluir um jogo cadastrado
        /// </summary>
        /// <remarks>
        /// Obrigatórios informar o Id do jogo para excluir
        /// </remarks>
        /// 
        [HttpDelete("{Id}")]
        public async Task<ActionResult> ExcluirJogos([FromRoute] int Id)
        {
          
               int ret = await _Jogos_Repository.Remover(Id);
               if(ret != 0)
               return Ok("Jogos excluido Com sucesso.");
               
               return Ok("O Jogo informado não esta cadastrado no sistema.");
           

        }
    }
}
