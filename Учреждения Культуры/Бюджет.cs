using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Учреждения_Культуры
{
    public partial class Бюджет : Form
    {
        db db = new db();
        new string Name;
        string Date = DateTime.Now.ToString("yyyy-MM-dd");
        public int Budget;
        public Бюджет()
        {
            InitializeComponent();
        }

        private void Бюджет_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void CreateColumns()
        {

            dataGridView1.Columns.Add("id", "id");
            dataGridView1.Columns.Add("Name", "Для чего используется");
            dataGridView1.Columns.Add("Date", "Дата добавления");
            dataGridView1.Columns.Add("SumBudget", "Бюджет");
        }

        private void CreateRows(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetDateTime(2), record.GetInt32(3));
        }


        private void RefreshDataGrid(DataGridView dgw)
        {

            dgw.Rows.Clear();

            string query = $"select * from Budget";

            SqlCommand cmd = new SqlCommand(query, db.con);

            db.con.Open();

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            while (sqlDataReader.Read())
            {
                CreateRows(dgw, sqlDataReader);
            }
            sqlDataReader.Close();
            db.con.Close();
        }

        // Функция для добавления новой записи в таблицу Budget
        public void P()
        {
            // Получение значений из полей ввода
            Name = textBox4.Text;
            Budget = Convert.ToInt32(textBox3.Text);

            // Создание запроса для вставки новой записи в таблицу Budget
            string query = $"insert into Budget(Name,Date,SumBudget) values ('{Name}','{Date}','{Budget}')";

            // Создание команды для выполнения запроса
            SqlCommand command = new SqlCommand(query, db.con);

            // Добавление параметров в команду
            command.Parameters.AddWithValue("Name", Name);
            command.Parameters.AddWithValue("Date", Date);
            command.Parameters.AddWithValue("SumBudget", Budget);

            try
            {
                // Открытие соединения с базой данных
                db.con.Open();

                // Выполнение запроса и получение количества затронутых строк
                int rowsAffected = command.ExecuteNonQuery();

                // Отображение сообщения об успешном добавлении записи
                MessageBox.Show("Данные занесены.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Закрытие соединения с базой данных
                db.con.Close();
            }
            catch (Exception ex)
            {
                // Отображение сообщения об ошибке в случае возникновения исключения
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "" || textBox4.Text == "" || textBox3.Text == " " || textBox4.Text == " ")
            {
                MessageBox.Show("Заполните пустые значения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                P();
                RefreshDataGrid(dataGridView1);
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                int id = (int)dataGridView1[0, rowIndex].Value;
                string deleteQuery = $"DELETE FROM Budget WHERE id = {id}";
                SqlCommand deleteCommand = new SqlCommand(deleteQuery, db.con);
                db.con.Open();
                deleteCommand.ExecuteNonQuery();
                db.con.Close();
                RefreshDataGrid(dataGridView1);
            }
            catch (Exception ex)
            {
                // Отображение сообщения об ошибке в случае возникновения исключения
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }
    }
}
