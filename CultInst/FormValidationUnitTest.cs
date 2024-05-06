using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Учреждения_Культуры;

namespace CultInst
{
    [TestClass]
    public class FormValidationUnitTest
    {
        string username = "";
        [TestMethod]
        public void Validate_WithEmptyFields_ReturnsFalse()
        {
            // Arrange
            var form = new Запрос(username);

            // Act
            bool isValid = form.Validate();

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void Validate_WithValidFields_ReturnsTrue()
        {
            // Arrange
            var form = new Запрос(username);
            form.textBox1.Text = "Test Name";
            form.textBox2.Text = "Test Address";
            form.textBox3.Text = "Test Phone Number";
            form.comboBox2.SelectedIndex = 1;

            // Act
            bool isValid = form.Validate();

            // Assert
            Assert.IsTrue(isValid);
        }
    }

}
