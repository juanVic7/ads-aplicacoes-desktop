using System;
using System.Linq;
using System.Collections.Generic;

//Trabalho de conclusão do 1°Bimestre 

//classe mãe
public abstract class ItemRPG
{
    public int Id { get; protected set; }
    public string Nome { get; protected set; }
    public decimal Preco { get; protected set; }
    public int QuantidadeEstoque { get; protected set; }
    public string Tipo { get; protected set; }

    public ItemRPG(int id, string nome, decimal preco, int estoque, string tipo)
    {
        //encapsulamento da classe mãe
        if (id < 1) throw new ArgumentException("Erro: Número de ID deve ser um valor positivo.");
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Erro: Nome do item não pode ser vazio.");
        if (preco < 0) throw new ArgumentException("Erro: Preço não deve receber um valor negativo.");
        if (estoque < 0) throw new ArgumentException("Erro: Quantidade em estoque não deve receber um valor negativo.");
        if (string.IsNullOrWhiteSpace(tipo)) throw new ArgumentException("Erro: Tipo de item não pode ser vazio.");
        Id = id;
        Nome = nome;
        Preco = preco;
        QuantidadeEstoque = estoque;
        Tipo = tipo;

    }

    public void AtualizarPreco(decimal novoPreco)
    {
        if (novoPreco < 0) throw new ArgumentException("Erro: Preço não pode ser negativo.");
        Preco = novoPreco;
    }
    public void AdicionarEstoque(int qtd)
    {
        if (qtd <= 0) throw new ArgumentException("Erro: Quantidade inválida.");
        QuantidadeEstoque += qtd;
    }
    public void RemoverEstoque(int qtd)
    {
        if (qtd <= 0) throw new ArgumentException("Erro: Quantidade inválida.");
        if (qtd > QuantidadeEstoque) throw new InvalidOperationException("Erro: Estoque insuficiente.");
        QuantidadeEstoque -= qtd;
    }

    public virtual string ExibirDetalhes()
    {
        return $"\n[ID: {Id:D3}] {GetType().Name} - {Tipo} | Nome: {Nome.PadRight(15)} | Preço: {Preco:C2} | Estoque: {QuantidadeEstoque}";
    }
}
public class Arma : ItemRPG
{
    public int Dano { get; private set; }
    public Arma(int id, string nome, decimal preco, int estoque, string tipo, int dano) : base(id, nome, preco, estoque, tipo)
    {
        if (dano < 0) throw new ArgumentException("Erro: Dano não pode receber um valor negativo.");
        Dano = dano;
    }
    public override string ExibirDetalhes()
    {
        return base.ExibirDetalhes() + $" -> Poder de Dano: {Dano}";
    }
}
public class Pocao : ItemRPG
{
    public string Efeito { get; private set; }

    public Pocao(int id, string nome, decimal preco, int estoque, string tipo, string efeito) : base(id, nome, preco, estoque, tipo)
    {
        if (string.IsNullOrWhiteSpace(efeito)) throw new ArgumentException("Erro: Efeito da poção não pode ser vazio");
        Efeito = efeito;
    }
    public override string ExibirDetalhes()
    {
        return base.ExibirDetalhes() + $" -> Efeito Especial: {Efeito}";
    }
}
public class Armadura : ItemRPG
{
    public int Defesa { get; private set; }
    public Armadura(int id, string nome, decimal preco, int estoque, string tipo, int defesa) : base(id, nome, preco, estoque, tipo)
    {
        if (defesa < 0) throw new ArgumentException("Erro: Defesa não pode receber um valor negativo.");
        Defesa = defesa;
    }
    public override string ExibirDetalhes()
    {
        return base.ExibirDetalhes() + $" -> Pontos de Defesa: {Defesa}";
    }
}
public class Ferramenta : ItemRPG
{
    public string Utilidade { get; private set; }
    public Ferramenta(int id, string nome, decimal preco, int estoque, string tipo, string utilidade) : base(id, nome, preco, estoque, tipo)
    {
        if (string.IsNullOrWhiteSpace(utilidade)) throw new ArgumentException("Erro: Utilidade da ferramenta não pode ser vazio.");
        Utilidade = utilidade;
    }
    public override string ExibirDetalhes()
    {
        return base.ExibirDetalhes() + $" -> Utilidade: {Utilidade}";
    }
}

