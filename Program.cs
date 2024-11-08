using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static bool AlfabeKontrolu(string alfabe, string regex)
    {
        foreach (char c in regex)
        {
            if (!alfabe.Contains(c) && c != '*' && c != '+' && c != '(' && c != ')')
            {
                return false;
            }
        }
        return true;
    }

    static List<string> DilUretici(string regex, string alfabe, int kelimeSayisi, int maxUzunluk)
    {
        var kelimeler = new HashSet<string>();

        for (int uzunluk = 1; uzunluk <= maxUzunluk; uzunluk++)
        {
            var kombinasyonlar = GetCombinations(alfabe, uzunluk);
            foreach (var kelime in kombinasyonlar)
            {
                if (DüzenliIfadeUyumu(regex, kelime))
                {
                    kelimeler.Add(kelime);
                    if (kelimeler.Count >= kelimeSayisi)
                        return kelimeler.ToList();
                }
            }
        }

        return kelimeler.ToList();
    }

    static List<string> GetCombinations(string alfabe, int maxLength)
    {
        var result = new List<string>();

        for (int length = 1; length <= maxLength; length++)
        {
            var combinations = new char[length];
            GenerateCombinations(alfabe, combinations, 0, result);
        }

        return result;
    }

    static void GenerateCombinations(string alfabe, char[] combinations, int index, List<string> result)
    {
        if (index == combinations.Length)
        {
            result.Add(new string(combinations));
            return;
        }

        foreach (var ch in alfabe)
        {
            combinations[index] = ch;
            GenerateCombinations(alfabe, combinations, index + 1, result);
        }
    }

    static bool DüzenliIfadeUyumu(string regex, string kelime)
    {
        int r = 0, k = 0;

        while (r < regex.Length && k < kelime.Length)
        {
            if (regex[r] == '(')
            {
                int closingParenthesis = regex.IndexOf(')', r);
                if (closingParenthesis == -1) return false;

                string group = regex.Substring(r + 1, closingParenthesis - r - 1);
                r = closingParenthesis + 1;

                if (r < regex.Length && (regex[r] == '*' || regex[r] == '+'))
                {
                    bool atLeastOne = regex[r] == '+';
                    int count = 0;

                    while (k < kelime.Length && kelime.Substring(k).StartsWith(group) && count < 300)
                    {
                        k += group.Length;
                        count++;
                    }

                    if (atLeastOne && count == 0) return false;
                    r++;
                }
                else if (kelime.Substring(k).StartsWith(group))
                {
                    k += group.Length;
                }
                else
                {
                    return false;
                }
            }
            else if (r + 1 < regex.Length && (regex[r + 1] == '*' || regex[r + 1] == '+'))
            {
                char ch = regex[r];
                bool atLeastOne = regex[r + 1] == '+';
                int count = 0;

                while (k < kelime.Length && kelime[k] == ch && count < 300)
                {
                    k++;
                    count++;
                }

                if (atLeastOne && count == 0) return false;
                r += 2;
            }
            else
            {
                if (regex[r] != kelime[k])
                    return false;

                r++;
                k++;
            }
        }

        return r == regex.Length && k == kelime.Length;
    }

    static bool LDilindeMi(string regex, string kelime)
    {
        return DüzenliIfadeUyumu(regex, kelime);
    }

    static void Main()
    {
        Console.WriteLine("Alfabeyi giriniz (örneğin: a,b,c):");
        string alfabe = Console.ReadLine().Replace(",", "").Replace(" ", "");

        Console.WriteLine("Parantez kullanmadan düzenli ifadeyi giriniz(örneğin: a+b*c):");
        string regex = Console.ReadLine();

        if (AlfabeKontrolu(alfabe, regex))
        {
            Console.WriteLine("Düzenli ifade S alfabesinden üretilebilir.");
            Console.WriteLine("L dilinin kaç kelimesini görmek istiyorsunuz?");
            int kelimeSayisi = int.Parse(Console.ReadLine());

            var kelimeler = DilUretici(regex, alfabe, kelimeSayisi, maxUzunluk: 10);
            Console.WriteLine("L kelimeleri:");
            foreach (var kelime in kelimeler)
            {
                Console.WriteLine(kelime);
            }
            Console.WriteLine(kelimeler.Count());
            Console.WriteLine("Kontrol edilecek kelimeyi giriniz:");
            string kontrolKelime = Console.ReadLine();
            if (LDilindeMi(regex, kontrolKelime))
            {
                Console.WriteLine("Bu kelime L diline aittir.");
            }
            else
            {
                Console.WriteLine("Bu kelime L diline ait değildir.");
            }
        }
        else
        {
            Console.WriteLine("Düzenli ifade S alfabesinden üretilemez.");
        }
    }
}
