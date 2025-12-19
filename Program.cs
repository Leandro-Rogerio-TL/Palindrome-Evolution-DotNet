using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;

namespace PalindromeBenchmark
{
    // Configuração para mostrar memória e ordenar do mais rápido para o mais lento
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class PalindromeComparativo
    {
        // Uma string longa e complexa para estressar o algoritmo
        private const string Data = "abcdef1234567890zyxwvutsrqpmlkjihgfedcba" + 
                                "anavalvalentinamanunaluana" + // Ruído com padrões próximos
                                "socorrammesubinoonibusemmarrocos" + // O palíndromo clássico (limpo)
                                "0987654321fedcbaxxxxxxxxxxxxxxxxxxxxxxxx" + // Ruído para forçar busca
                                "finalteste0123456789";

        // Instância da classe antiga
        private readonly SolucaoAntiga _solucaoAntiga = new SolucaoAntiga();

        [Benchmark(Baseline = true, Description = "Fev/2025: Legado (Substring)")]
        public string BenchmarkAntigo()
        {
            return _solucaoAntiga.LongestPalindrome(Data);
        }

        [Benchmark(Description = "Dez/2025: Otimizado (Span + Poda)")]
        public string BenchmarkNovo()
        {
            return SolucaoNova.MaiorSubpalindromo(Data);
        }
    }

    // ==========================================
    // CLASSE 1: Solução de Fevereiro/2025 (Antiga)
    // ==========================================
    public class SolucaoAntiga
    {
        public string LongestPalindrome(string s)
        {
            string subResp = "";
            int ini = 0;
            int fim = s.Length - 1;
            if (fim == 0) subResp = s;
            if (fim >= 1)
            {
                subResp = s[0].ToString(); // Alocação na Heap
                while (ini + subResp.Length <= fim)
                {
                    int i = ini;
                    int cont = 0;
                    int tempFim = fim; // Ajuste para não perder referência do fim original no loop interno

                    while (i < tempFim)
                    {
                        if (s[i] == s[tempFim])
                        {
                            i++;
                            cont++;
                        }
                        else
                        {
                            i = ini;
                            cont = 0;
                        }
                        tempFim--;
                    }
                    if (cont > 0)
                    {
                        if (subResp.Length <= ini + tempFim + cont)
                        {
                            // AQUI É O GARGALO: Substring aloca nova string na Heap
                            subResp = s.Substring(ini, tempFim - ini + cont + 1);
                        }
                    }
                    fim = s.Length - 1;
                    ini++;
                }
            }
            return subResp;
        }
    }

    // ==========================================
    // CLASSE 2: Solução de Dezembro/2025 (Nova)
    // ==========================================
    public class SolucaoNova
    {
        public static string MaiorSubpalindromo(string str)
        {
            if (str.Length < 4) return "none"; // Regra do teste

            int centroEsq = str.Length / 2;
            int centroDir = centroEsq + 1;
            int iniPalindromo = 0;
            int comprPalindromo = 0;

            // Poda inteligente e Span
            while (centroEsq > 0 && centroEsq >= comprPalindromo / 2)
            {
                var (inicio, comprimento) = ExpandirNoCentro(str.AsSpan(), centroEsq);
                if (comprimento >= comprPalindromo)
                {
                    iniPalindromo = inicio;
                    comprPalindromo = comprimento;
                }
                centroEsq--;
            }

            while (centroDir < str.Length - 1 && centroDir + comprPalindromo / 2 < str.Length)
            {
                var (inicio, comprimento) = ExpandirNoCentro(str.AsSpan(), centroDir);
                if (comprimento > comprPalindromo)
                {
                    iniPalindromo = inicio;
                    comprPalindromo = comprimento;
                }
                centroDir++;
            }

            if (comprPalindromo >= 3)
                return str.Substring(iniPalindromo, comprPalindromo);
            else
                return "none";
        }

        private static (int inicio, int comprimento) ExpandirNoCentro(ReadOnlySpan<char> s, int centro)
        {
            int esq = centro - 1;
            int dir = centro + 1;

            if (esq >= 0 && s[esq] == s[centro]) dir = centro;
            else if (dir < s.Length && s[centro] == s[dir]) esq = centro;

            while (esq >= 0 && dir < s.Length && s[esq] == s[dir])
            {
                esq--;
                dir++;
            }
            return (esq + 1, dir - esq - 1);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Rodando Benchmark... (Isso pode levar 1 ou 2 minutos)");
            BenchmarkRunner.Run<PalindromeComparativo>();
        }
    }
}