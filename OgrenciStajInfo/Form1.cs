using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;   

namespace OgrenciStajInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button3.Enabled = button4.Enabled = false;
            label1.Visible = label11.Visible = false;   
        }
        OleDbConnection baglanti;
        OleDbCommand komut;
        OleDbDataAdapter da;

        int count = 0;
        void VerileriGoster()
        {
            baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\NewDB.accdb");
            baglanti.Open();
            da = new OleDbDataAdapter("Select * From Bilgiler", baglanti);
            System.Data.DataTable tablo = new System.Data.DataTable();
            da.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            VerileriGoster();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || comboBox2.Text == "" || comboBox3.Text == "")
            {

                MessageBox.Show("Boş alanları doldurun");
            }
            else
            {

                VerileriGoster();
                string sorgu = "Insert into Bilgiler(AdSoyad, Numara, Program, UygulamaYeri, UygulamaYili, Sehir,Telefon, eposta) values(@AdSoyad, @Numara, @Program, @UygulamaYeri, @UygulamaYili, @Sehir, @Telefon, @eposta)";
                komut = new OleDbCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@AdSoyad", textBox2.Text);
                komut.Parameters.AddWithValue("@Numara", textBox3.Text);
                komut.Parameters.AddWithValue("@Program", textBox4.Text);
                komut.Parameters.AddWithValue("@UygulamaYeri", textBox5.Text);
                komut.Parameters.AddWithValue("@UygulamaYili", comboBox2.Text);
                komut.Parameters.AddWithValue("@Sehir", comboBox3.Text);
                komut.Parameters.AddWithValue("@Telefon", maskedTextBox1.Text);
                komut.Parameters.AddWithValue("@eposta", textBox7.Text);
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                VerileriGoster();//güncel hali için
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult silmeOnayi = MessageBox.Show(seciliIsım + " adlı öğrenciyi silmek istiyor musunuz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (silmeOnayi == DialogResult.Yes)
            {
                string sorgu = "Delete From Bilgiler where ID=@ID";
                komut = new OleDbCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@ID", dataGridView1.CurrentRow.Cells[0].Value);
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                VerileriGoster();
                textBox2.Text = textBox3.Text = textBox4.Text = maskedTextBox1.Text = textBox5.Text = textBox7.Text = null;
                comboBox2.SelectedItem = comboBox3.SelectedItem = null;

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                MessageBox.Show("Tarih seçiniz");
            }
            else
            {
                string sorgu = "Update Bilgiler Set AdSoyad=@AdSoyad, Numara=@Numara, Program=@Program, UygulamaYeri=@UygulamaYeri, UygulamaYili=@UygulamaYili, Sehir =@Sehir, Telefon=@Telefon, eposta=@eposta where ID=@ID";
                komut = new OleDbCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@AdSoyad", textBox2.Text);
                komut.Parameters.AddWithValue("@Numara", textBox3.Text);
                komut.Parameters.AddWithValue("@Program", textBox4.Text);
                komut.Parameters.AddWithValue("@UygulamaYeri", textBox5.Text);
                komut.Parameters.AddWithValue("@UygulamaYili", comboBox2.Text);
                komut.Parameters.AddWithValue("@Sehir", comboBox3.Text);
                komut.Parameters.AddWithValue("@Telefon", maskedTextBox1.Text);
                komut.Parameters.AddWithValue("@eposta", textBox7.Text);
                komut.Parameters.AddWithValue("@ID", Convert.ToInt32(label11.Text));
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
                VerileriGoster();
            }
        }

        void Tarih()
        {
           
            comboBox2.Items.Clear();
            try
            {

                for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 7; i--)
                {
                    comboBox2.Items.Add(i);
                }
                comboBox2.SelectedItem = DateTime.Now.Year.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            Tarih();
        }

        string seciliIsım;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            button3.Enabled = button4.Enabled = true;
            label11.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            seciliIsım = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = seciliIsım;
            textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            comboBox2.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            comboBox3.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            maskedTextBox1.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            textBox7.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            VerileriGoster();
            count = 0;
            baglanti.Open();
            string sorgu = "Select * From Bilgiler where AdSoyad like'%" + textBox8.Text + "%'";
            komut = new OleDbCommand(sorgu, baglanti);
            komut.ExecuteNonQuery();
            System.Data.DataTable tablo = new System.Data.DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(komut);
            da.Fill(tablo);
            count = Convert.ToInt32(tablo.Rows.Count.ToString());
            dataGridView1.DataSource = tablo;
            baglanti.Close();

            if (count == 0)
            {
                MessageBox.Show("Kayıt bulunamadı", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {

            if (textBox2.Text == "")
            {

            }
            else
            {
                string kelime;
                string[] kelimeler = textBox2.Text.Split(' ');

                for (int i = 0; i < kelimeler.Length; i++)
                {
                    kelimeler[i] = kelimeler[i].Substring(0, 1).ToUpper() + kelimeler[i].Substring(1).ToLower();
                }
                kelime = String.Join(" ", kelimeler);
                textBox2.Text = kelime;
            }
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button5.PerformClick();
            }
        }

        public void Temizle()
        {
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            textBox7.Text = null;
            maskedTextBox1.Text = null;
            comboBox2.Text = null;
            comboBox3.Text = null;
        }
        private void btnTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\NewDB.accdb");

            for (int Satir = 0; Satir < dataGridView1.Rows.Count - 1; Satir++)
                {
                string AccessSorgu = "insert into Bilgiler (AdSoyad, Numara, Program, UygulamaYeri, UygulamaYili, Sehir,Telefon, eposta) values(@AdSoyad, @Numara, @Program, @UygulamaYeri, @UygulamaYili, @Sehir, @Telefon, @eposta)";

                komut = new OleDbCommand(AccessSorgu, baglanti);
                baglanti.Open();
            
                komut.Parameters.AddWithValue("@AdSoyad", dataGridView1.Rows[Satir].Cells["dg_AdSoyad"].Value.ToString());
                komut.Parameters.AddWithValue("@Numara", dataGridView1.Rows[Satir].Cells["dg_Numara"].Value.ToString());
                komut.Parameters.AddWithValue("@Program", dataGridView1.Rows[Satir].Cells["dg_Program"].Value.ToString());
                komut.Parameters.AddWithValue("@UygulamaYeri", dataGridView1.Rows[Satir].Cells["dg_UygulamaYeri"].Value.ToString());
                komut.Parameters.AddWithValue("@UygulamaYili", dataGridView1.Rows[Satir].Cells["dg_UygulamaYili"].Value.ToString());
                komut.Parameters.AddWithValue("@Sehir", dataGridView1.Rows[Satir].Cells["dg_Sehir"].Value.ToString());
                komut.Parameters.AddWithValue("@Telefon", dataGridView1.Rows[Satir].Cells["dg_Telefon"].Value.ToString());
                komut.Parameters.AddWithValue("@eposta", dataGridView1.Rows[Satir].Cells["dg_Mail"].Value.ToString());

                komut.ExecuteNonQuery();
                baglanti.Close();
            }
           

            MessageBox.Show("Kayıtlar eklendi","Aktarma Başarılı",MessageBoxButtons.OK);
             
        }

        private void button7_Click(object sender, EventArgs e)
        {
          
            openFileDialog1.Filter = "Excel Çalışma Sayfası|*.xlsx";
            openFileDialog1.Title = "Lütfen bir excel çalışma sayfası seçin";
           

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string Liste = openFileDialog1.FileName;
                txYol.Text = Liste;

                string komut = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source =" + Liste + ";Extended Properties = 'Excel 12.0 Xml;HDR=Yes; IMEX=1;'";
                OleDbConnection excelBaglanti = new OleDbConnection(komut);
                OleDbCommand excelCmd = new OleDbCommand("Select * From [Sayfa1$]", excelBaglanti);
                excelBaglanti.Open();
                OleDbDataAdapter ExcelTabloCek = new OleDbDataAdapter(excelCmd);
                DataTable ExcelVeri = new DataTable();
                ExcelTabloCek.Fill(ExcelVeri);
                dataGridView1.DataSource = ExcelVeri;
                excelBaglanti.Close();
            }
           

        }
    }    
}          