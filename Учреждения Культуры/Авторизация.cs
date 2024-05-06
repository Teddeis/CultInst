using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Учреждения_Культуры
{
    public partial class Авторизация : Form
    {
        db db = new db();
        public Авторизация()
        {
            InitializeComponent();
        }
        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked ) 
            {
                textBox2.PasswordChar = textBox1.PasswordChar;
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Регистрация регистрация = new Регистрация();
            регистрация.Show();
        }

        private void Авторизация_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


        // Подключение к базе данных и проверка учетных данных
        private void logins()
        {
            // Получение имени пользователя из текстового поля
            string username = textBox1.Text;

            // Создание адаптера данных и таблицы данных
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable Table = new DataTable();

            // Создание запроса для проверки учетных данных
            string query = $"select * from Accountant where Accountant='{db.login}' and Password='{db.pass}'";

            // Создание команды для выполнения запроса
            SqlCommand command = new SqlCommand(query, db.con);

            // Назначение команды адаптеру данных
            adapter.SelectCommand = command;

            // Заполнение таблицы данными из базы данных
            adapter.Fill(Table);

            // Проверка, существует ли учетная запись с указанными учетными данными
            if (Table.Rows.Count == 1)
            {
                if (textBox1.Text == "d") // Если имя пользователя "d", то открыть форму "Распределение"
                {
                    MessageBox.Show("Вы успешно вошли!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    Распределение распределение = new Распределение(username);
                    распределение.Show();
                }
                else // Иначе открыть форму "Запрос"
                {
                    MessageBox.Show("Вы успешно вошли!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    Запрос запрос = new Запрос(username);
                    запрос.ShowDialog();
                }

                // Открытие и закрытие соединения с базой данных
                db.con.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
                db.con.Close();
            }
            else
            {
                // Отображение сообщения об ошибке
                MessageBox.Show("Неверный логин/пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }



        //Значения для заполнения
        private void button1_Click_1(object sender, EventArgs e)
        {
            db.login = textBox1.Text;
            db.pass = textBox2.Text;

            if (db.login == "" || db.pass == "")
            {
                MessageBox.Show("Пожалуйста, введите логин/пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            logins();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Восстановление в = new Восстановление();
            в.ShowDialog();
        }
    }
}
