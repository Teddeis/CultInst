using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Учреждения_Культуры
{
    public partial class ПроверкаСостоянияЗапроса : Form
    {
        public ПроверкаСостоянияЗапроса(string username)
        {
            InitializeComponent();
            textBox1.Text = username;

        }
        db db = new db();
        int selectedrows;

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ПроверкаСостоянияЗапроса_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void CreateColumns()
        {

            dataGridView1.Columns.Add("id", "Номер заявки");
            dataGridView1.Columns.Add("Name", "Название учреждения");
            dataGridView1.Columns.Add("Type", "Тип учреждения");
            dataGridView1.Columns.Add("Adress", "Адрес учреждения");
            dataGridView1.Columns.Add("Representative", "ФИО представителя");
            dataGridView1.Columns.Add("Information", "Контактная информация");
            dataGridView1.Columns.Add("Request", "Причина подачи");
            dataGridView1.Columns.Add("Status", "Статус");
            dataGridView1.Columns.Add("RejectionReason", "Причина отказа");
        }

        private void CreateRows(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(2), record.GetString(3), record.GetString(4), record.GetString(5), record.GetString(6), record.GetString(7),record.GetString(8),record.GetString(9));
        }

        // Функция для обновления данных в таблице
        private void RefreshDataGrid(DataGridView dgw)
        {
            // Очистка строк в таблице
            dgw.Rows.Clear();

            // Создание запроса для получения данных из таблицы CultInst для указанного пользователя
            string query = $"select * from CultInst where Accountant = '{textBox1.Text}'";

            // Создание команды для выполнения запроса
            SqlCommand cmd = new SqlCommand(query, db.con);

            // Открытие соединения с базой данных
            db.con.Open();

            // Выполнение запроса и получение результата
            SqlDataReader sqlDataReader = cmd.ExecuteReader();

            // Чтение данных из результата и создание строк в таблице
            while (sqlDataReader.Read())
            {
                CreateRows(dgw, sqlDataReader);
            }

            // Закрытие соединения с базой данных
            sqlDataReader.Close();
            db.con.Close();

            // Установка цвета фона для ячеек столбца "Статус" в зависимости от значения статуса
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


        private void label2_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
        }

        private void информацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Сумма распределения, замечания и другая информация будет отправлена на данные указанные в поле <Контактная информация>", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }
    }
}
