using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace MySQL_CS
{
	public partial class Form1 : Form
	{
		private SqlConnection sqlConnection = null;
		private SqlConnection nrthwndConnection = null;



		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString);

			sqlConnection.Open();

			nrthwndConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["NorthwinDB"].ConnectionString);

			nrthwndConnection.Open();

			SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT *FROM Products", nrthwndConnection);
			DataSet db = new DataSet();
			dataAdapter.Fill(db);
			dataGridView2.DataSource = db.Tables[0];

		}

		private void tabPage1_Click(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			SqlCommand command = new SqlCommand(
				$"INSERT INTO [Students](Name, Surname, Misto_rozd, Email, Phone) VALUES ('{textBox1.Text}','{textBox2.Text}','{textBox3.Text}','{textBox4.Text}','{textBox5.Text}')", sqlConnection) ;
			MessageBox.Show(command.ExecuteNonQuery().ToString());
		}

		private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
		{

		}

		private void button2_Click(object sender, EventArgs e)
		{
			SqlDataAdapter dataAdaptar = new SqlDataAdapter(
				textBox6.Text,
				nrthwndConnection);

			DataSet dataSet = new DataSet();
			dataAdaptar.Fill(dataSet);
			dataGridView1.DataSource = dataSet.Tables[0];
		}

		private void button3_Click(object sender, EventArgs e)
		{
			listView1.Items.Clear();

			SqlDataReader dataReader = null;

			try
			{
				SqlCommand sqlCommand = new SqlCommand("SELECT ProductName, QuantityPerUnit, UnitPrice from Products",
					nrthwndConnection);

				dataReader = sqlCommand.ExecuteReader();

				ListViewItem Item = null;

				while (dataReader.Read())
				{
					Item = new ListViewItem(new String[] { Convert.ToString(dataReader["ProductName"]),
						Convert.ToString(dataReader["QuantityPerUnit"]),
						Convert.ToString(dataReader["UnitPrice"]) });
					listView1.Items.Add(Item);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				if (dataReader != null && !dataReader.IsClosed)
				{
					dataReader.Close();
				}
			}
		}

		private void textBox7_TextChanged(object sender, EventArgs e)
		{
			(dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"ProductName LIKE '%{textBox7.Text}%'";
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (comboBox1.SelectedIndex)
			{
				case 0:
					(dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock<10";
					break;

					case 1:

					(dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock >= 10 AND UnitsInStock<=50";

					break;
				case 2:

					(dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock >=50";

					break;

				case 3:

					(dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "";

					break;

			}
		}
	}
}