//classe que coordena toda a lógica de venda e relatório
public class LojaService
{
    //só essa classe precisa ter acesso a lista de ítens
    private List<ItemRPG> itens = new List<ItemRPG>();
    //contador para ID dos ítens
    private int proximoId = 1;

    private List<Venda> historicoVendas = new List<Venda>();

    //Construtor da classe que cria uma lista com 4 itens
    public LojaService()
    {
        itens.Add(new Arma(proximoId++, "Espada Longa", 150, 10, "Corpo a corpo", 35));
        itens.Add(new Pocao(proximoId++, "Poção de Cura", 50, 20, "Cura", "Recupera 50 HP"));
        itens.Add(new Armadura(proximoId++, "Armadura de Ferro", 300, 5, "Pesada", 40));
        itens.Add(new Ferramenta(proximoId++, "Picareta", 80, 8, "Mineração", "Quebrar pedras"));
    }


    public void MenuCadastrarItem()
    {
        bool continuar = true;

        while (continuar)
        {
            Console.Clear();
            Console.WriteLine("=== CADASTRO DE NOVO ITEM ===");
            Console.WriteLine("\n1. Poção \n2. Arma \n3. Armadura \n4. Ferramenta \n0. Voltar");
            Console.Write("\nSelecione uma opção: ");
            string tipo = Console.ReadLine();
            if (tipo == "0")
            {
                continuar = false;
                continue;
            }
            if (tipo != "1" && tipo != "2" && tipo != "3" && tipo != "4")
            {
                MensagemErro("\nErro: Opção inválida, escolha novamente.");
                System.Threading.Thread.Sleep(2000);
                continue;
            }
            string nome = ValidarTexto("\nNome: ");
            decimal preco = ValidarPreco("Preço: ");
            int estoque = ValidarInteiro("Estoque inicial: ");
            switch (tipo)
            {
                case "1":
                    CadastrarPocao(nome, preco, estoque);
                    break;
                case "2":
                    CadastrarArma(nome, preco, estoque);
                    break;
                case "3":
                    CadastrarArmadura(nome, preco, estoque);
                    break;
                case "4":
                    CadastrarFerramenta(nome, preco, estoque);
                    break;
            }
        }

    }


    public void CadastrarArma(string nome, decimal preco, int estoque)
    {
        int idItem = proximoId;
        try
        {
            string tipoArma = ValidarTexto("Tipo de arma: ");
            int dano = ValidarInteiro("Dano: ");
            var item = new Arma(idItem, nome, preco, estoque, tipoArma, dano);
            itens.Add(item);
            proximoId++;
            MensagemSucesso($"\nArma com ID: {idItem} cadastrada com sucesso!");
        }
        catch (Exception ex)
        {
            MensagemErro(ex.Message);
        }
        System.Threading.Thread.Sleep(2000);
    }
    public void CadastrarPocao(string nome, decimal preco, int estoque)
    {

        int idItem = proximoId;

        try
        {
            string tipoPocao = ValidarTexto("Tipo de poção: ");
            string efeito = ValidarTexto("Efeito da poção: ");
            var item = new Pocao(idItem, nome, preco, estoque, tipoPocao, efeito);
            itens.Add(item);
            proximoId++;
            MensagemSucesso($"\nPoção com ID: {idItem} cadastrada com sucesso!");

        }
        catch (Exception ex)
        {
            MensagemErro(ex.Message);
        }
        System.Threading.Thread.Sleep(2000);
    }
    public void CadastrarArmadura(string nome, decimal preco, int estoque)
    {
        int idItem = proximoId;

        try
        {
            string tipoArmadura = ValidarTexto("Tipo de armadura: ");
            int defesa = ValidarInteiro("Defesa da armadura: ");
            var item = new Armadura(idItem, nome, preco, estoque, tipoArmadura, defesa);
            itens.Add(item);
            proximoId++;
            MensagemSucesso($"\nArmadura com ID: {idItem} cadastrada com sucesso!");
        }
        catch (Exception ex)
        {
            MensagemErro(ex.Message);
        }
        System.Threading.Thread.Sleep(2000);
    }
    public void CadastrarFerramenta(string nome, decimal preco, int estoque)
    {
        int idItem = proximoId;

        try
        {
            string tipoFerramenta = ValidarTexto("Tipo de ferramenta: ");
            string utilidade = ValidarTexto("Utilidade: ");
            var item = new Ferramenta(idItem, nome, preco, estoque, tipoFerramenta, utilidade);
            itens.Add(item);
            proximoId++;
            MensagemSucesso($"\nFerramenta com ID: {idItem} cadastrada com sucesso!");
        }
        catch (Exception ex)
        {
            MensagemErro(ex.Message);
        }
        System.Threading.Thread.Sleep(2000);
    }

