using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace nUnit1
{
    public class Solver
    {
        const int CLS = 65;
        const int CLE = 90;
        const int SLS = 97;
        const int SLE = 122;
        public void Generate(int size,string Path)
        {
            Random rand = new Random();
            char sym;
            //const int size = 5000;
            using (StreamWriter file = new StreamWriter(Path))
            {
                for (int i = 1; i <= size; ++i)
                {
                    sym = (char)(rand.Next(32, 127));
                    //Console.WriteLine(sym);
                    file.Write(sym);
                }
            }
        }
        public bool IsNumAscii(char a)
        {
            return ((a >= CLS && a <= CLE) || (a >= SLS && a <= SLE));
        }
        /*static void SymSkip(StreamReader f)
        {
            char a = ' ';
            var ff = f.BaseStream.Position;
            while (!IsNumAscii(a))
            {
                a = (char)f.Read();
            }
            f.BaseStream.Seek(f.BaseStream.Position - 1, SeekOrigin.Begin);
        }
        static void LettersSkip(StreamReader f)
        {
            char a = 'a';
            while (IsNumAscii(a))
            {
                a = (char)f.Read();
            }
            f.BaseStream.Seek(f.BaseStream.Position - 1, SeekOrigin.Begin);
        }*/
        public void SymSkip(StreamReader f)
        {
            char a = ' ';
            while (!f.EndOfStream && !IsNumAscii((char)f.Peek()))
            {
                f.Read();
            }
        }
        public void LettersSkip(StreamReader f)
        {
            while (!f.EndOfStream && IsNumAscii((char)f.Peek()))
            {
                f.Read();
            }
        }
        public string GetWord(StreamReader f)
        {
            char[] temp = new char[35];
            var ff = f.BaseStream.Position;
            int letters = 1;
            temp[0] = (char)f.Read();
            while (letters <= 30 && IsNumAscii(temp[letters - 1]))
            {
                temp[letters] = (char)f.Read();
                ++letters;
            }
            if (letters > 30)
            {
                LettersSkip(f);
            }
            temp[letters - 1] = '\0';
            return new string(temp).TrimEnd('\0');
        }
        public bool Check(string temp)
        {
            if (temp.Length > 26)
            {
                return false;
            }
            HashSet<char> counter = new HashSet<char>();
            foreach (char c in temp.ToLower())
            {
                if (counter.Contains(c))
                {
                    return false;
                }
                counter.Add(c);
            }
            return true;
        }
        public List<string> Solve(string Path)
        {
            //Generate(Path);
            List<string> list = new List<string>();
            using (StreamReader file = new StreamReader(Path))
            {

                string buffer;
                while (!file.EndOfStream)
                {
                    SymSkip(file);
                    buffer = GetWord(file);
                    if (Check(buffer))
                    {
                       list.Add(buffer);
                    }
                }
                
            }
            return list;
        }
        public static void Main(string[] args)
        {
            Solver solver = new Solver();
            //solver.Solve("C:\\Users\\Max\\Desktop\\testtttt.txt");
        }
    }
}
