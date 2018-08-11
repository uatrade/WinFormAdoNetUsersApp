using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
namespace UsersApp
{
    public partial class Form1 : Form
    {
        private SqlConnection conn;
        private SqlDataReader reader;
        SqlDataAdapter da = null;
        DataSet set = null;
        SqlCommandBuilder cmd = null;
        string myrole;
        string cs = "";
        public Form1()
        {
            InitializeComponent();

            conn = new SqlConnection();

            cs= ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            conn.ConnectionString = cs;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboRole.Items.Add("User");
            comboRole.Items.Add("Admin");
            comboRole.Text = comboRole.Items[0].ToString();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            myrole = "";

            myrole = comboRole.SelectedItem.ToString();
        }

        private void buttonGetList_Click(object sender, EventArgs e) //вывод списка
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "Select *from [UsersAdo].[dbo].Users";
                // command.Connection = conn;
                conn =new SqlConnection(cs);
                set = new DataSet();
                da = new SqlDataAdapter(command.CommandText, conn);
                cmd = new SqlCommandBuilder(da);
                dataGridView1.DataSource = null;      

                da.Fill(set, "[UsersAdo].[dbo].Users");
                dataGridView1.DataSource =set.Tables["[UsersAdo].[dbo].Users"];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if(conn!=null)
                conn.Close();
                if (reader != null)
                reader.Close();
            }

        }

        public bool ChooseLogin(string login)     //проверка логина на повторение
        {
            List<string> mylogin = new List<string>();
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandText = "Select logins from [UsersAdo].[dbo].Users";
                command.Connection = conn;
                conn.Open();
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    mylogin.Add(reader["logins"].ToString());
                }

                foreach(var items in mylogin)
                {
                    if(items.ToString()==login)
                    {
                        return true;
                    } 
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return true;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
                if (reader != null)
                    reader.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)   //регистрация
        {
            if (ChooseLogin(textlogin.Text) == true)
            {
                MessageBox.Show("Login уже существует");
            }
            else
            {
                try
                {
                    conn.Open();
                    SqlCommand commandAdd = new SqlCommand();
                    commandAdd.Connection = conn;

                    commandAdd.CommandText = $"Insert into [UsersAdo].[dbo].Users values('{textlogin.Text}', '{textPassw.Text.GetHashCode()}', '{textEmail.Text}', '{textPhone.Text}', '{myrole}')";
                    commandAdd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }

                finally
                {
                    if (conn != null)
                        conn.Close();
                }

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            da.Update(set, "[UsersAdo].[dbo].Users");
        }
    }
}