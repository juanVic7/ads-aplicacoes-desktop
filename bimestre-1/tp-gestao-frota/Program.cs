using System;

namespace GestaoFrota;

public interface IManutencao
{
    decimal CalcularCustoRevisao();
}
public abstract class Veiculo : IManutencao
{
    public string Placa { get; set; }
    public decimal Tanque { get; protected set; }

    public Veiculo(string placa, decimal combustivel)
    {
        if (combustivel < 0) throw new ArgumentException("O tanque não pode ser negativo.");
        Placa = placa;
        Tanque = combustivel;
    }
    public abstract decimal CalcularCustoRevisao();
    public abstract void Viajar(decimal distanciaKm);
    public abstract decimal CalcularPedagio();
}
public class Carro : Veiculo
{
    public Carro(string placa, decimal combustivel) : base(placa, combustivel) { }
    public override void Viajar(decimal distanciaKm)
    {
        if ((Tanque - (distanciaKm / 10m)) < 0) throw new InvalidOperationException("Combustível insuficiente para o carro.");
        Tanque -= distanciaKm / 10m;
    }
    public override decimal CalcularPedagio()
    {
        return 8.50m;
    }
    public override decimal CalcularCustoRevisao()
    {
        return 300.0m;
    }
}

public class Caminhao : Veiculo
{
    public int QuantidadeEixos { get; private set; }
    public Caminhao(string placa, decimal combustivel, int qtEixos) : base(placa, combustivel)
    {
        if (qtEixos < 0) throw new InvalidOperationException("A quantidade de eixos não pode ser negativa.");
        QuantidadeEixos = qtEixos;
    }
    public override void Viajar(decimal distanciaKm)
    {
        if ((Tanque - (distanciaKm / 4m)) < 0) throw new InvalidOperationException("Combustível insuficiente para o caminhão.");
        Tanque -= distanciaKm / 4m;
    }
    public override decimal CalcularPedagio()
    {

        return 8.50m * QuantidadeEixos;
    }
    public override decimal CalcularCustoRevisao()
    {
        return 1500.0m;
    }
}
class Program
{
    /* MÁGICA DO POLIMORFISMO: Este método aceita QUALQUER veículo (Carro ou
    Caminhão) */
    static void SimularViagemPolimorfica(Veiculo v, decimal quilometros)
    {
        try
        {
            Console.WriteLine($"\n--- A INICIAR VIAGEM COM O VEÍCULO: {v.Placa} ---");
            /* O C# decide sozinho qual regra de viagem aplicar */
            v.Viajar(quilometros);
            Console.WriteLine($"Sucesso! Restou no tanque: {v.Tanque:F2} Litros");
            Console.WriteLine($"Custo de Portagem (Pedágio): {v.CalcularPedagio():C}");
            Console.WriteLine($"Previsão de Revisão na Oficina: {v.CalcularCustoRevisao():C} ");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ALERTA NA VIAGEM: {ex.Message}");
        }
    }
    static void Main()
    {
        Console.WriteLine("--- CADASTRO DA FROTA ---");
        /* 1. Cadastrando o Carro */
        Console.WriteLine("\n[A] Configurar o Carro do Gerente");
        Console.Write("Digite a Placa: ");
        string placaCarro = Console.ReadLine();
        Console.Write("Combustível Inicial (Litros): ");
        decimal tanqueCarro = decimal.Parse(Console.ReadLine());
        Carro carroGerente = null;
        try
        {
            carroGerente = new Carro(placaCarro, tanqueCarro);
            Console.WriteLine("Carro registado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO NO REGISTO: {ex.Message}");
        }
        /* 2. Cadastrando o Caminhão */
        Console.WriteLine("\n[B] Configurar o Caminhão de Carga");
        Console.Write("Digite a Placa: ");
        string placaCam = Console.ReadLine();
        Console.Write("Combustível Inicial (Litros): ");
        decimal tanqueCam = decimal.Parse(Console.ReadLine());
        Console.Write("Quantidade de Eixos: ");
        int eixosCam = int.Parse(Console.ReadLine());
        Caminhao caminhaoCarga = null;
        try
        {
            caminhaoCarga = new Caminhao(placaCam, tanqueCam, eixosCam);
            Console.WriteLine("Caminhão registado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO NO REGISTO: {ex.Message}");
        }
        /* 3. A Simulação da Viagem */
        Console.WriteLine("\n--- SIMULAÇÃO DE ROTA ---");
        Console.Write("Qual a distância do trajeto (Km)? ");
        decimal distancia = decimal.Parse(Console.ReadLine());
        /* Enviamos os dois veículos diferentes para o mesmo método polimórfico */
        if (carroGerente != null)
        {
            SimularViagemPolimorfica(carroGerente, distancia);
        }
        if (caminhaoCarga != null)
        {
            SimularViagemPolimorfica(caminhaoCarga, distancia);
        }
        Console.WriteLine("\nSimulação concluída. Pressione ENTER para sair.");
        Console.ReadLine();
    }
}