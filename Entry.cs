﻿using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace admission_commision
{
    public partial class Entry : Form
    {
        public Entry()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var login = textBox1.Text;
            var pass = textBox2.Text;

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable dt = new DataTable();

            string hashedPassword = ComputeSha256Hash(pass);
            string query = $"SELECT id, login, password, is_admin FROM users WHERE login = '{login}' and password = '{hashedPassword}'";

            MySqlCommand cmd = new MySqlCommand(query, SQLClass.conn);

            adapter.SelectCommand = cmd;
            adapter.Fill(dt);

            if (dt.Rows.Count == 1)
            {
                var user = new CheckAdmin(dt.Rows[0].ItemArray[1].ToString(), Convert.ToBoolean(dt.Rows[0].ItemArray[3]));

                textBox1.Clear();
                textBox2.Clear();

                AdmissionCom rf = (AdmissionCom)Application.OpenForms["admission_com"];
                if (rf == null)
                {
                    AdmissionCom form = new AdmissionCom(user);
                    this.Hide();
                    form.Show();
                    this.Activate();
                }
                else
                {
                    rf.Activate();
                    this.Hide();
                }
            }
            else
                MessageBox.Show("Неверное имя пользователя или пароль!", "Access denied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Registr rf = new Registr();
            this.Hide();
            rf.ShowDialog();
            this.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
            textBox1.MaxLength = 45;
            textBox2.MaxLength = 45;
        }
    }
}