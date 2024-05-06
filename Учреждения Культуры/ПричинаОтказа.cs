using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Учреждения_Культуры
{
    public partial class ПричинаОтказа : Form
    {
        db db = new db();
        private IDbConnection @object;

        public ПричинаОтказа(string ids)
        {
            InitializeComponent();

            textBox2.Text = ids;
        }

        // Обработчик события нажатия кнопки button1
        private void button1_Click(object sender, EventArgs e)
        {
            // Проверка, заполнены ли оба текстовых поля
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                // Отображение сообщения об ошибке, если поля не заполнены
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Вызов метода StatusFalse() для изменения статуса заявки на "Отказано"
                StatusFalse();

                // Скрытие текущей формы
                this.Hide();
            }
        }

        // Функция для изменения статуса заявки на "Отказано"
        public void StatusFalse()
        {
            // Создание запроса для обновления таблицы CultInst и установки статуса заявки на "Отказано"
            string query = $"update CultInst set Status = 'Отказано', RejectionReason = '{textBox1.Text}' where id = '{textBox2.Text}'";

            // Создание команды для выполнения запроса
            SqlCommand command = new SqlCommand(query, db.con);

            try
            {
                // Открытие соединения с базой данных
                db.con.Open();

                // Выполнение запроса и получение количества затронутых строк
                int rowsAffected = command.ExecuteNonQuery();

                // Закрытие соединения с базой данных
                db.con.Close();
            }
            catch (Exception ex)
            {
                // Отображение сообщения об ошибке в случае возникновения исключения
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
