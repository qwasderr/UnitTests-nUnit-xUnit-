using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework.Internal;
using nUnit1;
using System.Drawing;
using System.Reflection.PortableExecutable;
namespace TestProject1
{
    public class Tests
    {
        private Solver _solver;
        private string path;
        private Dictionary<string, bool> test;
        private Dictionary<string,List<string>> matchers_test;
        private Dictionary<string, string> letters_test;
        public Dictionary<string, bool> Init()
        {
            test = new Dictionary<string, bool>
            {
                { "aA", false },
                { "a", true },
                { "b", true },
                { "AC", true },
                { "AA", false }
            };
            return test;
        }
        public Dictionary<string, List<string>> Init_matchers()
        {
            matchers_test = new Dictionary<string, List<string>>
            {
                { "  AAA AB$BCD++EA-Bb ", new List<string> { "AB", "BCD", "EA" } },
                { "AbC4597Ddi8Dcf", new List<string> { "AbC", "Dcf" } },
                { "b83dc5)Cvf", new List<string> { "b", "dc", "Cvf" } },
                { " Kfg  fF)((Fgv", new List<string> { "Kfg", "Fgv" } },
                { "dfd95Dcv_dxz3ase8ed", new List<string> { "Dcv", "dxz", "ase", "ed" } }
            };
            return matchers_test;
        }
        public Dictionary<string, string> Letters_Init()
        {
            letters_test = new Dictionary<string, string>
            {
                { "aA5656D", "5656D" },
                { "aCDC$DD", "$DD" },
                { "azs%%SSD", "%%SSD" },
                { "AAAAJF^DVF", "^DVF" },
                { "avcf3S324#", "3S324#" },
                {"asc","" }
            };
            return letters_test;
        }
        public void Write(string s) 
        {
            using (StreamWriter file = new StreamWriter(path))
            {
                file.Write(s);
            }
        }
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            path = "C:\\Users\\Max\\Desktop\\test.txt";
            test = Init();
            matchers_test = Init_matchers();
            letters_test = Letters_Init();

        }
        [SetUp]
        public void Setup()
        {
            _solver = new Solver();
        }
        [Test]
        [TestCase('A', ExpectedResult = true)]
        [TestCase('z', ExpectedResult = true)]
        [TestCase('0', ExpectedResult = false)]
        [TestCase('9', ExpectedResult = false)]
        [TestCase('@', ExpectedResult = false)]
        [TestCase('~', ExpectedResult = false)]
        public bool IsNumAscii_InputChar_ReturnsExpectedResult(char input)
        {
            return _solver.IsNumAscii(input);
        }
    
    [Test]
        public void NonExistingFile()
        {
            string testpath = "1.txt";
            Assert.Throws<FileNotFoundException>(() => _solver.Solve(testpath));
        }
        [Test]
        [TestCase("abcde", true)]
        [TestCase("abcda", false)]
        [TestCase("aBcdeFghijklmnopQrstuvwXYZ", true)]
        [TestCase("ABCA", false)]
        public void Check_Word_Test(string word, bool expectedResult)
        {
            bool result = _solver.Check(word);
            Assert.AreEqual(expectedResult, result);
        }
        [Test]
        [TestCase("   AAAB", "AAAB")] 
        [TestCase("95^^ABCd", "ABCd")]
        [TestCase("&&$$**^**Asdj", "Asdj")]
        public void SymSkip_SkipsNonAsciiCharacters_Correctly(string test, string expectedString)
        {
            Write(test);
            using (StreamReader file = new StreamReader(path))
            {
                _solver.SymSkip(file);
                Assert.AreEqual(expectedString, file.ReadLine());
            }
        }
        [Test]
        public void Check_Word_Test_2()
        {
            foreach (var test in test)
            {
                if (test.Value == true)
                {
                    Assert.IsTrue(_solver.Check(test.Key));
                }
                else
                {
                    Assert.IsFalse(_solver.Check(test.Key));
                }
            }
        }
        [Test]
        [TestCase(1000)]
        [TestCase(5000)]
        [TestCase(10000)]
        public void Generate_CreatesFileWithExpectedSize(int size)
        {
            _solver.Generate(size,path);
            FileInfo fileInfo = new FileInfo(path);
            Assert.IsTrue(fileInfo.Exists, "File does not exist.");
            Assert.AreEqual(size, fileInfo.Length, $"File size is not equal to {size}.");
        }
        [Test]
        public void Solve_matchers()
        {
            foreach (var test in matchers_test)
            {
                Write(test.Key);
                Assert.That(_solver.Solve(path), Has.Exactly(test.Value.Count).Items);
                Assert.That(_solver.Solve(path), Is.EqualTo(test.Value));
            }
           
        }
        [Test]
        public void Letters_Skip_Test()
        {
            foreach (var test in letters_test)
            {
                Write(test.Key);
                using (StreamReader file = new StreamReader(path))
                {
                    _solver.LettersSkip(file);
                    string result = file.ReadToEnd() ?? "";
                    Assert.That(result, Is.EqualTo(test.Value));
                }
                
            }

        }
        [TestCaseSource(nameof(GetWordTestData))]
        public void GetWord_ReturnsExpectedWord(string input, string expected)
        {
            Assume.That(input, Does.Match(@"^(?:[A-Za-z]|$)").IgnoreCase, message: "Data isn't correct");
            Write(input);
            using (StreamReader file = new StreamReader(path))
            {
                string result = _solver.GetWord(file);
                Assert.That(result, Is.EqualTo(expected));
            }
        }

        private static IEnumerable<TestCaseData> GetWordTestData()
        {
            yield return new TestCaseData("word1 word2", "word");
            yield return new TestCaseData("12345 abcde", "");
            yield return new TestCaseData("abc def", "abc");
            yield return new TestCaseData("symbol@ symbol#", "symbol");
            yield return new TestCaseData("", "");
            yield return new TestCaseData("ACgd%487498thj", "ACgd");
        }
    }
}