    public decimal ValidarPreco(string mensagem)
    {
        bool ok;
        decimal preco;
        do
        {
            Console.Write(mensagem);
            string sPreco = Console.ReadLine();
            ok = decimal.TryParse(sPreco, out preco);
            if (!ok || preco < 0)
            {
                MensagemErro("Erro: Preço inválido, digite novamente.");
            }
        }
        while (!ok || preco < 0);
        return preco;
    }
    public int ValidarInteiro(string mensagem)
    {
        bool ok;
        int inteiro;
        do
        {
            Console.Write(mensagem);
            string sInteiro = Console.ReadLine();
            ok = int.TryParse(sInteiro, out inteiro);
            if (!ok || inteiro < 0)
            {
                MensagemErro("Erro: Valor inválido, digite novamente.");
            }
        }
        while (!ok || inteiro < 0);
        return inteiro;

    }
    public string ValidarTexto(string mensagem)
    {
        string texto;
        do
        {
            Console.Write(mensagem);
            texto = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(texto)) MensagemErro("Erro: Este campo não pode ficar vazio, digite novamente.");
        } while (string.IsNullOrWhiteSpace(texto));
        return texto;
    }
    public void RelatarEstoque()
    {
        Console.WriteLine("\n=== ESTOQUE ATUAL ===");

        if (!itens.Any())
        {
            MensagemAviso("\nAviso: O estoque está vazio.");
        }
        else
        {
            MensagemInfo(string.Join("\n", itens.Select(i => i.ExibirDetalhes())));
        }
    }
    public void AtualizarPrecoItem()
    {
        bool continuar = true;
        while (continuar)
        {
            Console.Clear();
            RelatarEstoque();
            int id = ValidarInteiro("\nID do item: ");
            var itemAchado = itens.FirstOrDefault(i => i.Id == id);
            if (itemAchado != null)
            {
                itemAchado.AtualizarPreco(ValidarPreco($"Novo preço para {itemAchado.Nome}: "));
                MensagemSucesso("\nPreço atualizado com sucesso!");
            }
            else MensagemAviso("\nAviso: Item não encontrado.");
            continuar = PerguntarContinuar("\nDeseja continuar? [S/N]: ");
        }

    }

    public void ReporEstoqueItem()
    {
        bool continuar = true;

        while (continuar)
        {
            Console.Clear();
            RelatarEstoque();
            int id = ValidarInteiro("\nID do item: ");
            var itemAchado = itens.FirstOrDefault(i => i.Id == id);
            if (itemAchado != null)
            {
                itemAchado.AdicionarEstoque(ValidarInteiro($"Quantidade a adicionar para {itemAchado.Nome}: "));
                MensagemSucesso("Estoque abastecido!");
            }
            else MensagemAviso("\nAviso: Item não encontrado.");
            continuar = PerguntarContinuar("\nDeseja continuar? [S/N]: ");
        }

    }
    public void VenderItem()
    {
        bool continuar = true;
        while (continuar)
        {
            Console.Clear();
            RelatarEstoque();

            int id = ValidarInteiro("\nID do item para venda: ");
            var itemAchado = itens.FirstOrDefault(i => i.Id == id);

            if (itemAchado != null)
            {
                int qtd = ValidarInteiro("Quantidade: ");

                try
                {
                    itemAchado.RemoverEstoque(qtd);

                    var novaVenda = new Venda(itemAchado.Nome, qtd, itemAchado.Preco * qtd);
                    historicoVendas.Add(novaVenda);

                    MensagemSucesso($"Venda concluída! Total: {itemAchado.Preco * qtd:C2}");
                }
                catch (Exception ex)
                {
                    MensagemErro(ex.Message);
                }
            }
            else MensagemAviso("Aviso: Item não encontrado.");
            continuar = PerguntarContinuar("\nDeseja continuar? [S/N]: ");
        }



    }
    public void RelatarHistoricoVendas()
    {
        Console.Clear();
        Console.WriteLine("=== HISTÓRICO DE VENDAS ===");
        if (!historicoVendas.Any()) MensagemAviso("\nAviso: Histórico de vendas está vazio.");
        else
        {
            string relatorioFinal = string.Join("\n", historicoVendas);
            MensagemInfo($"\n{relatorioFinal}");
        }
    }
    public void FecharCaixa()
    {
        Console.Clear();
        Console.WriteLine($"\n=== FECHAMENTO DE CAIXA ===");
        MensagemSucesso($"\nTotal faturado hoje: {historicoVendas.Sum(hv => hv.ValorVenda):C2}");
    }
    public void MensagemSucesso(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(mensagem);
        Console.ResetColor();
    }
    public void MensagemErro(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(mensagem);
        Console.ResetColor();
    }
    public void MensagemAviso(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(mensagem);
        Console.ResetColor();
    }
    public void MensagemInfo(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(mensagem);
        Console.ResetColor();
    }
    public bool PerguntarContinuar(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            string opcao = Console.ReadLine().ToUpper();

            if (opcao == "S") return true;
            if (opcao == "N") return false;

            MensagemErro("Erro: Digite apenas S ou N.");
        }
    }


}
public class Venda
{
    public string NomeItem { get; private set; }
    public int QuantidadeVendida { get; private set; }
    public decimal ValorVenda { get; private set; }
    public DateTime Data { get; private set; }
    public Venda(string nome, int quantidade, decimal valor)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Erro: Nome do item não pode ser vazio.");
        if (quantidade <= 0) throw new ArgumentException("Erro: Quantidade de venda não deve receber um valor negativo.");
        if (valor < 0) throw new ArgumentException("Erro: Valor da venda não deve receber um valor negativo.");
        NomeItem = nome;
        QuantidadeVendida = quantidade;
        ValorVenda = valor;
        Data = DateTime.Now;
    }
    public override string ToString()
    {
        return $"Nome: {NomeItem.PadRight(15)} | Quantidade vendida: {QuantidadeVendida} | Valor total da venda: {ValorVenda:c} - {Data}";
    }
}

