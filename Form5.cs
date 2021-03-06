﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace 农产品物流管理系统
{
    public partial class Form5 : Form
    {
        string Pno;
        string user;
        string Cno;
        MySqlConnection conn;
        public Form5(MySqlConnection conn,string user)
        {
            
            this.conn = conn;
            this.user = user;
            InitializeComponent();
            label6.Text = "";
            label7.Text = "";
            panel1.BackColor = Color.FromArgb(125, 255, 255, 255);
            label2.BackColor = Color.FromArgb(0, 255, 255, 255);
            label3.BackColor = Color.FromArgb(0, 255, 255, 255);
            label4.BackColor = Color.FromArgb(0, 255, 255, 255);
            label5.BackColor = Color.FromArgb(0, 255, 255, 255);
            label6.BackColor = Color.FromArgb(0, 255, 255, 255);
            label7.BackColor = Color.FromArgb(0, 255, 255, 255);

            conn.Open();
            string sql = "select CName from crops ;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add($"{reader.GetString("CName")}");
            }
            reader.Dispose();
            conn.Close();
            

            int f=1;
            Random ran = new Random();
            int n = ran.Next(100000000);
            Pno = n.ToString();

            while(f==1)
            {
                conn.Open();
                string sql_isdouble = $"select PNo from plante where PNo= '{Pno}' ;";
                cmd = new MySqlCommand(sql_isdouble, conn);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ran = new Random();
                    n = ran.Next(100000000);
                    Pno = n.ToString();
                }
                else
                {
                    f = 0;
                }
                reader.Dispose();
                conn.Close();
            }
            textBox2.Text = Pno;
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            BackgroundImageLayout = ImageLayout.Stretch;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            int f = 1;
            Random ran = new Random();
            int n = ran.Next(100000000);
            Pno = n.ToString();

            while (f == 1)
            {
                conn.Open();
                string sql_isdouble = $"select PNo from plante where PNo= '{Pno}' ;";
                MySqlCommand cmd = new MySqlCommand(sql_isdouble, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ran = new Random();
                    n = ran.Next(100000000);
                    Pno = n.ToString();
                }
                else
                {
                    f = 0;
                }
                reader.Dispose();
                conn.Close();
            }
            textBox2.Text = Pno;
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            string selected;
            conn.Open();
            if(comboBox1.SelectedItem == null)
            {
                label7.Text = "请选择农产品!";
                conn.Close();
                return;
            }
            label7.Text = "";
            selected = comboBox1.SelectedItem.ToString();
            string sql = $"select CNo from crops where CName ='{selected}'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            Cno = reader.GetString("CNo");
            reader.Dispose();

            //判断是否输入是数字
            int tmp;
            if(!int.TryParse(textBox1.Text,out tmp))
            {
                label6.Text = "请输入数字!";
                conn.Close();
                return;
            }
            else
            {
                int num = 0;
                num = int.Parse(textBox1.Text);
                if (num <= 0)
                {
                    label6.Text = "请输入正整数";
                    conn.Close();
                    return;
                }
                else
                    label6.Text = "";
            }

            try
            {
                string sql_ins = $"insert into plante (PNo,FNo,CNo,ProdDate,Yeild,FStock,IsFresh) values('{textBox2.Text}','{user}','{Cno}','{dateTimePicker1.Value.ToString()}','{textBox1.Text}','{textBox1.Text}',0);";
                cmd = new MySqlCommand(sql_ins, conn);
                int result = cmd.ExecuteNonQuery();
                MessageBox.Show("入库成功!", "入库提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception e1)
            {
                MessageBox.Show("入库失败,请重新操作!", "入库提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Console.WriteLine(e1.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
