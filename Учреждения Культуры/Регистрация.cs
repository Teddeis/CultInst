using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Учреждения_Культуры
{
    public partial class Регистрация : Form
    { 
        //Подключение к базе данных
        db db = new db();
        public Регистрация()
        {
            InitializeComponent();
        }

        private void Регистрация_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Авторизация авторизация = new Авторизация();
            авторизация.Show();
        }



        // Функция регистрации нового пользователя
        public void registration()
        {
            // Получение значений из полей ввода
            string username = textBox1.Text;

            // Создание адаптера данных и таблицы данных
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable Table = new DataTable();

            // Создание запроса для проверки наличия существующей учетной записи с таким же именем пользователя или электронной почтой
            string query = $"select Accountant,Email,Password from Accountant where Accountant='{db.login}' or Email = '{db.email}'";

            // Создание команды для выполнения запроса
            SqlCommand cmd = new SqlCommand(query, db.con);

            // Назначение команды адаптеру данных
            adapter.SelectCommand = cmd;

            // Заполнение таблицы данными из базы данных
            adapter.Fill(Table);

            // Открытие соединения с базой данных
            db.con.Open();

            // Проверка, существует ли учетная запись с указанным именем пользователя или электронной почтой
            if (Table.Rows.Count == 0)
            {
                // Создание команды для вставки новой учетной записи в базу данных
                SqlCommand insertCommand = new SqlCommand($"insert into Accountant(Accountant,Email,Password) values ('{db.login}','{db.email}','{db.pass}')", db.con);

                // Выполнение запроса на вставку и проверка количества затронутых строк
                if (insertCommand.ExecuteNonQuery() == 1)
                {
                    // Отображение сообщения об успешной регистрации
                    MessageBox.Show("Регистрация прошла успешно!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Скрытие текущей формы и открытие формы "Запрос"
                    this.Hide();
                    Запрос запрос = new Запрос(username);
                    запрос.ShowDialog();
                }
            }
            else
            {
                // Отображение сообщения об ошибке, если учетная запись с таким же именем пользователя или электронной почтой уже существует
                MessageBox.Show("Такой логин/почта уже существует", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            // Закрытие соединения с базой данных
            db.con.Close();
        }

        //Значения для заполнения

        private void button1_Click_1(object sender, EventArgs e)
        {
            db.login = textBox1.Text;
            db.email = textBox7.Text;
            db.pass = textBox2.Text;

            if (db.login == "" || db.email == "" || db.pass == "" || db.login == " " || db.email == " " || db.pass == " ")
            {
                MessageBox.Show("Пожалуйста, введите логин/почту/пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            registration();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = textBox1.PasswordChar;
            }
            else
            {
                textBox2.PasswordChar = '*';
            }
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешенные символы: "-", "_", ".", "(", ")", цифры, буквы на английском языке, пробел, Backspace
            char[] allowedChars = new char[] { '-', '_', '.', '(', ')', ' ' };
            if (!char.IsLetterOrDigit(e.KeyChar) && !allowedChars.Contains(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Запрещаем ввод недопустимого символа
            }
        }

        private void email_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешенные символы: "-", "_", ".", "(", ")", цифры, буквы на английском языке, пробел, Backspace
            char[] allowedChars = new char[] { '-', '_', '.', '(', ')', ' ', '@' };
            if (!char.IsLetterOrDigit(e.KeyChar) && !allowedChars.Contains(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Запрещаем ввод недопустимого символа
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            // Регулярное выражение для проверки корректного окончания почтового адреса
            Regex emailRegex = new Regex(@"[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");

            // Проверка на наличие корректного окончания почтового адреса в текстовом поле
            if (emailRegex.IsMatch(textBox7.Text))
            {
                // Корректное окончание найдено
                button1.Enabled = true;
            }
            else
            {
                // Корректное окончание не найдено
                button1.Enabled = false;
            }
        }
    }
}
