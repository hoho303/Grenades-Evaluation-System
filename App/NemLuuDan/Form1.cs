using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NemLuuDan
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            layDiem();
        }

        private void layDiem()
        {
            
            string diem = txtNhapDiem.Text;
            if (diem != null)
            {
                txtTimeName.Text = "Thời Gian Chuẩn Bị Ném";
                txtTime.Text = "10";
                if (diem.Length < 2)
                    txtDiem.Text = "0" + diem;
                else txtDiem.Text = diem;

                if (txtDiem1.Text == "")
                    txtDiem1.Text = diem;
                else if (txtDiem2.Text == "")
                    txtDiem2.Text = diem;
                else if (txtDiem3.Text == "")
                    txtDiem3.Text = diem;
                else if (txtDiem4.Text == "")
                    txtDiem4.Text = diem;
                else if (txtDiem5.Text == "")
                {
                    txtDiem5.Text = diem;
                    txtDiem1.ResetText();
                    txtDiem2.ResetText();
                    txtDiem3.ResetText();
                    txtDiem4.ResetText();
                    txtDiem5.ResetText();
                    txtDiem.ResetText();
                    txtTimeName.Text = "Thời Gian Chuẩn Bị Ném";
                    txtTime.Text = "10";
                }
            }        
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int time = Convert.ToInt32(txtTime.Text);
            if (time > 0)
            {
                txtTime.Text = (time - 1).ToString();
            }
            else if (txtTimeName.Text.Equals("Thời Gian Chuẩn Bị Ném"))
            {
                txtTimeName.Text = "Thời Gian Ném Lựu Đạn";
                txtTime.Text = "60";
            }
            else
            {
                txtTimeName.Text = "Thời Gian Chuẩn Bị Ném";
                txtTime.Text = "10";
                if (txtDiem1.Text == "")
                    txtDiem1.Text = "0";
                else if (txtDiem2.Text == "")
                    txtDiem2.Text = "0";
                else if (txtDiem3.Text == "")
                    txtDiem3.Text = "0";
                else if (txtDiem4.Text == "")
                    txtDiem4.Text = "0";
                else if (txtDiem5.Text == "")
                {
                    txtDiem5.Text = "0";
                    txtDiem1.ResetText();
                    txtDiem2.ResetText();
                    txtDiem3.ResetText();
                    txtDiem4.ResetText();
                    txtDiem5.ResetText();
                    txtDiem.ResetText();
                    txtTimeName.Text = "Thời Gian Chuẩn Bị Ném";
                    txtTime.Text = "10";
                }
            }
         }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}