class Program
{
    public static void Main()
    {
        var servico = new LojaService();
        bool continuar = true;

        while (continuar)
        {
            Console.Clear();
            Console.WriteLine("=== Loja: A Doninha Encantada - Menu ===\n");
            Console.WriteLine("1. Cadastrar ítem");
            Console.WriteLine("2. Atualizar preço");
            Console.WriteLine("3. Repor estoque");
            Console.WriteLine("4. Vender ítem");
            Console.WriteLine("5. Relatório de estoque");
            Console.WriteLine("6. Relatório de vendas");
            Console.WriteLine("7. Fechamento de caixa");
            Console.WriteLine("0. Sair do sistema");
            Console.Write("\nEscolha uma opção: ");

            string opcao = Console.ReadLine();




            switch (opcao)
            {
                case "1":
                    Console.Clear();
                    servico.MenuCadastrarItem();
                    break;
                case "2":
                    servico.AtualizarPrecoItem();

                    break;
                case "3":
                    servico.ReporEstoqueItem();
                    break;

                case "4":
                    servico.VenderItem();
                    break;
                case "5":
                    Console.Clear();
                    servico.RelatarEstoque();
                    Console.ReadKey();
                    break;
                case "6":

                    servico.RelatarHistoricoVendas();
                    Console.ReadKey();
                    break;
                case "7":
                    servico.FecharCaixa();
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

