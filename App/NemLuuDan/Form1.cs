using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Xml;
using System.Media;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.Net;
using ExcelDataReader;

namespace NemLuuDan
{
	public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
	{
		int lan = 1;
		int soNguoi = 0;
		string lop = "";
		string xuatLop = "";
		string server = "http://192.168.1.1/";
		WiFiConnection connection = new WiFiConnection();
		Boolean wifi = false;
		Boolean nemTap = true;
		OleDbConnection conn = new OleDbConnection();
		string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "/DiemSo.accdb;Persist Security Info=True;Jet OLEDB:Database Password=2092003";
		string InputData = String.Empty;
		private string xuatlop;

		delegate void SetTextCallback(string text);
		public Form1()
		{
			InitializeComponent();
			Control.CheckForIllegalCrossThreadCalls = false;
			serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceive);
		}
		private void DataReceive(object obj, SerialDataReceivedEventArgs e)
		{
			InputData = serialPort1.ReadExisting();
			if (InputData != String.Empty)
			{
				SetText(InputData);
			}

		}
		private void SetText(string text)
		{
			if (this.textBox1.InvokeRequired)
			{
				SetTextCallback d = new SetTextCallback(SetText);
				this.Invoke(d, new object[] { text });
			}
			else this.textBox1.Text = text;
		}
		private void button1_Click(object sender, EventArgs e)
		{

		}

