using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Учреждения_Культуры
{
    public partial class Отчет : Form
    {
        PrintDialog printDialog = new PrintDialog();
        PrintDocument printDocument = new PrintDocument();
        db db = new db();
        Random random = new Random();
        public Отчет()
        {
            InitializeComponent();
        }
        
        private void Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            printDocument.PrintPage += PrintDocument_PrintPage;

            printDialog.Document = printDocument;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                this.Hide();
                printDocument.Print();
                P();
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;

            // Определение размеров листа A4
            float pageWidth = e.PageSettings.PrintableArea.Width;
            float pageHeight = e.PageSettings.PrintableArea.Height;

            // Установка шрифта для печати
            Font font = new Font("Sans", 12);

            PrintTabControl(graphics, pageWidth, pageHeight);
        }

        private void PrintTabControl(Graphics graphics, float pageWidth, float pageHeight)
        {
            // Печать содержимого элементов на листе
            foreach (Control control in Controls)
            {
                if (control is TextBox)
                {
                    PrintTextBox(control as TextBox, graphics, pageWidth, pageHeight);
                }
                else if (control is System.Windows.Forms.Label)
                {
                    PrintLabel(control as System.Windows.Forms.Label, graphics, pageWidth, pageHeight);
                }
            }
        }

        private void PrintTextBox(TextBox textBox, Graphics graphics, float pageWidth, float pageHeight)
        {
            // Получение положения и размеров TextBox на форме
            Rectangle textBoxBounds = textBox.Bounds;

            // Масштабирование положения и размеров TextBox для печати на листе A4
            float scaleX = pageWidth / this.Width;
            float scaleY = pageHeight / this.Height;
            float printX = textBoxBounds.X * scaleX;
            float printY = textBoxBounds.Y * scaleY;
            float printWidth = textBoxBounds.Width * scaleX;
            float printHeight = textBoxBounds.Height * scaleY;

            // Печать содержимого TextBox на листе
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Near;
            stringFormat.LineAlignment = StringAlignment.Near;
            graphics.DrawString(textBox.Text, textBox.Font, Brushes.Black, new RectangleF(printX, printY, printWidth, printHeight), stringFormat);
        }

        private void PrintLabel(System.Windows.Forms.Label label, Graphics graphics, float pageWidth, float pageHeight)
        {
            // Получение положения и размеров Label на форме
            Rectangle labelBounds = label.Bounds;

            // Масштабирование положения и размеров Label для печати на листе A4
            float scaleX = pageWidth / this.Width;
            float scaleY = pageHeight / this.Height;
            float printX = labelBounds.X * scaleX;
            float printY = labelBounds.Y * scaleY;
            float printWidth = labelBounds.Width * scaleX;
            float printHeight = labelBounds.Height * scaleY;

            // Увеличение текста на несколько символов
            string text = label.Text;
            int increaseTextBy = 5; // Увеличение текста на 5 символов
            text += new string(' ', increaseTextBy);

            // Печать содержимого Label на листе
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Near;
            stringFormat.LineAlignment = StringAlignment.Near;
            graphics.DrawString(text, label.Font, Brushes.Black, new RectangleF(printX, printY, printWidth, printHeight), stringFormat);
        }

        private void P()
        {
            // Здесь предполагается, что у вас уже есть TextBox с именем textBox1
            string textToSave = textBox3.Text;
            

            // Получаем сегодняшнюю дату и используем её для создания уникального имени файла
            string fileName = "Отчет#" + textBox1.Text + " " + DateTime.Now.ToString("yyyy-MM-dd") + " " + random.Next(100000, 10000000) + ".txt";

            // Путь к файлу, куда будем сохранять
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

            // Сохраняем текст в файл
            File.WriteAllText(filePath, textToSave);

            int Id = Convert.ToInt32(textBox1.Text);
            string Dates = label5.Text;
            string Description_allocation = filePath;

            string query = $"insert into Report(idDistr,Date,Description_allocation) values ('{Id}','{Dates}','{Description_allocation}')";
            SqlCommand command = new SqlCommand(query, db.con);
            command.Parameters.AddWithValue("idDistr", Id);
            command.Parameters.AddWithValue("Date", Dates);
            command.Parameters.AddWithValue("Description_allocation", Description_allocation);

            try
            {
                db.con.Open();
                int rowsAffected = command.ExecuteNonQuery();
                MessageBox.Show("Отчет сохранен.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                db.con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }
    }
}
