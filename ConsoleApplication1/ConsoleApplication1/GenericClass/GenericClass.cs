
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class GenericClass: SearchBase
    {

     

        public virtual void ProcessData(string webPage,int datascourceId)
        {

            throw new NotImplementedException();
        }
        public string convertSingleSpaces(string Data)
        {
            Data = Regex.Replace(Data, @"\t|\n|\r", "");
             Data = Regex.Replace(Data, @"\s+", " ");
            return Data;
        }
        public string TrimData(string txtData)
        {
            if (txtData.Trim().Length > 0)
            {
                for (int i = 0; i < txtData.Length; i++)
                {
                    txtData = txtData.Trim().TrimStart(new char[] { '-', '.', ':', ',', '&', '*', ';' }).Trim();
                    txtData = txtData.Trim().TrimEnd(new char[] { '-', '.', ':', ',', '&', '*', '(', ';' }).Trim();

                }
            }
            return txtData;
        }
        public string GetFirstWord(string webPageContent)
        {

            var matches = Regex.Matches(webPageContent, @"\w+[^\s]*\w+|\w");
            foreach (var match in matches)
            {
                if (match.ToString().Trim().Length > 1)
                {
                    return match.ToString();

                }


            }
            return string.Empty;

        }
        public String TitleCaseString(String s)
        {
            if (s == null || s.Trim().Length == 0) return s;

            String[] words = s.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length == 0) continue;

                Char firstChar = Char.ToUpper(words[i][0]);
                String rest = "";
                if (words[i].Length > 1)
                {
                    rest = words[i].Substring(1).ToLower();
                }
                words[i] = firstChar + rest;
            }
            return String.Join(" ", words);
        }

        public void PrintProgram()
        {
               for (int b = 0; b < 20; b++)
            {



                int i, j, n;
                //Console.WriteLine("Enter no for printing star in diamond shape");
                n = b;// int.Parse(Console.ReadLine());
                for (i = 1; i <= n; i++)
                {
                    for (j = n; j >= i; j--)
                    {
                        Console.Write(" ");
                    }
                    for (j = 1; j <= i; j++)
                    {
                        Console.Write("*");
                    }
                    for (j = 1; j <= i; j++)
                    {
                        Console.Write("*");
                    }
                    Console.WriteLine();
                }

                for (i = 1; i <= n; i++)
                {
                    for (j = 0; j <= i; j++)
                    {
                        Console.Write(" ");
                    }
                    for (j = n; j >= i; j--)
                    {
                        Console.Write("*");
                    }
                    for (j = n; j >= i; j--)
                    {
                        Console.Write("*");
                    }
                    Console.WriteLine();
                }
                Thread.Sleep(100);

                if (b == 19)
                    b = 0;
               // Console.ReadKey();
            }
        }
    }
}
