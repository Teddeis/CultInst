using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Учреждения_Культуры
{
    public partial class Запрос : Form
    {
        Form inputForm = new Form();
        System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
        System.Windows.Forms.Button okButton = new System.Windows.Forms.Button();
        System.Windows.Forms.Button backButton = new System.Windows.Forms.Button();
        public Запрос(string username)
        {
            InitializeComponent();
            listBox1.Items[0] = username;
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            texts.Text = textBox1.Text.Length.ToString() + "/500";
            texts1.Text = textBox2.Text.Length.ToString() + "/50";
            texts2.Text = textBox3.Text.Length.ToString() + "/100";
            texts3.Text = textBox4.Text.Length.ToString() + "/100";
            texts5.Text = textBox6.Text.Length.ToString() + "/50";
            texts4.Text = textBox5.Text.Length.ToString() + "/500";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                button1.Enabled = true;
            }
            else
            { 
                button1.Enabled = false; 
            }
        }

        public void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                textBox5.Text = "Требуется поддержка, для оплаты труда рабочим.";
            }
            if (comboBox2.SelectedIndex == 1)
            {
                textBox5.Text = "Требуется поддержка, для организации культурных мероприятий.";
            }
            if (comboBox2.SelectedIndex == 2)
            {
                textBox5.Text = "Требуется поддержка, для ремонтных работ в зоне учреждения.";
            }
            if (comboBox2.SelectedIndex == 3)
            {
                textBox5.Clear();
            }
        }

        private void Запрос_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        //Подключение к базе данных
        public db db = new db();
        public void P()
        {
            // Создание запроса для вставки в таблицу CultInst
            string query = $"insert into CultInst(Accountant, Name,Type,Adress,Representative,Information, Request, Status) values ('{listBox1.Items[0]}','{db.Names}', '{db.Types}','{db.Adress}','{db.Representative}','{db.info}', '{db.Person}', '{db.Status}')";

            // Создание команды для выполнения запроса
            SqlCommand command = new SqlCommand(query, db.con);

            // Добавление параметров в команду
            command.Parameters.AddWithValue("Accountant", listBox1.Items[0]);
            command.Parameters.AddWithValue("Name", db.Names);
            command.Parameters.AddWithValue("Type", db.Types);
            command.Parameters.AddWithValue("Adress", db.Adress);
            command.Parameters.AddWithValue("Representative", db.Representative);
            command.Parameters.AddWithValue("Information", db.info);
            command.Parameters.AddWithValue("Request", db.Person);
            command.Parameters.AddWithValue("Status", db.Status);

            try
            {
                // Открытие соединения с базой данных
                db.con.Open();

                // Выполнение запроса
                int rowsAffected = command.ExecuteNonQuery();

                // Отображение сообщения об успешном отправлении запроса
                MessageBox.Show("Запрос отправлен, спасибо что обратилис к нам!", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Закрытие соединения с базой данных
                db.con.Close();
            }
            catch (Exception ex)
            {
                // Отображение сообщения об ошибке
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            db.Names = textBox1.Text;
            db.Types = textBox2.Text;
            db.Adress = textBox3.Text;
            db.Representative = textBox6.Text;
            db.info = textBox4.Text;
            db.Person = textBox5.Text;
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox1.Text == " " || textBox2.Text == " " || textBox3.Text == " " || textBox4.Text == " " || textBox5.Text == " " || textBox6.Text == " ")
            {
                MessageBox.Show("Заполните пустые значения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                P();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == 0)
            {
                    string query = $"SELECT * FROM Accountant WHERE Accountant = '{listBox1.Items[0].ToString()}'";
                        SqlCommand command = new SqlCommand(query, db.con);
                        db.con.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            MessageBox.Show($"Аккаунт: {reader.GetString(1)}\nПароль: {reader.GetString(3)}\nПочта: {reader.GetString(2)}", "Информация", MessageBoxButtons.OK); // assuming the data is stored as a string in the database
                        }
                        reader.Close();    
                db.con.Close();
            }
            if (listBox1.SelectedIndex == 1)
            {
                // Создаем новое текстовое поле
                textBox.Location = new System.Drawing.Point(10, 10);
                textBox.Size = new System.Drawing.Size(200, 20);
                textBox.TextChanged += TextBox_TextChanged;
                textBox.KeyPress += email_KeyPress;

                // Создаем новое всплывающее окно
                inputForm.Text = "Изменение почты";
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.ShowIcon = false;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.MaximizeBox = false;
                inputForm.MinimizeBox = false;
                inputForm.BackColor = System.Drawing.Color.White;
                inputForm.Size = new System.Drawing.Size(235, 110 );
                inputForm.AutoSize = false;
                inputForm.ControlBox = false;
                inputForm.Controls.Add(textBox);

                // Добавляем кнопку OK
                okButton.Text = "Сохранить";
                okButton.FlatStyle = FlatStyle.Flat;
                okButton.Location = new System.Drawing.Point(10, 40);
                okButton.Click += (s, r) => 
                {   

                    // Отображаем введенный пользователем текст в MessageBox
                    if (!string.IsNullOrEmpty(textBox.Text))
                    {
                        Zamena();
                        inputForm.Close();
                        textBox.Clear();
                        return;
                    }
                };
                inputForm.Controls.Add(okButton);

                backButton.Text = "Назад";
                backButton.FlatStyle = FlatStyle.Flat;
                backButton.Location = new System.Drawing.Point(135, 40);
                backButton.Click += (s, ev) => { inputForm.Close(); };
                inputForm.Controls.Add(backButton);

                // Отображаем всплывающее окно как диалоговое
                inputForm.ShowDialog();

            }
            if (listBox1.SelectedIndex == 2)
            {
                this.Hide();
                Авторизация авторизация = new Авторизация();
                авторизация.ShowDialog();
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            // Регулярное выражение для проверки корректного окончания почтового адреса
            Regex emailRegex = new Regex(@"[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");

            // Проверка на наличие корректного окончания почтового адреса в текстовом поле
            if (emailRegex.IsMatch(textBox.Text))
            {
                // Корректное окончание найдено
                okButton.Enabled = true;
            }
            else
            {
                // Корректное окончание не найдено
                okButton.Enabled = false;
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

        private void статусЗаявкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
          string username = listBox1.Items[0].ToString();
          ПроверкаСостоянияЗапроса п = new ПроверкаСостоянияЗапроса(username);
          п.ShowDialog();
        }

        private void информацияОЗаявкахToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Сумма распределения, замечания и другая информация будет отправлена на данные указанные в поле <Контактная информация>", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void средстваСвязиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Номер телефона: 8(8452) 57-45-21\nEmail: zapros.cult@gmail.com", "Средства связи", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void оПриложенToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Распределение бюджета между учреждениями культуры обычно осуществляется на основе плановых бюджетных ассигнований, которые утверждаются на уровне государственной и муниципальной власти. Решения о распределении средств могут приниматься с учетом приоритетов в развитии культуры, потребностей конкретных учреждений, региональных особенностей и других факторов. Общее распределение бюджета между учреждениями может проводиться как через непосредственное выделение средств, так и через конкурсы, гранты и программы поддержки. Точные механизмы и процедуры распределения бюджета могут различаться в разных странах и регионах.", "О приложении", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }


        public void Zamena()
        {
            {

                string query = $"update Accountant set Email = '{textBox.Text}' where Accountant = '{listBox1.Items[0].ToString()}' ";
                SqlCommand command = new SqlCommand(query, db.con);
                command.Parameters.AddWithValue("Email", textBox.Text);

                try
                {
                    db.con.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show("Сохранено", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    db.con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message);
                }
            }
        }
    }
}
