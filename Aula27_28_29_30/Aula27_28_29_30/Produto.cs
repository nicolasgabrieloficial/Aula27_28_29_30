using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Aula27_28_29_30
{
    public class Produto
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public float Preco { get; set; }
        private const string PATH = "Database/produto.csv";

        public Produto()
        {   

            // Solução do desafio
            string pasta = PATH.Split('/')[0];
            if(!Directory.Exists(pasta))
            {
                Directory.CreateDirectory(pasta);
            }

            // Cria o arquivo caso não exista
            if(!File.Exists(PATH))
            {
                File.Create(PATH).Close();
            }
        }

        public void Cadastrar(Produto prod)
        {            
            string[] linha = new string[] { PrepararLinha(prod) };
            File.AppendAllLines(PATH, linha);
        }

        public List<Produto> Ler()
        {
            // Criamos uma lista para guardar o retorno
            List<Produto> prod = new List<Produto>();

            // Lemos o .csv e separamos em um array de linhas
            string[] linhas = File.ReadAllLines(PATH);

            // Varremos nossas linhas
            foreach(string linha in linhas)
            {
                
                string[] dado = linha.Split(";");

               

                Produto p   = new Produto();
                p.Codigo    = Int32.Parse( Separar(dado[0]) );
                p.Nome      = Separar(dado[1]);
                p.Preco     = float.Parse( Separar(dado[2]) );

                prod.Add(p);
            }

            prod = prod.OrderBy(z => z.Nome).ToList();

            return prod;
        }

        public List<Produto> Filtrar(string _nome){
            return Ler().FindAll(x => x.Nome == _nome);
        }

        public void Remover(string _termo)
        {
            // Criamos uma lista de linhas para fazer uma espécie de backup 
            // na memória do sistema
            List<string> linhas = new List<string>();

            using(StreamReader arquivo = new StreamReader(PATH))
            {
                string linha;
                while((linha = arquivo.ReadLine()) != null){
                    linhas.Add(linha);
                }
            }

            linhas.RemoveAll(a => a.Contains(_termo));

            ReescreverCSV(linhas);
        }

        public void Alterar(Produto produtoAlterado){

            // Criamos uma lista de linhas para fazer uma espécie de backup 
            List<string> linhas = new List<string>();

            using(StreamReader arquivo = new StreamReader(PATH))
            {
                string linha;
                while((linha = arquivo.ReadLine()) != null){
                    linhas.Add(linha);
                }
            }

            linhas.RemoveAll(z => z.Split(";")[0].Split("=")[1] == produtoAlterado.Codigo.ToString());

            linhas.Add( PrepararLinha( produtoAlterado ) );

            ReescreverCSV(linhas);
        }

        /// <summary>
        /// Reescreve o CSV
        /// </summary>
        /// <param name="lines">Lista de linhas</param>
        private void ReescreverCSV(List<string> lines){
            // Criamos uma forma de reescrever o arquivo sem as linhas removidas
            using(StreamWriter output = new StreamWriter(PATH))
            {
                foreach(string ln in lines)
                {
                    output.Write(ln+"\n");
                }
            }
        }

        /// <summary>
        /// Método que separa o símbolo de = da string do csv
        /// </summary>
        /// <param name="dado">Coluna do csv separada</param>
        /// <returns>string somente com o valor da coluna</returns>
        public string Separar(string dado)
        {
            return dado.Split("=")[1];
        }

        private string PrepararLinha(Produto p)
        {
            return $"codigo={p.Codigo};nome={p.Nome};preco={p.Preco}";
        }


    }
}