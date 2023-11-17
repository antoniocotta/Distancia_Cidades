using System;
using System.Collections.Generic;
using System.Linq;



class Program
{
    static void Main()
    {
        // Criar o grafo e adicionar os vértices
        var grafo = new Grafo();
        var capinopolis = grafo.AdicionarNo("Capinópolis");
        var centralina = grafo.AdicionarNo("Centralina");
        var ituiutaba = grafo.AdicionarNo("Ituiutaba");
        var itumbiara = grafo.AdicionarNo("Itumbiara");
        var monteAlegreDeMinas = grafo.AdicionarNo("Monte Alegre de Minas");
        var douradinhos = grafo.AdicionarNo("Douradinhos");
        var tupaciguara = grafo.AdicionarNo("Tupaciguara");
        var uberlandia = grafo.AdicionarNo("Uberlândia");
        var araguari = grafo.AdicionarNo("Araguari");
        var indianopolis = grafo.AdicionarNo("Indianópolis");
        var romaria = grafo.AdicionarNo("Romaria");
        var cascalhoRico = grafo.AdicionarNo("Cascalho Rico");
        var estrelaDoSul = grafo.AdicionarNo("Estrela do Sul");
        var saoJuliana = grafo.AdicionarNo("São Juliana");
        var grupiara = grafo.AdicionarNo("Grupiara");

        // Adicionar as arestas
        grafo.ConectarNos(capinopolis, centralina, 40);
        grafo.ConectarNos(capinopolis, ituiutaba, 30);
        grafo.ConectarNos(centralina, itumbiara, 20);
        grafo.ConectarNos(centralina, monteAlegreDeMinas, 75);
        grafo.ConectarNos(ituiutaba, monteAlegreDeMinas, 85);
        grafo.ConectarNos(ituiutaba, douradinhos, 90);
        grafo.ConectarNos(itumbiara, tupaciguara, 55);
        grafo.ConectarNos(monteAlegreDeMinas, tupaciguara, 44);
        grafo.ConectarNos(monteAlegreDeMinas, douradinhos, 28);
        grafo.ConectarNos(monteAlegreDeMinas, uberlandia, 60);
        grafo.ConectarNos(tupaciguara, uberlandia, 60);
        grafo.ConectarNos(douradinhos, uberlandia, 63);
        grafo.ConectarNos(uberlandia, araguari, 30);
        grafo.ConectarNos(uberlandia, indianopolis, 45);
        grafo.ConectarNos(uberlandia, romaria, 78);
        grafo.ConectarNos(araguari, cascalhoRico, 28);
        grafo.ConectarNos(araguari, estrelaDoSul, 34);
        grafo.ConectarNos(indianopolis, saoJuliana, 40);
        grafo.ConectarNos(romaria, saoJuliana, 28);
        grafo.ConectarNos(romaria, estrelaDoSul, 27);
        grafo.ConectarNos(cascalhoRico, grupiara, 32);
        grafo.ConectarNos(grupiara, estrelaDoSul, 38);

        // Perguntar ao usuário a cidade de origem
        Console.Write("Digite a cidade de origem: ");
        var cidadeOrigem = Console.ReadLine();

        // Perguntar ao usuário a cidade de destino
        Console.Write("Digite a cidade de destino: ");
        var cidadeDestino = Console.ReadLine();

        // Encontrar o menor caminho usando Dijkstra
        var dijkstra = new DijkstraAlgoritmo();
        var noOrigem = grafo.Nos.FirstOrDefault(no => no.Rotulo.Equals(cidadeOrigem, StringComparison.OrdinalIgnoreCase));
        var noDestino = grafo.Nos.FirstOrDefault(no => no.Rotulo.Equals(cidadeDestino, StringComparison.OrdinalIgnoreCase));

        if (noOrigem != null && noDestino != null)
        {
            var caminho = dijkstra.EncontrarCaminhoMaisCurto(noOrigem, noDestino);

            if (caminho != null)
            {
                Console.WriteLine("Menor caminho encontrado:");

                // Correção na linha abaixo
                Console.WriteLine($"Distância total: {CalcularDistanciaTotal(caminho)} km");

                Console.Write("Cidades por onde passou: ");
                foreach (var no in caminho)
                {
                    Console.WriteLine(no.Rotulo);
                }
            }
            else
            {
                Console.WriteLine("Não foi possível encontrar um caminho entre as cidades especificadas.");
            }
        }
        // Função para calcular a distância total do caminho
        static int CalcularDistanciaTotal(Grafo.No[] caminho)
        {
            int distanciaTotal = 0;

            for (int i = 0; i < caminho.Length - 1; i++)
            {
                var aresta = caminho[i].Arestas.FirstOrDefault(a => a.No1 == caminho[i + 1] || a.No2 == caminho[i + 1]);
                distanciaTotal += aresta != null ? aresta.Valor : 0;
            }

            return distanciaTotal;
        }
    }




    public class Grafo
    {
        private readonly List<No> nos = new List<No>();

        public IEnumerable<No> Nos => nos;

        public No AdicionarNo(string rotulo)
        {
            var novoNo = new No(rotulo);
            nos.Add(novoNo);
            return novoNo;
        }

        public void ConectarNos(No noOrigem, No noDestino, int valorConexao)
        {
            noOrigem.ConectarA(noDestino, valorConexao);
        }

        public class No
        {
            public string Rotulo { get; }
            private readonly List<Aresta> arestas = new List<Aresta>();

