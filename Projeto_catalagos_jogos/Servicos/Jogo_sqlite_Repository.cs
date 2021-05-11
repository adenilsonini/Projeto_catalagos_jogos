using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Projeto_catalagos_jogos.Models;

namespace Projeto_catalagos_jogos.Servicos
{
    public class Jogo_sqlite_Repository : IJogos_Repository
    {
        private readonly SqliteConnection SqliteConnection;
        public Jogo_sqlite_Repository(IConfiguration configuration)
        {
            SqliteConnection = new SqliteConnection(configuration.GetConnectionString("Conn_sqlite"));
        }

        public async Task<List<Jogos_View>> GetAllJogos(int pagina, int qtde)
        {
            var jogos = new List<Jogos_View>();

            var comando = $"select * from jogos order by id limit {qtde} offset {((pagina - 1) * qtde)}";

            await SqliteConnection.OpenAsync();

            SqliteCommand sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            SqliteDataReader dt = await sqliteCommand.ExecuteReaderAsync();

            while (dt.Read())
            {
                jogos.Add(new Jogos_View
                {
                    Id = (int)(long)dt["Id"],
                    Nome_jogo = (string)dt["Nome_jogo"],
                    Audio = (string)dt["Audio"],
                    Classificacao = (string)dt["Classificacao"],
                    preco = (double)dt["Preco"]
                });
            }

            await SqliteConnection.CloseAsync();
            return jogos;
        }

        public async Task<Jogos_View> GetByIdJogos(int id)
        {
            Jogos_View jogos = null;

            var comando = $"select * from jogos where Id = '{id}'";

            await SqliteConnection.OpenAsync();

            SqliteCommand sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            SqliteDataReader dt = await sqliteCommand.ExecuteReaderAsync();

            while (dt.Read())
            {
                jogos = new Jogos_View
                {
                    Id = (int)(long)dt["Id"],
                    Nome_jogo = (string)dt["Nome_jogo"],
                    Audio = (string)dt["Audio"],
                    Classificacao = (string)dt["Classificacao"],
                    preco = (double)dt["Preco"]
                };
            }

            await SqliteConnection.CloseAsync();
            return jogos;
        }

        public async Task<Jogos_View> Consultar_jogo(string Name, double Preco)
        {
            
            var comando = $"select * from jogos where Nome_jogo = '{Name}' and Preco = '{Preco}'";

            Jogos_View jogos = null;

            await SqliteConnection.OpenAsync();

            SqliteCommand sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            SqliteDataReader dt = await sqliteCommand.ExecuteReaderAsync();

            while (dt.Read())
            {
                jogos = new Jogos_View
                {
                    Id = (int)(long)dt["Id"],
                    Nome_jogo = (string)dt["Nome_jogo"],
                    Audio = (string)dt["Audio"],
                    Classificacao = (string)dt["Classificacao"],
                    preco = (double)dt["Preco"]
                };
            }

            await SqliteConnection.CloseAsync();
            return jogos;
        }

        public async Task<Jogos_View> Inserir(Jogos_input jogo)
        {

            // Rotina abaixo consulta se o jogo já esta cadastrodo com o mesmo Nome e preço
            var jogos = await Consultar_jogo(jogo.Nome_jogo, jogo.Preco);

            if (jogos != null)
            {
                return new Jogos_View
                {
                    Id = jogos.Id,
                    Nome_jogo =  jogo.Nome_jogo +  ",  Obs.: Jogo Já cadastrado no Id: " + jogos.Id,
                    Audio = jogos.Audio,
                    Classificacao = jogos.Classificacao,
                    preco = jogos.preco
                };
            }
          
            // Rotina abaixo para cadastrar o jogo no banco de dados

            var comando = $"INSERT INTO jogos (Nome_jogo, Distribuidora, Desenvolvedora, Ano_lancamento, Classificacao, Audio, Preco) VALUES ('{jogo.Nome_jogo}', '{jogo.Distribuidora}', '{jogo.Desenvolvedora}','{jogo.Ano_lancamento}','{jogo.Classificacao}', '{jogo.Audio}','{jogo.Preco}')";

            await SqliteConnection.OpenAsync();

            SqliteCommand sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            await sqliteCommand.ExecuteNonQueryAsync();
            sqliteCommand.Dispose();

            //Rotina abaixo para buscar o último codigo inserido no banco de dados
            //comando =  "SELECT * FROM jogos WHERE id = (SELECT max(Id) FROM jogos)";
            comando = "SELECT max(Id) FROM jogos";

            sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            var Id = await sqliteCommand.ExecuteScalarAsync();
           
            await SqliteConnection.CloseAsync();

            return new Jogos_View
            {
                Id = Convert.ToInt32(Id),
                Nome_jogo = jogo.Nome_jogo,
                Audio = jogo.Audio,
                Classificacao = jogo.Classificacao,
                preco = jogo.Preco
            };

        }

        public async Task<int> Atualizar(int Id, Jogos_input jogo)
        {

            var comando = $"UPDATE jogos set Nome_jogo = '{jogo.Nome_jogo} ', Distribuidora = '{jogo.Distribuidora}', Desenvolvedora = '{jogo.Desenvolvedora}', Ano_lancamento = '{jogo.Ano_lancamento}', Classificacao = '{jogo.Classificacao}', Audio = '{jogo.Audio}', Preco = '{jogo.Preco}' WHERE Id = '{Id}'";

            await SqliteConnection.OpenAsync();

            SqliteCommand sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            int ret = await sqliteCommand.ExecuteNonQueryAsync();

            await SqliteConnection.CloseAsync();

            return ret;
        }

        public async Task<int> Atualizar_preco(int Id, double preco)
        {

            var comando = $"UPDATE jogos set Preco = '{preco}' WHERE Id = '{Id}'";

            await SqliteConnection.OpenAsync();

            SqliteCommand sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            int ret = await sqliteCommand.ExecuteNonQueryAsync();

            await SqliteConnection.CloseAsync();

            return ret;
        }

        public async Task<int> Remover(int Id)
        {

            var comando = $"delete from jogos WHERE Id = '{Id}'";

            await SqliteConnection.OpenAsync();

            SqliteCommand sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            int ret = await sqliteCommand.ExecuteNonQueryAsync();

            await SqliteConnection.CloseAsync();

            return ret;
        }

        public void Dispose()
        {
            SqliteConnection?.Close();
            SqliteConnection?.Dispose();
        }

    }
}
