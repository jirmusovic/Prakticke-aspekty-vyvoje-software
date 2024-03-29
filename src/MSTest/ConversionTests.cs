/**
 * @file ConversionTests.cs
 * @class ConversionTest
 * 
 * @brief Sada testů pro převod mezi číselnými soustavami
 * 
 * @date 28-04-2022
 * @author xmothe00
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calc;
using System;

namespace MSTest
{
    [TestClass]
    public class ConversionTests
    {
        /**
         * @test Testování převodů z desítkové do osmičkové soustavy
         */
        [TestMethod]
        public void From10base()
        {
            var convert = new Conversion();
            var result = convert.Convert_from_to("256", "10", "8");
            Assert.IsTrue(result == "400");
        }
        /**
         * @test Testování převodů z desítkové do osmičkové soustavy se špatně zadanou vstupní hodnotou
         */
        [TestMethod]
        public void From10baseBad()
        {
            var convert = new Conversion();
            var result = convert.Convert_from_to("M", "10", "8");
            Assert.IsTrue(result == null);
        }
        /**
         * @test Testování převodů z dvojkové do desítkové soustavy
         */
        [TestMethod]
        public void From2base()
        {
            var convert = new Conversion();
            var result = convert.Convert_from_to("1111100000111110000011111", "2", "10");
            Assert.IsTrue(result == "32537631");
        }
        /**
         * @test Testování převodů z dvojkové do desítkové soustavy se špatně zadanou vstupní hodnotou
         */
        [TestMethod]
        public void From2baseBad()
        {
            var convert = new Conversion();
            var result = convert.Convert_from_to("1111100000111210000011111", "2", "10");
            Assert.IsTrue(result == null);
        }
        /**
         * @test Testování převodů z šestnáctkové do dvojkové soustavy
         */
        [TestMethod]
        public void From16base()
        {
            var convert = new Conversion();
            var result = convert.Convert_from_to("ff89e", "16", "2");
            Assert.IsTrue(result == "11111111100010011110");
        }
        /**
         * @test Testování převodů z šestnáctkové do dvojkové soustavy se špatně zadanou vstupní hodnotou
         */
        [TestMethod]
        public void From16baseBad()
        {
            var convert = new Conversion();
            var result = convert.Convert_from_to("ZZZ", "16", "2");
            Assert.IsTrue(result == null);
        }
        /**
         * @test Testování převodů z šestnáctkové do dvojkové soustavy
         */
        [TestMethod]
        public void Fromrandom1()
        {
            var convert = new Conversion();
            var result = convert.Convert_from_to("g18", "24", "9");
            Assert.IsTrue(result == "13615");
        }
        /**
         * @test Testování převodů z třícetipětkové do třicetišestkové soustavy se špatně zadanou vstupní hodnotou
         */
        [TestMethod]
        public void Fromrandom2bad()
        {
            var convert = new Conversion();
            var result = convert.Convert_from_to("Z1313", "35", "36");
            Assert.IsTrue(result == null);
        }
        /**
         * @test Testování převodů z desítkové báze do desítkové baze se špatně zadanou vstupní hodnotou
         */
        [TestMethod]
        public void Fromrandom3bad()
        {
            var convert = new Conversion();
            var result = convert.Convert_from_to("10", "10", "654");
            Assert.IsTrue(result == null);
        }

    }
}