            public No(string rotulo)
            {
                Rotulo = rotulo;
            }

            public IEnumerable<Aresta> Arestas => arestas;

            public IEnumerable<VizinhancaInfo> Vizinhos =>
                from aresta in Arestas
                select new VizinhancaInfo(
                    aresta.No1 == this ? aresta.No2 : aresta.No1,
                    aresta.Valor
                );

            public object Valor { get; internal set; }

            private void Atribuir(Aresta aresta)
            {
                arestas.Add(aresta);
            }

            public void ConectarA(No outroNo, int valorConexao)
            {
                Aresta.Criar(valorConexao, this, outroNo);
            }

            public struct VizinhancaInfo
            {
                public No No { get; }
                public int PesoParaNo { get; }

                public VizinhancaInfo(No no, int pesoParaNo)
                {
                    No = no;
                    PesoParaNo = pesoParaNo;
                }
            }

            public class Aresta
            {
                public int Valor { get; }
                public No No1 { get; }
                public No No2 { get; }

                private Aresta(int valor, No no1, No no2)
                {
                    if (valor <= 0)
                    {
                        throw new ArgumentException("O valor da aresta precisa ser positivo.");
                    }

                    Valor = valor;
                    No1 = no1;
                    no1.Atribuir(this);
                    No2 = no2;
                    no2.Atribuir(this);
                }

                public static Aresta Criar(int valor, No no1, No no2)
                {
                    return new Aresta(valor, no1, no2);
                }
            }
        }
    }
    public interface IShortestPathFinder
    {
        Grafo.No[] FindShortestPath(Grafo.No from, Grafo.No to);
    }

    public class DijkstraAlgoritmo : IShortestPathFinder
    {
        private class Peso
        {
            public Grafo.No De { get; }
            public int Valor { get; }

            public Peso(Grafo.No de, int valor)
            {
                De = de;
                Valor = valor;
            }
        }

        class DadosDeVisita
        {
            readonly List<Grafo.No> _visitados =
                new List<Grafo.No>();

            readonly Dictionary<Grafo.No, Peso> _pesos =
                new Dictionary<Grafo.No, Peso>();

            readonly List<Grafo.No> _agendados =
                new List<Grafo.No>();

            public void RegistrarVisita(Grafo.No no)
            {
                if (!_visitados.Contains(no))
                    _visitados.Add(no);
            }

            public bool FoiVisitado(Grafo.No no)
            {
                return _visitados.Contains(no);
            }

            public void AtualizarPeso(Grafo.No no, Peso novoPeso)
            {
                if (!_pesos.ContainsKey(no))
                {
                    _pesos.Add(no, novoPeso);
                }
                else
                {
                    _pesos[no] = novoPeso;
                }
            }

            public Peso ConsultarPeso(Grafo.No no)
            {
                Peso resultado;
                if (!_pesos.ContainsKey(no))
                {
                    resultado = new Peso(null, int.MaxValue);
                    _pesos.Add(no, resultado);
                }
                else
                {
                    resultado = _pesos[no];
                }
                return resultado;
            }

            public void AgendarVisita(Grafo.No no)
            {
                _agendados.Add(no);
            }

            public bool TemVisitasAgendadas => _agendados.Count > 0;

            public Grafo.No ObterNoParaVisitar()
            {
                var ordenados = from n in _agendados
                                orderby ConsultarPeso(n).Valor
                                select n;

                var resultado = ordenados.First();
                _agendados.Remove(resultado);
                return resultado;
            }

            public bool TemCaminhoCalculadoParaOrigem(Grafo.No no)
            {
                return ConsultarPeso(no).De != null;
            }

            public IEnumerable<Grafo.No> CaminhoCalculadoParaOrigem(Grafo.No no)
            {
                var n = no;
                while (n != null)
                {
                    yield return n;
                    n = ConsultarPeso(n).De;
                }
            }
        }

        public Grafo.No[] EncontrarCaminhoMaisCurto(Grafo.No de, Grafo.No para)
        {
            var controle = new DadosDeVisita();

            controle.AtualizarPeso(de, new Peso(null, 0));
            controle.AgendarVisita(de);

            while (controle.TemVisitasAgendadas)
            {
                var noVisitado = controle.ObterNoParaVisitar();
                var pesoNoVisitado = controle.ConsultarPeso(noVisitado);
                controle.RegistrarVisita(noVisitado);

                foreach (var infoVizinhanca in noVisitado.Vizinhos)
                {
                    if (!controle.FoiVisitado(infoVizinhanca.No))
                    {
                        controle.AgendarVisita(infoVizinhanca.No);
                    }

                    var pesoVizinho = controle.ConsultarPeso(infoVizinhanca.No);

                    var pesoProvavel = (pesoNoVisitado.Valor + infoVizinhanca.PesoParaNo);
                    if (pesoVizinho.Valor > pesoProvavel)
                    {
                        controle.AtualizarPeso(infoVizinhanca.No, new Peso(noVisitado, pesoProvavel));
                    }
                }
            }

            return controle.TemCaminhoCalculadoParaOrigem(para)
                ? controle.CaminhoCalculadoParaOrigem(para).Reverse().ToArray()
                : null;
        }

        Grafo.No[] IShortestPathFinder.FindShortestPath(Grafo.No from, Grafo.No to)
        {
            throw new NotImplementedException();
        }
    }
}
