using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using nUnit1;

namespace TestProject2
{
    public class Tests2
    {
        private Solver _solver;
        private string path;

        public Tests2()
        {
            _solver = new Solver();
            path = "C:\\Users\\Max\\Desktop\\test.txt";
            
        }
        [Fact]
        [Trait("Category_Type", "Reading_File_Methods"), Trait("Method_Type", "Secondary_Method")]
        public void NonExistingFile()
        {
            string testpath = "1.txt";
            Assert.Throws<FileNotFoundException>(() => _solver.Solve(testpath));
        }
        [Theory]
        [InlineData(1000)]
        [InlineData(5000)]
        [InlineData(10000)]
        [Trait("Category_Type", "Writing_File_Methods")]
        public void Generate_CreatesFileWithExpectedSize(int size)
        {
            _solver.Generate(size, path);
            FileInfo fileInfo = new FileInfo(path);
            Assert.True(fileInfo.Exists, "File does not exist.");
            Assert.Equal(size, fileInfo.Length);
        }
        [Theory]
        [InlineData("abcde", true)]
        [InlineData("abcda", false)]
        [InlineData("aBcdeFghijklmnopQrstuvwXYZ", true)]
        [InlineData("ABCA", false)]
        [Trait("Category_Type", "String_Methods"), Trait("Method_Type", "Primary_Method")]
        public void Check_Word_Test(string word, bool expectedResult)
        {
            Assert.Equal(_solver.Check(word), expectedResult);
        }
        [Theory]
        [InlineData('A', true)]
        [InlineData('z', true)]
        [InlineData('0', false)]
        [InlineData('9', false)]
        [InlineData('@', false)]
        [InlineData('~', false)]
        [Trait("Category_Type", "Char_Methods"), Trait("Method_Type", "Secondary_Method")]
        public void IsNumAscii_InputChar_ReturnsExpectedResult(char input, bool expectedResult)
        {
            Assert.Equal(_solver.IsNumAscii(input), expectedResult);
        }
    }
}
