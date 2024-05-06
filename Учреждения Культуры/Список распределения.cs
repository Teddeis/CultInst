using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Учреждения_Культуры
{
    public partial class Список_распределения : Form
    {
        public DataGridViewRow row;
        int selectedrows;
        db db = new db();
        public Список_распределения()
        {
            InitializeComponent();
        }

        private void Список_распределения_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("d.id", "id");
            dataGridView1.Columns.Add("c.Name", "Название учреждения");
            dataGridView1.Columns.Add("d.idBudget", "Номер бюджета");
            dataGridView1.Columns.Add("b.SumBudget", "Сумма бюджета");
            dataGridView1.Columns.Add("d.SumDistr", "Распределенная сумма");
            dataGridView1.Columns.Add("c.Request", "Причина подачи");
        }

        private void CreateRows(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0),record.GetString(1), record.GetInt32(2), record.GetInt32(3), record.GetInt32(4), record.GetString(5));
        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string query = $"EXEC [Вывод]";

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

        public void Bolsh(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string query = $"EXEC [Наибольший бюджет]";

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

  

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
        }

        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedrows = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedrows];

                if (row.Cells[0].Value == null)
                {
                    button2.Visible = false;
                    MessageBox.Show("Выбрано пустое значение!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    button2.Visible = true;
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            string Date = DateTime.Now.ToString("yyyy-MM-dd");

            row = dataGridView1.Rows[selectedrows];
            Отчет о = new Отчет();
            о.textBox1.Text = row.Cells[0].Value.ToString();
            о.label5.Text = Date;
            о.textBox3.Text = $"Доступный бюджет распределен для учреждения:\n\n\r{row.Cells[1].Value.ToString()}\n\n\rДля использования в целях:\n\n\r{row.Cells[4].Value.ToString()}\n\n\rОтчет предоставляется в двух официальных формах: Печатная, Электронная\n\n\rВсе права, на использование распределенного бюджета передаются учреждению.\n\n\rВсе нарушения, возникшие после распределения(не касающиеся момента, до получения денег) берет на себя учреждение, на которое было произведено распределение.\n\n\rСумма распределения: {row.Cells[3].Value.ToString()}₽";
            о.ShowDialog();
        }

        // Функция для получения среднего значения распределенного бюджета
        public void AVG()
        {
            try
            {
                // Создание запроса для выполнения хранимой процедуры [Среднее значение]
                string query = "EXEC [Среднее значение]";

                // Создание команды для выполнения запроса
                SqlCommand command = new SqlCommand(query, db.con);

                // Открытие соединения с базой данных
                db.con.Open();

                // Выполнение запроса и получение результата
                SqlDataReader reader = command.ExecuteReader();

                // Проверка наличия данных в результате
                if (reader.Read())
                {
                    // Получение среднего значения из результата
                    int avg = reader.GetInt32(0);

                    // Отображение среднего значения в метке label3
                    label3.Text = "Среднее значение: " + avg;
                }

                // Закрытие соединения с базой данных
                db.con.Close();
            }
            catch (Exception ex)
            {
                // Отображение сообщения об ошибке в случае возникновения исключения
                MessageBox.Show("Произошла ошибка: " + ex.Message);
                db.con.Close();
            }

        }

        // Функция для получения суммарного распределенного бюджета
        public void SUM()
        {
            try
            {
                // Создание запроса для выполнения хранимой процедуры [Суммарный распределенный бюджет]
                string query = "EXEC [Суммарный распределенный бюджет]";

                // Создание команды для выполнения запроса
                SqlCommand command = new SqlCommand(query, db.con);

                // Открытие соединения с базой данных
                db.con.Open();

                // Выполнение запроса и получение результата
                SqlDataReader reader = command.ExecuteReader();

                // Проверка наличия данных в результате
                if (reader.Read())
                {
                    // Получение суммарного распределенного бюджета из результата
                    int sum = reader.GetInt32(0);

                    // Отображение суммарного распределенного бюджета в метке label3
                    label3.Text = "Суммарный бюджет: " + sum;
                }

                // Закрытие соединения с базой данных
                db.con.Close();
            }
            catch (Exception ex)
            {
                // Отображение сообщения об ошибке в случае возникновения исключения
                MessageBox.Show("Произошла ошибка: " + ex.Message);
                db.con.Close();
            }
        }


        private void Button6_Click(object sender, EventArgs e)
        {
            Bolsh(dataGridView1);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            AVG();
            label3.Visible = true;
            panel3.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SUM();
            label3.Visible = true;
            panel3.Visible = true;
        }
    }
}
