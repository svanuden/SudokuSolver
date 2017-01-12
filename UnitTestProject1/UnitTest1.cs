using System;
using ConsoleApp1.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var grid = new Grid();
            grid.SetValue(0, 0, 3);
            grid.SetValue(5, 0, 8);
            grid.SetValue(0, 1, 7);
            grid.SetValue(2, 1, 8);
            grid.SetValue(3, 1, 3);
            grid.SetValue(4, 1, 2);
            grid.SetValue(8, 1, 5);
            grid.SetValue(3, 2, 9);
            grid.SetValue(7, 2, 1);
            grid.SetValue(0, 3, 9);
            grid.SetValue(5, 3, 4);
            grid.SetValue(7, 3, 2);
            grid.SetValue(4, 4, 1);
            grid.SetValue(1, 5, 7);
            grid.SetValue(3, 5, 8);
            grid.SetValue(8, 5, 9);
            grid.SetValue(2, 6, 5);
            grid.SetValue(5, 6, 3);
            grid.SetValue(0, 7, 8);
            grid.SetValue(4, 7, 4);
            grid.SetValue(5, 7, 7);
            grid.SetValue(6, 7, 5);
            grid.SetValue(8, 7, 3);
            grid.SetValue(3, 8, 5);
            grid.SetValue(8, 8, 6);
            grid.Process();
        }
    }
}
