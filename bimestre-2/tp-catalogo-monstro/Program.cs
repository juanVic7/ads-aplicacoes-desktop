using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

public class Monstro
{
    private static int contadorId = 1;
    public int Id { get; set; }
    public string Nome { get; set; }
    public int NivelPerigo { get; set; }


    public Monstro() { }

    public Monstro(string nome, int nvPerigo)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Erro: Nome inválido.");
        if (nvPerigo < 0) throw new ArgumentException("Erro: Nível de Perigo inválido.");
        Id = contadorId++;
        Nome = nome;
        NivelPerigo = nvPerigo;
    }
    public void MostrarInfo()
    {
        Console.WriteLine($"[ID:{Id}] - {Nome} - Nível de perigo: {NivelPerigo}\n");
    }
    public static void AcertarContadorID(List<Monstro> lista)
    {
        if (lista != null && lista.Any())
        {
            int maiorId = lista.Max(m => m.Id);
            contadorId = maiorId + 1;
        }
    }
}

public class GerenciamentoCatalogo
{


    public List<Monstro> monstros = new List<Monstro>();
 
    private string caminho = "c:\\temp\\";
    public void CadastrarMonstro()
    {
        string nome;
        int nvPerigo;
        Console.WriteLine("=== Cadastro - Monstro ===\n");
        try
        {
            nome = Validacoes.ValidarTexto("Nome do monstro: ");
            nvPerigo = Validacoes.ValidarInt("Nîvel de perigo: ");
            var m = new Monstro(nome, nvPerigo);
            monstros.Add(m);

            Console.WriteLine("\nMonstro cadastrado com sucesso!");
            m.MostrarInfo();
            SerializarJson();
            string auditoriaMensagem = $"[{DateTime.Now}] O monstro {m.Nome} foi adicionado ao catálogo.\n";
            RegistrarAuditoria(auditoriaMensagem);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("\nPressione qualquer tecla para voltar...");

        Console.ReadKey();
    }
    public void ListarMonstros()
    {
        Console.WriteLine("=== Lista - Monstros ===\n");
        if (monstros.Any())
        {
            foreach (Monstro m in monstros) m.MostrarInfo();
        }
        else
        {
            Console.WriteLine("Aviso: A lista está vazia.\n");
        }
        Console.WriteLine("Pressione qualquer tecla para voltar...");
        Console.ReadKey();
    }
    public void SerializarJson()
    {
        try
        {
            string json = JsonSerializer.Serialize(monstros, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            string arquivoJSON = caminho + "bestiario.json";

            File.WriteAllText(arquivoJSON, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErro ao salvar: {ex.Message}");
        }

    }
    public void ExportarDatasetIA()
    {
        string arquivoDataset = caminho + "dataset_ia.jsonl";
        try
        {
            if (!monstros.Any())
            {
                Console.WriteLine("Nenhum monstro para exportar.");
                return;
            }
            if (File.Exists(arquivoDataset))
            {
                File.Delete(arquivoDataset);
            }
            foreach (Monstro m in monstros)
            {
                string json = JsonSerializer.Serialize(m);

                File.AppendAllText(arquivoDataset, json + Environment.NewLine);
            }

            string auditoriaMensagem = $"[{DateTime.Now}] Dataset de IA gerado com sucesso.\n";
            RegistrarAuditoria(auditoriaMensagem);
            Console.WriteLine("\nDataset exportado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErro ao exportar: {ex.Message}");
        }
    }
    public void RegistrarAuditoria(string mensagem)
    {
        string arquivoTXT = caminho + "auditoria.txt";
        File.AppendAllText(arquivoTXT, mensagem);

    }
    public void CarregarDados()
    {
        try
        {
            string arquivoJSON = caminho + "bestiario.json";
            if (File.Exists(arquivoJSON))
            {
                string jsonLido = File.ReadAllText(arquivoJSON);
                var lista = JsonSerializer.Deserialize<List<Monstro>>(jsonLido);

                if (lista != null) monstros = lista;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErro ao carregar dados: {ex.Message}");
        }


    }


}
public class Validacoes
{
    public static string ValidarTexto(string mensagem)
    {
        string texto;
        do
        {
            Console.Write(mensagem);
            texto = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(texto)) Console.WriteLine("Erro: Este campo não pode ficar vazio, digite novamente.\n");
        } while (string.IsNullOrWhiteSpace(texto));
        return texto;
    }
    public static int ValidarInt(string mensagem)
    {
        int num;
        bool ok;
        do
        {
            Console.Write(mensagem);
            string snum = Console.ReadLine();
            ok = int.TryParse(snum, out num);
            if (!ok || num < 0) Console.WriteLine("Erro: Esse valor não pode ser negativo, digite novamente.\n");
        } while (!ok || num < 0);
        return num;
    }

}

class Program
{
    public static void Main()
    {
        var catalogo = new GerenciamentoCatalogo();
        catalogo.CarregarDados();
        Monstro.AcertarContadorID(catalogo.monstros);

        bool continuar = true;
        while (continuar)
        {
            Console.Clear();
            Console.WriteLine("=== Catálogo Monstros - Menu ===\n");
            Console.WriteLine("1. Cadastrar novo monstro");
            Console.WriteLine("2. Listar monstros conhecidos");
            Console.WriteLine("3. Exportar dados para treino de IA");
            Console.WriteLine("0. Sair do sistema");
            Console.Write("\nEscolha uma opção: ");

            string opcao = Console.ReadLine();
            switch (opcao)
            {
                case "1":
                    Console.Clear();
                    catalogo.CadastrarMonstro();
                    break;
                case "2":
                    Console.Clear();
                    catalogo.ListarMonstros();

                    break;
                case "3":
                    catalogo.ExportarDatasetIA();
                    Console.ReadKey();
                    break;
                case "0":
                    Console.WriteLine("\nEncerrando o sistema...");
                    continuar = false;
                    break;
                default:
                    Console.WriteLine("Opção inválida!");
                    Console.ReadKey();
                    break;
            }


        }
    }
}

