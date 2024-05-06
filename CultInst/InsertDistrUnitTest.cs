using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Учреждения_Культуры;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CultInst
{
    [TestClass]
    public class InsertDistrUnitTest
    {
        [TestMethod]
        public void Zapoln()
        {
            string username = "";

            var form = new Распределение(username);

            form.idInst = 3;
            form.idDistr = 9;
            form.SumDistr = 10;

            form.P();
        }

        [TestMethod]
        public void StatusFalse()
        {
            string ids = "3";
            var form = new ПричинаОтказа(ids);

            form.textBox2.Text = ids;
            form.textBox1.Text = "Test";

            form.StatusFalse();
        }
    }
}
