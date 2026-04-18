using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace SpaceInvadersADS
{
    abstract class Entidade
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Simbolo { get; set; }
        public ConsoleColor Cor { get; set; }

    }

    class Nave : Entidade
    {
        public int Vidas { get; set; }

        public Nave(int larguraTela)
        {
            X = larguraTela / 2;
            Y = 18;
            Simbolo = "A";
            Cor = ConsoleColor.Yellow;
            Vidas = 3;
        }

        public void MoverDireita(int larguraTela)
        {
            X = Math.Min(larguraTela - 1, X + 1);
        }

        public void MoverEsquerda()
        {
            X = Math.Max(0, X - 1);
        }

        public void PerderVida()
        {
            Vidas--;
        }

        public bool EstaVivo => Vidas > 0;
    }

    class Alien : Entidade
    {
        public Alien(int x, int y)
        {
            X = x;
            Y = y;
            Simbolo = "TT";
            Cor = ConsoleColor.Green;
        }
    }

    class Tiro : Entidade
    {
        public Tiro(int x, int y)
        {
            X = x;
            Y = y;
            Simbolo = "|";
            Cor = ConsoleColor.White;
        }

        public void Avancar()
        {
            Y--;
        }
    }

    class Program
    {
        const int LARGURA = 50;
        const int ALTURA = 20;
        const int FPS_DEALY_MS = 17;

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
#pragma warning disable CA1416
            Console.SetWindowSize(LARGURA, ALTURA + 3);

            Console.SetBufferSize(LARGURA, ALTURA + 3);
#pragma warning restore CA1416

            var nave = new Nave(LARGURA);
            var aliens = new List<Alien>();
            var tiros = new List<Tiro>();

            for (int linha = 1; linha <= 2; linha++)
            {
                for (int col = 4; col < LARGURA - 4; col += 5)
                {
                    aliens.Add(new Alien(col, linha));
                }
            }

            bool jogando = true;
            int pontos = 0;

            int direcaoAliens = 1;
            int contadorMovimentoAlien = 0;
            int maxTirosSimultaneos = 3;
            DateTime ultimoTiro = DateTime.MinValue;

            while (jogando)
            {
                if (Console.KeyAvailable)
                {
                    var tecla = Console.ReadKey(true).Key;

                    if (tecla == ConsoleKey.LeftArrow)
                    {
                        nave.MoverEsquerda();
                    }
                    if (tecla == ConsoleKey.RightArrow)
                    {
                        nave.MoverDireita(LARGURA);
                    }
                    if (tecla == ConsoleKey.Spacebar && tiros.Count < maxTirosSimultaneos && (DateTime.Now - ultimoTiro).TotalMilliseconds > 180)
                    {
                        tiros.Add(new Tiro(nave.X, nave.Y - 1));
                        ultimoTiro = DateTime.Now;
                        Console.Beep(1200, 60);
                    }

                    if (tecla == ConsoleKey.Escape)
                    {
                        jogando = false;
                    }
                }

                foreach (var t in tiros)
                {
                    t.Avancar();

                }
                tiros.RemoveAll(t => t.Y < 0);

                contadorMovimentoAlien++;

                if (contadorMovimentoAlien >= 12)
                {
                    contadorMovimentoAlien = 0;

                    bool deveDescer = false;

                    foreach (var a in aliens)
                    {
                        int novoX = a.X + direcaoAliens;

                        if (novoX <= 1 || novoX >= LARGURA - 2)
                        {
                            deveDescer = true;
                            break;
                        }


                    }

                    if (deveDescer)
                    {
                        direcaoAliens *= -1;
                        foreach (var a in aliens)
                        {
                            a.Y++;
                        }
                    }
                    else
                    {
                        foreach (var a in aliens)
                        {
                            a.X += direcaoAliens;
                        }
                    }
                }

                foreach (var tiro in tiros.ToList())
                {
                    var atingido = aliens.Find(aliens => aliens.X == tiro.X && aliens.Y == tiro.Y);

                    if (atingido != null)
                    {
                        aliens.Remove(atingido);
                        tiros.Remove(tiro);
                        pontos += 100;
                        Console.Beep(800, 80);
                    }
                }

                if (aliens.Any(a => a.Y >= nave.Y - 1))
                {
                    nave.PerderVida();

                    if (!nave.EstaVivo)
                    {
                        jogando = false;
                        MostrarTelaFinal("GAME OVER", pontos, nave.Vidas);
                        continue;
                    }

                    foreach(var a in aliens)
                    {
                        a.Y = Math.Max(1, a.Y - 3);
                    }
                }
                if(aliens.Count == 0)
                {
                    MostrarTelaFinal("VOCE VENCEU!!", pontos, nave.Vidas);
                    jogando = false;
                    continue;
                }

                Console.Clear();

                Desenhar(nave.X, nave.Y, nave.Simbolo, nave.Cor);

                foreach(var a in aliens)
                {
                    Desenhar(a.X, a.Y, a.Simbolo, a.Cor);
                }

                foreach (var t in tiros)
                {
                    Desenhar(t.X, t.Y, t.Simbolo, t.Cor);
                }

                Console.SetCursorPosition(1, ALTURA);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"Pontos: {pontos,-6}, vidas: *");
                Console.ForegroundColor= ConsoleColor.Red;
                for (int i = 0; i < nave.Vidas; i++)
                {
                    Console.Write(" S2");
                }
                Console.ResetColor();

                Thread.Sleep(FPS_DEALY_MS);
            }
        }
        static void Desenhar( int x, int y, string simbolo, ConsoleColor cor)
        {
            if(x < 0 || x >= LARGURA || y < 0 || y >= ALTURA)
            {
                return;
            }

            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = cor;
            Console.Write(simbolo);
            Console.ResetColor();
        }

        static void MostrarTelaFinal(string titulo, int pontos, int vidas)
        {
            Console.Clear();
            Console.ForegroundColor= ConsoleColor.Yellow;
            Console.SetCursorPosition(8, 6);
            Console.WriteLine(titulo);
            Console.SetCursorPosition(8, 6);
            Console.WriteLine($"Pontuação final: {pontos}");
            Console.SetCursorPosition(8, 9);
            Console.WriteLine($"Vidas restantes: {vidas}");
            Console.ResetColor();
            Console.SetCursorPosition(8, 12);
            Console.WriteLine("Presioni enti");

        }

    }

}