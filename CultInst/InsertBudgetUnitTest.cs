using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data;
using Учреждения_Культуры;

namespace CultInst
{
    [TestClass]
    public class InsertBudgetUnitTest
    {
        [TestMethod]
        public void P_WithValidInputs_ExecutesQuery()
        {
            var form = new Бюджет();
            // Act
            form.textBox4.Text = "Test Name";
            form.textBox3.Text = "1000";
            form.P();
        }
    }
}
