using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data;
using Учреждения_Культуры;

namespace CultInst
{
    [TestClass]
    public class InsertCultInstUnitTest
    {
        [TestMethod]
        public void P_WithValidInputs_ExecutesQuery()
        {
            string username = "";
            // Arrange
            var form = new Запрос(username); // Создаем экземпляр тестируемой формы

            // Act
            form.listBox1.Items.Add("test");
            form.db.Names = "Test Name";
            form.db.Types = "Test Type";
            form.db.Adress = "Test Address";
            form.db.Representative = "Test Representative";
            form.db.info = "Test Information";
            form.db.Person = "Test Request";
            form.db.Status = "Test Status";

            // Assert
            form.P();
        }
    }
}
