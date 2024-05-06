using System;
using System.Net.Mail;
using System.Net;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Linq;
using System.Text.RegularExpressions;

namespace Учреждения_Культуры
{
    public partial class Восстановление : Form
    {
        public Восстановление()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            string recipientEmail = textBox1.Text;
            string emailSubject = "#" + r.Next(1000, 10000) + " Восстановление пароля";
            string emailBody = GetDataFromDatabase(); // Метод для получения данных из базы данных

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("zapros.cult@ro.ru"); // Адрес отправителя
                    mail.To.Add(recipientEmail); // Адрес получателя
                    mail.Subject = emailSubject;
                    mail.Body = emailBody;

                    using (SmtpClient smtp = new SmtpClient("smtp.rambler.ru")) // Сервер и порт SMTP
                    {
                        smtp.Credentials = new NetworkCredential("zapros.cult@ro.ru", "01-01-01Qwe"); // Ваши учетные данные для отправки почты
                        smtp.EnableSsl = true; // Включение SSL

                        smtp.Send(mail); // Отправка письма
                    }
                }

                MessageBox.Show("Письмо успешно отправлено, сообщение возможно отправлено в спам!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        db db = new db();
        private string GetDataFromDatabase()
        {
            string data = string.Empty;

            // Извлечение нужных данных
            try
            {
                    db.con.Open();
                    // Пример выполнения запроса к базе данных для извлечения данных
                    string query = $"select * from Accountant where Email = '{textBox1.Text}'";
                    using (SqlCommand command = new SqlCommand(query, db.con))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                data = $"Добрый день!\r\n\r\nМы получили запрос на восстановление вашего пароля. \nВаш аккаунт: {reader["Accountant"].ToString()} \nВаш пароль: {reader["Password"].ToString()}\r\n\r\nС уважением,\r\nКоманда поддержки"; // Получение данных из столбца результата запроса
                            }
                        }
                    }
                    db.con.Close();
            }      
            catch (Exception ex)
            {
                // Обработка ошибок подключения к базе данных
                MessageBox.Show("Произошла ошибка при получении данных из базы данных: " + ex.Message);
            }

            return data;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            // Регулярное выражение для проверки корректного окончания почтового адреса
            Regex emailRegex = new Regex(@"[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");

            // Проверка на наличие корректного окончания почтового адреса в текстовом поле
            if (emailRegex.IsMatch(textBox1.Text))
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

        private void email_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешенные символы: "-", "_", ".", "(", ")", цифры, буквы на английском языке, пробел, Backspace
            char[] allowedChars = new char[] { '-', '_', '.', '(', ')', ' ', '@' };
            if (!char.IsLetterOrDigit(e.KeyChar) && !allowedChars.Contains(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; // Запрещаем ввод недопустимого символа
            }
        }
    }
}
