using System;

public class ItemRPG
{
    public string Nome { get; private set; } 
    public decimal Preco { get; private set; }
    public int Estoque { get; private set; }
    public ItemRPG(string nome, decimal preco)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentNullException("Erro: Nome do item não pode ser vazio!");
        }
        if (preco < 0)
        {
            throw new ArgumentNullException("Erro: Preço não pode ser negativo!");
        }
        Nome = nome;
        Preco = preco;

    }
    public void RealizarVenda (int quantidade)
    {
        Console.WriteLine($"> Tentando vender {quantidade} unidades...");
        if (quantidade < 0)
        {
            Console.WriteLine("Erro: Quantidade não pode ser negativa!");
        }
        else if (quantidade > Estoque)
        {
            Console.WriteLine("Erro: Estoque insuficiente.");
        }
        else
        {
            Estoque -= quantidade;
            Console.WriteLine($"Venda Realizada! Estoque restante: {Estoque}");
        }
    }
    public void Reabastecer (int quantidade)
    {
        Console.WriteLine($"> Tentando reabastecer {quantidade} unidades...");
        if (quantidade < 0)
        {
            Console.WriteLine("ERRO: Quantidade não pode ser negativa.");
        }
        else
        {
            Estoque += quantidade;
            Console.WriteLine($"Estoque atualizado: {Estoque}");
        }
    }
}
class Principal
{
    public static void Main(string[] args)
    {
        Console.WriteLine("--- RPG SHOP ---");
        Console.WriteLine("Criando item 'Espada Suprema'...");
        var novoItem = new ItemRPG("Espada Suprema", 1000m);
        //novoItem.Nome = "Espada Alternativa"; // Nome é private set, só pode ser alterado dentro da classe 
        Console.WriteLine($"Sucesso! Preço: {novoItem.Preco:c}, Estoque inicial: {novoItem.Estoque}");
        Console.WriteLine();
        novoItem.Reabastecer(10);
        Console.WriteLine();
        novoItem.RealizarVenda(5);
        Console.WriteLine();
        novoItem.RealizarVenda(50);
        Console.WriteLine();
        Console.WriteLine("-- ESTADO FINAL--");
        Console.WriteLine($"Item: {novoItem.Nome}\nPreço: {novoItem.Preco:c}\nEstoque: {novoItem.Estoque}");
    }
}