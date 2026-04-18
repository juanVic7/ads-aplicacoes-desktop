using System;
using System.Collections.Generic;
using System.Linq;

public class Musica
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Artista { get; set; }
    public string Genero { get; set; }
    public int DuracaoSegundos { get; set; }
    public long Reproducoes { get; set; }
}
class Program
{
    static void Main()
    {
        /* Base de Dados em Memória */
        List<Musica> playlist = new List<Musica>
        {
            new Musica { Id=1, Nome="Bohemian Rhapsody", Artista="Queen",
            Genero="Rock", DuracaoSegundos=354, Reproducoes=1500000000 },
            new Musica { Id=2, Nome="Shape of You", Artista="Ed Sheeran",
            Genero="Pop", DuracaoSegundos=233, Reproducoes=3000000000 },
            new Musica { Id=3, Nome="Hotel California", Artista="Eagles",
            Genero="Rock", DuracaoSegundos=390, Reproducoes=900000000 },
            new Musica { Id=4, Nome="Blinding Lights", Artista="The Weeknd",
            Genero="Pop", DuracaoSegundos=200, Reproducoes=2800000000 },
            new Musica { Id=5, Nome="Smells Like Teen Spirit", Artista="Nirvana",
            Genero="Rock", DuracaoSegundos=301, Reproducoes=1200000000 },
            new Musica { Id=6, Nome="Rolling in the Deep", Artista="Adele",
            Genero="Pop", DuracaoSegundos=228, Reproducoes=1800000000 },
            new Musica { Id=7, Nome="Enter Sandman", Artista="Metallica",
            Genero="Metal", DuracaoSegundos=331, Reproducoes=800000000 },
            new Musica { Id=8, Nome="Despacito", Artista="Luis Fonsi",
            Genero="Reggaeton", DuracaoSegundos=228, Reproducoes=4000000000 },
            new Musica { Id=9, Nome="Billie Jean", Artista="Michael Jackson",
            Genero="Pop", DuracaoSegundos=294, Reproducoes=1100000000 },
            new Musica { Id=10, Nome="Numb", Artista="Linkin Park", Genero="Rock",
            DuracaoSegundos=187, Reproducoes=950000000 }
        };
        bool continuar = true;
        /* Loop do Menu Interativo */
        while (continuar)
        {
            Console.Clear();
            Console.WriteLine("=== PORT-A-SONG DATA ANALYTICS ===");
            Console.WriteLine("1. Procurar músicas por Gênero");
            Console.WriteLine("2. Ver o Top 3 músicas mais ouvidas (Geral)");
            Console.WriteLine("3. Obter média de duração do catálogo");
            Console.WriteLine("4. Verificar se existe algum artista específico");
            Console.WriteLine("0. Sair do sistema");
            Console.Write("\nEscolha uma opção: ");
            string opcao = Console.ReadLine();
            if (opcao != "0") Console.WriteLine("\n--- RESULTADO ---");
            else Console.WriteLine("Encerrando o sistema...");

            switch (opcao)
            {
                case "1":
                    Console.Write("Digite o Gênero desejado (ex: Rock, Pop,Metal): ");
                    string generoBuscado = Console.ReadLine();
                    BuscarPorGenero(playlist, generoBuscado);
                    break;
                case "2":
                    MaisOuvidas(playlist);
                    break;
                case "3":
                    DuracaoMediaCatalogo(playlist);
                    break;
                case "4":
                    Console.Write("Qual o nome do artista que deseja verificar?");
                    string artistaBuscado = Console.ReadLine();
                    BuscarArtista(playlist, artistaBuscado);
                    break;
                case "0":
                    continuar = false;
                    break;
                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }
            if (continuar)
            {
                Console.WriteLine("\nPressione ENTER para voltar ao menu...");
                Console.ReadLine();
            }
        }
    }
    static void BuscarPorGenero(List<Musica> playlist, string genero)
    {
        var resultadosBusca = playlist.Where(m => m.Genero.ToLower().Contains(genero.ToLower()));
        if (!resultadosBusca.Any())
        {
            Console.WriteLine($"Não há nenhuma música do gênero {genero}");
        }
        else
        {
            Console.WriteLine($"\nAqui estão algumas opções de {genero} para você:\n");
            foreach (Musica m in resultadosBusca)
            {
                Console.WriteLine($"- {m.Nome}");
            }
        }
    }
    static void MaisOuvidas(List<Musica> playlist)
    {
        int posicao = 1;
        var topOuvidas = playlist.OrderByDescending(m => m.Reproducoes).Take(3);
        foreach (Musica m in topOuvidas)
        {
            Console.WriteLine($"{posicao}º {m.Nome} - {m.Reproducoes:N0} de reproduções.");
            posicao++;
        }
    }
    static void DuracaoMediaCatalogo(List<Musica> playlist)
    {
        var mediaDuracao = playlist.Average(m => m.DuracaoSegundos);
        Console.WriteLine($"\nDuração média do catálogo: {mediaDuracao} segundos.");
    }
    static void BuscarArtista(List<Musica> playlist, string nomeArtista)
    {
        var musicas = playlist.Where(m => m.Artista.ToLower().Trim() == nomeArtista.ToLower().Trim()).ToList();

        if (musicas.Any())
        {
            Console.WriteLine($"\nMúsicas de {nomeArtista} disponíveis no catálogo:\n");

            foreach (var m in musicas)
            {
                Console.WriteLine($"- {m.Nome}");
            }
        }
        else
        {
            Console.WriteLine($"\nNenhuma música encontrada para o artista {nomeArtista}.");
        }
    }
}