		private void layDiem(int diemNhan)
		{
			docDiem(diemNhan);
			string diem = diemNhan.ToString();
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
				if (!nemTap)
				{
					luuDiem();
					doiNguoi();
				}
				resetDiem();
				txtTimeName.Text = "Thời Gian Chuẩn Bị Ném";
				txtTime.Text = "10";
			}
		}
		void resetDiem()
		{
			txtDiem1.ResetText();
			txtDiem2.ResetText();
			txtDiem3.ResetText();
			txtDiem4.ResetText();
			txtDiem5.ResetText();
			txtDiem.ResetText();
		}
		private void resetThongTin()
		{
			label5.ResetText();
			label6.ResetText();
			label7.ResetText();
			soNguoi = 0;
		}
		private void doiNguoi()
		{
			if (soNguoi < gridView1.RowCount)
			{
				label5.Text = "Họ Và Tên: " + gridView1.GetRowCellValue(soNguoi, colHoVaTen).ToString();
				label6.Text = "Lớp: " + gridView1.GetRowCellValue(soNguoi, colLop).ToString();
				label7.Text = "Số Thứ Tự: " + gridView1.GetRowCellValue(soNguoi, colSTT).ToString();
				soNguoi++;
			}
			else
			{
				label5.Text = "";
				label6.Text = "";
				label7.Text = "";
			}
		}

		private void luuDiem()
		{
			string STT = label7.Text.Replace("Số Thứ Tự:", "").Trim();
			string Lop = label6.Text.Replace("Lớp:", "").Trim();
			conn.ConnectionString = connectionString;
			conn.Open();
			OleDbCommand cmd = new OleDbCommand();
			cmd.Connection = conn;
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = System.String.Concat("Update DiemSo SET DiemLan1='" + txtDiem1.Text + "', DiemLan2='" + txtDiem2.Text + "', DiemLan3='" + txtDiem3.Text + "', DiemLan4='" + txtDiem4.Text + "', DiemLan5='" + txtDiem5.Text + "' Where STT='" + STT + "'AND Lop='"+Lop+"'");
			cmd.CommandType = CommandType.Text;
			cmd.ExecuteNonQuery();
			conn.Close();
		}
		private void getData()
		{
			Action action = new Action(actionGetData);
			Task task = new Task(action);
			task.Start();
		}
		private void actionGetData()
		{
			string link = server + "getData" + lan;
			WebClient client = new WebClient();
			Stream data = client.OpenRead(new Uri(link));
			StreamReader reader = new StreamReader(data);
			int diem = int.Parse(reader.ReadToEnd());
			if (diem >= 0)
			{
				layDiem(diem);
				if(lan < 5)
				{
					lan++;
				}
				else
				{
					lan = 1;
				}
			}	
			data.Close();
			reader.Close();
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
				txtDiem.Text = "";
				txtTimeName.Text = "Thời Gian Ném Lựu Đạn";
				txtTime.Text = "60";
			}
			else
			{
				webBrowser1.Navigate(server + "getZero");
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

		private void label7_Click(object sender, EventArgs e)
		{

		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (!serialPort1.IsOpen)
			{
				serialPort1.PortName = SerialPort.GetPortNames()[0];
				serialPort1.BaudRate = 115200;
				serialPort1.Open();
			}
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			serialPort1.Close();
			serialPort1.Dispose();
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			if (IsNumber(textBox1.Text.Trim()) && Convert.ToInt32(textBox1.Text) > 0)
			{
				layDiem(int.Parse(textBox1.Text.Trim()));

			}
		}

		private void docDiem(int v)
		{
			string duongDan = Application.StartupPath + "/DocDiem/";
			switch (v)
			{
				case 4:
					duongDan += "DIEM4.WAV";
					break;
				case 5:
					duongDan += "DIEM5.WAV";
					break;
				case 6:
					duongDan += "DIEM6.WAV";
					break;
				case 7:
					duongDan += "DIEM7.WAV";
					break;
				case 8:
					duongDan += "DIEM8.WAV";
					break;
				case 9:
					duongDan += "DIEM9.WAV";
					break;
				case 10:
					duongDan += "DIEM10.WAV";
					break;
			}
			SoundPlayer sound = new SoundPlayer(duongDan);
			sound.Play();
		}

		public bool IsNumber(string pValue)
		{
			foreach (Char c in pValue)
			{
				if (!Char.IsDigit(c))
					return false;
			}
			return true;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			ketNoiCoDay.Checked = true;

		}
		private void kiemTra()
		{
			conn.ConnectionString = connectionString;
			conn.Open();
			DataSet ds = new DataSet();
			OleDbDataAdapter da = new OleDbDataAdapter("Select * from DiemSo Where Lop ='" + lop + "'", conn);
			da.Fill(ds);
			conn.Close();
			conn.Dispose();
			gridControl1.DataSource = ds.Tables[0];
			if (gridView1.RowCount > 0)
			{
				doiNguoi();
			}
			else
			{
				resetThongTin();
				label14.Visible = true;
				label13.Visible = true;
				nemTap = true;
				resetDiem();
				DevExpress.XtraEditors.XtraMessageBox.Show("Tên lớp không hợp lệ\nVui lòng chọn lại", "Thông báo");
			}
		}
		private void button1_Click_1(object sender, EventArgs e)
		{
			if (!wifi)
			{
				try
				{
					if (!serialPort1.IsOpen)
					{
						serialPort1.PortName = SerialPort.GetPortNames()[0];
						serialPort1.BaudRate = 115200;
						serialPort1.Open();
					}
					timer1.Enabled = true;
				}
				catch
				{
					MessageBox.Show("Vui lòng kêt nối tới Arduino", "Thông báo");
				}
			}
			else
			{
				if (connection.testWiFi(server))
				{
					timer2.Enabled = true;
					timer1.Enabled = true;
				}
				else
				{
					MessageBox.Show("Vui lòng kiểm tra lại WiFi", "Thông báo");
				}
			}
		}

		private void ketNoiCoDay_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			ketNoiWifi.Checked = false;
			wifi = false;
		}

		private void ketNoiWifi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			ketNoiCoDay.Checked = false;
			wifi = true;
		}

		private void timer2_Tick(object sender, EventArgs e)
		{
			getData();
		}

		private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			getDuLieuTuExcel();
			luuDuLieuHocSinh();

		}

		private void luuDuLieuHocSinh()
		{
			conn.ConnectionString = connectionString;
			conn.Open();
			OleDbCommand cmd = new OleDbCommand();
			cmd.Connection = conn;
			cmd.CommandType = CommandType.Text;
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				cmd.CommandText = System.String.Concat("INSERT INTO DiemSo(STT, HoVaTen, Lop) VALUES ('"+ row.Cells[0].Value+"','"+ row.Cells[2].Value + "','"+ row.Cells[1].Value + "')");
				cmd.CommandType = CommandType.Text;
				cmd.ExecuteNonQuery();
			}
			conn.Close();
			DevExpress.XtraEditors.XtraMessageBox.Show("Thêm dữ liệu thành công","Thông báo");
		}

		void getDuLieuTuExcel()
		{
			DataSet ds;
			using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx|Excel Workbook 97-2003|*.xls", ValidateNames = true })
			{
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					using (var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read))
					{
						IExcelDataReader reader;
						if (ofd.FilterIndex == 2)
						{
							reader = ExcelReaderFactory.CreateBinaryReader(stream);
						}
						else
						{
							reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
						}

						ds = reader.AsDataSet(new ExcelDataSetConfiguration()
						{
							ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
							{
								UseHeaderRow = true
							}
						});
						dataGridView1.DataSource = ds.Tables[0];
						reader.Close();
					}
				}
			}
		}

		private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			label14.Visible = false;
			label13.Visible = false;
			nemTap = false;
			string value = "";

			if (ChonLopDialog.Show("Vui lòng nhập tên lớp",
				"Nhập tên lớp vào đây:", ref value) == DialogResult.OK)
			{
				if(value != "")
				{
					lop = value;
					kiemTra();
				}
			}
		}

		private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			string value = "";

			if (ChonLopDialog.Show("Vui lòng nhập tên lớp",
				"Nhập tên lớp vào đây:", ref value) == DialogResult.OK)
			{
				if (value != "")
				{
					xuatlop = value;
					xuatDuLieu(0);
				}
			}
		}

		private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			string value = "";

			if (ChonLopDialog.Show("Vui lòng nhập tên lớp",
				"Nhập tên lớp vào đây:", ref value) == DialogResult.OK)
			{
				if (value != "")
				{
					xuatlop = value;
					xuatDuLieu(1);
				}
			}
		}

		private void xuatDuLieu(int type)
		{
			conn.ConnectionString = connectionString;
			conn.Open();
			DataSet ds = new DataSet();
			OleDbDataAdapter da = new OleDbDataAdapter("Select * from DiemSo Where Lop ='" + xuatlop + "'", conn);
			da.Fill(ds);
			conn.Close();
			conn.Dispose();
			gridControl2.DataSource = ds.Tables[0];
			if (gridView2.RowCount > 0)
			{
				if (type == 0)
				{
					SaveFileDialog saveFileDialog1 = new SaveFileDialog();
					saveFileDialog1.Filter = "Excel Workbook| *.xlsx | Excel Workbook 97 - 2003 | *.xls";
					saveFileDialog1.ValidateNames = true;
					saveFileDialog1.Title = "Save an Excel File";
					if (saveFileDialog1.ShowDialog() == DialogResult.OK)
					{
						if (saveFileDialog1.FilterIndex == 2)
						{
							gridControl2.ExportToXls(saveFileDialog1.FileName);
						}
						else
						{
							gridControl2.ExportToXlsx(saveFileDialog1.FileName);
						}
					}
				}
				else if (type == 1)
				{
					SaveFileDialog saveFileDialog1 = new SaveFileDialog();
					saveFileDialog1.Filter = "PDF File|*.pdf";
					saveFileDialog1.Title = "Save an PDF File";
					if (saveFileDialog1.ShowDialog() == DialogResult.OK)
					{
						gridControl2.ExportToPdf(saveFileDialog1.FileName);
					}
				}
			}
			else
			{
				DevExpress.XtraEditors.XtraMessageBox.Show("Tên lớp không hợp lệ\nVui lòng chọn lại", "Thông báo");
			}		
		}

		private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
		{
			label14.Visible = true;
			label13.Visible = true;
			nemTap = true;
			resetThongTin();
			resetDiem();
		}

		private void ribbonControl1_Click(object sender, EventArgs e)
		{

		}
	}
}