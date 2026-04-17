using System;


namespace Tp1;

public static class Program
{
    public static void Main()
    {

        string nomeFuncionario = "", resposta;
        decimal salarioBruto, salarioLiquido, descontoInss, descontoIrrf;


        do
        {
            Console.WriteLine("---FOLHA DE PAGAMENTO---");

            Console.Write("Digite o seu nome: ");
            nomeFuncionario = Console.ReadLine();

            salarioBruto = ValidarSalario("Digite o salário bruto: ");


            Console.WriteLine("processando...");
            descontoInss = calculoINSS(salarioBruto);
            descontoIrrf = calculoIRRF(salarioBruto - descontoInss);
            salarioLiquido = salarioBruto - descontoInss - descontoIrrf;

            holerite(nomeFuncionario, salarioBruto, salarioLiquido, descontoInss, descontoIrrf);

            Console.Write("> Novo cálculo? [S/N]: ");
            do
            {
                resposta = Console.ReadLine().ToUpper();
                if (resposta != "S" && resposta != "N")
                {
                    Console.WriteLine("Resposta inválida, digite apenas S ou N");
                }


            } while (resposta != "S" && resposta != "N");
            Console.Clear();


        } while (resposta == "S");



        Console.ReadKey();
    }
    static decimal calculoINSS(decimal salario)
    {
        decimal descontoInss;
        if (salario <= 1300m)
        {
            descontoInss = salario * 0.075m;
        }
        else if (salario <= 2500m)
        {
            descontoInss = salario * 0.09m;
        }
        else if (salario <= 3900m)
        {
            descontoInss = salario * 0.12m;
        }
        else
        {
            descontoInss = salario * 0.14m;
        }
        return descontoInss;
    }
    static decimal calculoIRRF(decimal salarioBase)
    {
        return (salarioBase > 1900m) ? salarioBase * 0.075m : 0m;
    }
    static void holerite(string nome, decimal salarioBruto, decimal salarioLiquido, decimal inss, decimal irrf)
    {
        Console.WriteLine("==========================");
        Console.WriteLine($"FOLHA: {nome.ToUpper()}");
        Console.WriteLine($"(+) Salário Bruto: {salarioBruto:c}");
        Console.WriteLine($"(-) Desconto INSS: {inss:c}");
        Console.WriteLine($"(-) Desconto IRRF: {irrf:c}");
        Console.WriteLine("==========================");
        Console.WriteLine($"(=) Salário Líquido: {salarioLiquido:c}");

    }
    static decimal ValidarSalario(string mensagem)
    {
        bool ok;
        decimal salario;

        do
        {
            Console.Write(mensagem);
            string sSalario = Console.ReadLine();
            ok = decimal.TryParse(sSalario, out salario);
            if (!ok || salario <= 0m)
            {
                Console.WriteLine("Salário inválido, digite novamente.");
            }
        }
        while (!ok || salario <= 0m);
        return salario;
    }
}
