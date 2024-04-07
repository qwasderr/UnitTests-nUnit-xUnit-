using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using nUnit1;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
namespace TestProject2
{
    public class BeforeAll
    {
        public Dictionary<string, bool> test;
        public Dictionary<string, List<string>> matchers_test;
        public Dictionary<string, string> letters_test;
        public BeforeAll()
        {
            test = Init();
            matchers_test = InitMatchers();
            letters_test = LettersInit();
        }
        private static Dictionary<string, bool> Init()
        {
            return new Dictionary<string, bool>
            {
                { "aA", false },
                { "a", true },
                { "b", true },
                { "AC", true },
                { "AA", false }
            };
        }

        private static Dictionary<string, List<string>> InitMatchers()
        {
            return new Dictionary<string, List<string>>
            {
                { "  AAA AB$BCD++EA-Bb ", new List<string> { "AB", "BCD", "EA" } },
                { "AbC4597Ddi8Dcf", new List<string> { "AbC", "Dcf" } },
                { "b83dc5)Cvf", new List<string> { "b", "dc", "Cvf" } },
                { " Kfg  fF)((Fgv", new List<string> { "Kfg", "Fgv" } },
                { "dfd95Dcv_dxz3ase8ed", new List<string> { "Dcv", "dxz", "ase", "ed" } }
            };
        }

        private static Dictionary<string, string> LettersInit()
        {
            return new Dictionary<string, string>
            {
                { "aA5656D", "5656D" },
                { "aCDC$DD", "$DD" },
                { "azs%%SSD", "%%SSD" },
                { "AAAAJF^DVF", "^DVF" },
                { "avcf3S324#", "3S324#" },
                {"asc","" }
            };
        }
    }
    public class Tests: IClassFixture<BeforeAll>
    {
        private Solver _solver;
        private string path;
        private readonly BeforeAll _fixture;

        public Tests(BeforeAll fixture)
        {
            _fixture = fixture;
            _solver = new Solver();
            path = "C:\\Users\\Max\\Desktop\\test2.txt";
        }

        private void Write(string s)
        {
            using (StreamWriter file = new StreamWriter(path))
            {
                file.Write(s);
            }
        }

        [Theory]
        [InlineData("   AAAB", "AAAB")]
        [InlineData("95^^ABCd", "ABCd")]
        [InlineData("&&$$**^**Asdj", "Asdj")]
        [Trait("Category_Type", "String_Methods"), Trait("Method_Type", "Secondary_Method")]
        public void SymSkip_SkipsNonAsciiCharacters_Correctly(string test, string expectedString)
        {
            Write(test);
            using (StreamReader file = new StreamReader(path))
            {
                _solver.SymSkip(file);
                Assert.Equal(expectedString, file.ReadLine());
            }
        }

        [Fact]
        [Trait("Category_Type", "String_Methods"), Trait("Method_Type", "Primary_Method")]
        public void Check_Word_Test_2()
        {
            foreach (var test in _fixture.test)
            {
                if (test.Value == true)
                {
                    Assert.True(_solver.Check(test.Key));
                }
                else
                {
                    Assert.False(_solver.Check(test.Key));
                }
            }
        }

        [Fact]
        [Trait("Category_Type", "Solver_Methods"), Trait("Method_Type", "Primary_Method")]
        public void Solve_matchers()
        {
            foreach (var test in _fixture.matchers_test)
            {
                Write(test.Key);
              
                Assert.Equal(test.Value.Count, _solver.Solve(path).Count);
                Assert.Equal(test.Value, _solver.Solve(path));
            }
        }

        [Fact]
        [Trait("Category_Type", "String_Methods"), Trait("Method_Type", "Secondary_Method")]
        public void Letters_Skip_Test()
        {
            foreach (var test in _fixture.letters_test)
            {
                Write(test.Key);
                using (StreamReader file = new StreamReader(path))
                {
                    _solver.LettersSkip(file);
                    string result = file.ReadToEnd() ?? "";
                    Assert.Equal(test.Value, result);
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetWordTestData))]
        [Trait("Category_Type", "String_Methods"), Trait("Method_Type", "Secondary_Method")]
        public void GetWord_ReturnsExpectedWord(string input, string expected)
        {
            Write(input);
            using (StreamReader file = new StreamReader(path))
            {
                string result = _solver.GetWord(file);
                Assert.NotEmpty(result);
                Assert.Equal(expected, result);
            }
        }

        [Theory]
        [InlineData("")]
        [Trait("Category_Type", "String_Methods"), Trait("Method_Type", "Secondary_Method")]
        public void GetWord_ReturnsExpectedWord_Null(string input)
        {
            Write(input);
            using (StreamReader file = new StreamReader(path))
            {
                string result = _solver.GetWord(file);
                Assert.Empty(result);
            }
        }
        public static IEnumerable<object[]> GetWordTestData()
        {
            yield return new object[] { "word1 word2", "word" };
            yield return new object[] { "abc def", "abc" };
            yield return new object[] { "symbol@ symbol#", "symbol" };
            yield return new object[] { "ACgd%487498thj", "ACgd" };
        }
    }
}
