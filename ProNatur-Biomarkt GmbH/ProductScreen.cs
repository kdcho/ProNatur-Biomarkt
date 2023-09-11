using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProNatur_Biomarkt_GmbH
{
    public partial class ProductScreen : Form
    {
        private SqlConnection databaseConnection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\qn4h\\Documents\\ProNatur-Biomarkt.mdf;Integrated Security=True;Connect Timeout=30");
        private int lasSelectedProductKey;
        public ProductScreen()
        {
            InitializeComponent();
            ShowProducts();
        }

        private void ShowProducts()
        {
            databaseConnection.Open();

            string query = "SELECT * from Products";
            SqlDataAdapter adapter = new SqlDataAdapter(query, databaseConnection);

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);

            dataGridView.DataSource = dataSet.Tables[0];
            dataGridView.Columns[0].Visible = false;

            databaseConnection.Close();
        }

        private void ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlCommand sqlCommand = new SqlCommand(query, databaseConnection))
            {
                databaseConnection.Open();

                if (parameters != null)
                {
                    sqlCommand.Parameters.AddRange(parameters);
                }

                sqlCommand.ExecuteNonQuery();
                databaseConnection.Close();
            } // Automatically closes and disposes of the command
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxProductName.Text) ||
                string.IsNullOrEmpty(textBoxBrandName.Text) ||
                string.IsNullOrEmpty(textBoxPrice.Text) ||
                string.IsNullOrEmpty(comboBoxCategory.Text))
            {
                MessageBox.Show("Bitte fülle alle Werte aus.");
                return;
            }

            string productName = textBoxProductName.Text;
            string productBrand = textBoxBrandName.Text;
            string productCategory = comboBoxCategory.Text;

            if (!decimal.TryParse(textBoxPrice.Text, out decimal productPrice))
            {
                MessageBox.Show("Ungültiger Preis. Bitte geben Sie eine gültige Dezimalzahl ein.");
                return;
            }

            string query = "INSERT INTO Products (Name, Brand, Category, Price) " +
                           "VALUES (@Name, @Brand, @Category, @Price)";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Name", productName),
                new SqlParameter("@Brand", productBrand),
                new SqlParameter("@Category", productCategory),
                new SqlParameter("@Price", productPrice)
            };

            ExecuteQuery(query, parameters);
            EmptyAllFields();
            ShowProducts();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lasSelectedProductKey == 0)
            {
                MessageBox.Show("Bitte wähle ein Produkt aus.");
                return;
            }
            
            if (string.IsNullOrEmpty(textBoxProductName.Text) ||
                string.IsNullOrEmpty(textBoxBrandName.Text) ||
                string.IsNullOrEmpty(textBoxPrice.Text) ||
                string.IsNullOrEmpty(comboBoxCategory.Text))
            {
                MessageBox.Show("Bitte fülle alle Werte aus.");
                return;
            }

            string productName = textBoxProductName.Text;
            string productBrand = textBoxBrandName.Text;
            string productCategory = comboBoxCategory.Text;

            if (!decimal.TryParse(textBoxPrice.Text, out decimal productPrice))
            {
                MessageBox.Show("Ungültiger Preis. Bitte geben Sie eine gültige Dezimalzahl ein.");
                return;
            }

            string query = "UPDATE Products " +
                   "SET Name = @Name, Brand = @Brand, Category = @Category, Price = @Price " +
                   "WHERE Id = @Id";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", lasSelectedProductKey),
                new SqlParameter("@Name", productName),
                new SqlParameter("@Brand", productBrand),
                new SqlParameter("@Category", productCategory),
                new SqlParameter("@Price", productPrice)
            };

            ExecuteQuery(query, parameters);
            EmptyAllFields();
            ShowProducts();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(lasSelectedProductKey == 0)
            {
                MessageBox.Show("Bitte wähle ein Produkt aus.");
                return;
            }

            string query = "DELETE FROM Products WHERE Id = @Id";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", lasSelectedProductKey)
            };

            ExecuteQuery(query, parameters);
            EmptyAllFields();
            ShowProducts();
        }

        private void btnEmptyInputs_Click(object sender, EventArgs e)
        {
            EmptyAllFields();
        }

        private void EmptyAllFields()
        {
            textBoxProductName.Text = string.Empty;
            textBoxBrandName.Text = string.Empty;
            comboBoxCategory.SelectedItem = null;
            textBoxPrice.Text = string.Empty;
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxProductName.Text = dataGridView.SelectedRows[0].Cells[1].Value.ToString();
            textBoxBrandName.Text = dataGridView.SelectedRows[0].Cells[2].Value.ToString();
            comboBoxCategory.Text = dataGridView.SelectedRows[0].Cells[3].Value.ToString();
            textBoxPrice.Text = dataGridView.SelectedRows[0].Cells[4].Value.ToString();

            lasSelectedProductKey = (int)dataGridView.SelectedRows[0].Cells[0].Value;
        }
    }
}
