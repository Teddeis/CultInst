using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data;
using Учреждения_Культуры;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CultInst
{
    [TestClass]
    public class DbUnitTest
    {
        [TestMethod]
        public void Constructor_OpensConnection()
        {
            // Arrange and Act
            db db = new db();
            db.con.Open();
            // Assert
            Assert.IsTrue(db.con.State == ConnectionState.Open);
            db.con.Close();
        }
    }
}
