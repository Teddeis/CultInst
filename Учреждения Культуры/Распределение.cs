using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace Учреждения_Культуры
{
    public partial class Распределение : Form
    {
        db db = new db();
        public int idInst;
        public int idDistr;
        public int SumDistr;
        int selectedrows;
        
        public Распределение(string username)
        {
            InitializeComponent();

            textBox4.Text = username;
            
        }
        private void Распределение_Load(object sender, EventArgs e)
        {
            CreateColumns();
            CreateColumns2();

            RefreshDataGrid(dataGridView1);
            RefreshDataGrid2(dataGridView3);
        }

        private void CreateColumns()
        {

            dataGridView1.Columns.Add("id", "id");
            dataGridView1.Columns.Add("Name", "Название учреждения");
            dataGridView1.Columns.Add("Type", "Тип учреждения");
            dataGridView1.Columns.Add("Adress", "Адрес учреждения");
            dataGridView1.Columns.Add("Representative", "ФИО представителя");
            dataGridView1.Columns.Add("Information", "Контактная информация");
            dataGridView1.Columns.Add("Request", "Причина подачи");
            dataGridView1.Columns.Add("Status", "Статус поданной заявки");
            dataGridView1.Columns.Add("RejectionReason", "Причина отказа");
        }

        private void CreateColumns2()
        {

            dataGridView3.Columns.Add("id", "id");
            dataGridView3.Columns.Add("Name", "Для чего используется");
            dataGridView3.Columns.Add("Date", "Дата");
            dataGridView3.Columns.Add("SumBudget", "Доступный бюджета");
        }

        private void CreateRows(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(2), record.GetString(3), record.GetString(4), record.GetString(5), record.GetString(6), record.GetString(7),record.GetString(8),record.GetString(9));
        }

        private void CreateRows2(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetDateTime(2), record.GetInt32(3));
        }

        public void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string query = $"select * from CultInst";

            SqlCommand cmd = new SqlCommand(query, db.con);

            db.con.Open();


            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            while (sqlDataReader.Read())
            {
                CreateRows(dgw, sqlDataReader);
            }
            sqlDataReader.Close();
            db.con.Close();
            for (int j = 0; j < dgw.RowCount; j++)
            {
                switch (dgw[7, j].Value.ToString())
                {

                    case "Одобрено":


                        dgw[7, j].Style.BackColor = Color.YellowGreen;


                        break;
                    case "Не рассмотрено":

                        dgw[7, j].Style.BackColor = Color.Yellow;

                        break;
                    case "Отказано":

                        dgw[7, j].Style.BackColor = Color.Red;

                        break;
                }
            }
        }

        private void RefreshDataGrid2(DataGridView dgws)
        {
            dgws.Rows.Clear();

            string query = $"select * from Budget";

            SqlCommand cmd = new SqlCommand(query, db.con);

            db.con.Open();

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            while (sqlDataReader.Read())
            {
                CreateRows2(dgws, sqlDataReader);
            }
            sqlDataReader.Close();
            db.con.Close();
        }

        //Метод для заполнения базы данных для распределения бюджета.
        public void P()
        {
            {
                string query = $"insert into Distr(idInst,idBudget,SumDistr) values ('{idInst}','{idDistr}','{SumDistr}')";
                SqlCommand command = new SqlCommand(query, db.con);
                command.Parameters.AddWithValue("idInst", idInst);
                command.Parameters.AddWithValue("idBudget", idDistr);
                command.Parameters.AddWithValue("SumDistr", SumDistr);

                try
                {
                    db.con.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show("Данные занесены.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    db.con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message);
                }
            }
        }

        //Нажатие на DataGrid и вывод имени отправителя 
        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            selectedrows = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedrows];

                if (row.Cells[0].Value == null)
                {
                    MessageBox.Show("Выбрано пустое значение!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("> Название учреждения: \n" + row.Cells[1].Value.ToString() + "\n\n> Причина подачи заявки: \n" + row.Cells[6].Value.ToString(),"Причина подачи");
                    textBox2.Text = row.Cells[0].Value.ToString();
                }
                
            }
        }

        private void dataGridView2_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            selectedrows = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView3.Rows[selectedrows];

                if(row.Cells[0].Value == null)
                {
                    MessageBox.Show("Выбрано пустое значение!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    textBox3.Text = row.Cells[0].Value.ToString();
                }
            }
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (!Char.IsDigit(ch) && ch != 8) //Если символ, введенный с клавы - не цифра (IsDigit),
            {
                e.Handled = true;// то событие не обрабатывается. ch!=8 (8 - это Backspace)
            }
        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Авторизация авторизация = new Авторизация();
            авторизация.ShowDialog();
        }

        private void Распределение_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox3.Text == "" || textBox1.Text == "")
            {
                MessageBox.Show("Заполните пустые значения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                idInst = Convert.ToInt32(textBox2.Text);
                idDistr = Convert.ToInt32(textBox3.Text);
                SumDistr = Convert.ToInt32(textBox1.Text);

                СравнитьЗначения(dataGridView3);
                RefreshDataGrid(dataGridView1);
                RefreshDataGrid2(dataGridView3);
            }
        }

        int максЗначение = 0;
        // Функция для сравнения введенного значения с доступным значением и выполнения соответствующих действий
        private void СравнитьЗначения(DataGridView dataGridView)
        {
            // Сброс максимального значения
            максЗначение = 0;

            // Получение максимального значения из выбранной строки указанного столбца
            int n = int.Parse(dataGridView3.CurrentRow.Cells[3].Value.ToString());
            int значение = Convert.ToInt32(n);
            if (значение > максЗначение)
            {
                максЗначение = значение;
            }

            // Получение введенного значения из текстового поля
            int введенноеЗначение = Convert.ToInt32(textBox1.Text);

            // Сравнение введенного значения с максимальным значением
            if (максЗначение >= введенноеЗначение)
            {
                // Вызов методов для обновления распределенного бюджета, снижения бюджета учреждения и изменения статуса заявки
                P();
                Ps();
                StatusTrue();
            }
            else
            {
                // Отображение сообщения об ошибке, если введенное значение больше доступного
                MessageBox.Show("Введенное число больше, чем доступно!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Функция для обновления распределенного бюджета
        private void Ps()
        {
            // Создание запроса для обновления столбца SumBudget в таблице Budget
            string query = $"update Budget set SumBudget = SumBudget - '{textBox1.Text}' where id = '{textBox3.Text}'";

            // Создание команды для выполнения запроса
            SqlCommand command = new SqlCommand(query, db.con);

            // Попытка открыть соединение с базой данных, выполнить запрос и закрыть соединение
            try
            {
                db.con.Open();
                int rowsAffected = command.ExecuteNonQuery();
                db.con.Close();
            }
            catch (Exception ex)
            {
                // Отображение сообщения об ошибке в случае возникновения исключения
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }


        private void StatusTrue()
        {
                string query = $"update CultInst set Status = 'Одобрено', RejectionReason = 'Пусто' where id = '{textBox2.Text}'";
                SqlCommand command = new SqlCommand(query, db.con);

                try
                {
                    db.con.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    db.con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message);
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Бюджет бюджет = new Бюджет();

            бюджет.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RefreshDataGrid2(dataGridView3);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Список_распределения список_Распределения = new Список_распределения();
            список_Распределения.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string ids = textBox2.Text;
            if (textBox2.Text == "")
            {
                MessageBox.Show("Заполните пустые значения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                ПричинаОтказа о = new ПричинаОтказа(ids);
                о.ShowDialog();
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
