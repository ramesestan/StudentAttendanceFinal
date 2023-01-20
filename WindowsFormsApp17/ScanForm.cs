using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Aztec;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;

namespace WindowsFormsApp17
{
    public partial class ScanForm : Form
    {
        private MySqlConnection con = new MySqlConnection();
        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;
        public ScanForm()
        {
            InitializeComponent();
            con.ConnectionString = @"server=localhost;database=user_infotb;userid=root;password=;";
        }
        private void ScanForm_Load(object sender, EventArgs e)
        {
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in CaptureDevice)
                comboBox1.Items.Add(Device.Name);

            comboBox1.SelectedIndex = 0;
            FinalFrame = new VideoCaptureDevice();
            //Date and Time
            label2.Text = DateTime.Now.ToLongDateString();
            time_text.Text = DateTime.Now.ToLongTimeString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // open camera 
            FinalFrame = new VideoCaptureDevice(CaptureDevice[comboBox1.SelectedIndex].MonikerString);
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            FinalFrame.Start();
        }
        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox2.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void ScanForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FinalFrame.IsRunning == true)
                FinalFrame.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //BarcodeReader reader = new BarcodeReader();
            //Result result = reader.Decode((Bitmap)pictureBox2.Image);
            //try
            //{
            //    string decoded = result.ToString().Trim();
            //    ID_text.Text = decoded;
            //    if (decoded != null)
            //    {
            //        con.Open();
            //        MySqlCommand coman = new MySqlCommand();
            //        coman.Connection = con;
            //        coman.CommandText = "SELECT * FROM  registrationtb  WHERE ID LIKE'%" + ID_text.Text + "%'";
            //        MySqlDataReader dr = coman.ExecuteReader();
            //        dr.Read();
            //        if (dr.HasRows)
            //        {
            //            Name_text.Text = dr["Name"].ToString();
            //            Email_text.Text = dr["EmailAddress"].ToString();
            //            Dateofbirth_text.Text = dr["DateOfBirth"].ToString();
            //            Class_text.Text = dr["Class"].ToString();
            //            Phone_text.Text = dr["PhoneNumber"].ToString();
            //            gender_text.Text = dr["Gender"].ToString();
            //            byte[] img = ((byte[])dr["Photo"]);
            //            MemoryStream ms = new MemoryStream(img);
            //            pictureBox1.Image = Image.FromStream(ms);


            //        }

            //        con.Close();
            //        ();timer2.Start
            //        MessageBox.Show("Data Saved Successfully!");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error " + ex);
            //}
            //try
            //{
            //    BarcodeReader reader = new BarcodeReader();
            //    Result result = reader.Decode((Bitmap)pictureBox2.Image);
            //    if (result != null)
            //    {
            //        string decoded = result.ToString().Trim();
            //        ID_text.Text = decoded;
            //        if (decoded != null)
            //        {
            //            con.Open();
            //            MySqlCommand coman = new MySqlCommand();
            //            coman.Connection = con;
            //            coman.CommandText = "SELECT * FROM  registrationtb  WHERE ID LIKE'%" + ID_text.Text + "%'";
            //            MySqlDataReader dr = coman.ExecuteReader();
            //            if (dr.HasRows)
            //            {
            //                dr.Read();
            //                Name_text.Text = dr["Name"].ToString();
            //                Email_text.Text = dr["EmailAddress"].ToString();
            //                Dateofbirth_text.Text = dr["DateOfBirth"].ToString();
            //                Class_text.Text = dr["Class"].ToString();
            //                Phone_text.Text = dr["PhoneNumber"].ToString();
            //                gender_text.Text = dr["Gender"].ToString();
            //                byte[] img = ((byte[])dr["Photo"]);
            //                if (img != null)
            //                {
            //                    MemoryStream ms = new MemoryStream(img);
            //                    pictureBox1.Image = Image.FromStream(ms);
            //                }
            //                else
            //                {
            //                    MessageBox.Show("No Image Found");
            //                }
            //            }
            //            con.Close();
            //            timer2.Start();
            //            MessageBox.Show("Data Saved Successfully!");
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error: " + ex.Message);
            //}

            //MessageBox.Show("alert");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //scan qr code for attendance recording
            try
            {
                BarcodeReader reader = new BarcodeReader();
                Result result = reader.Decode((Bitmap)pictureBox2.Image);
                if (result != null || true)
                {
                    //for testing use this
                    //string decoded = "ID#TEST";
                    string decoded = result.ToString().Trim();
                    ID_text.Text = decoded;
                    if (decoded != null)
                    {
                        this.FetchData();

                    }
                    else 
                    {
                        MessageBox.Show("Valid");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Result");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void Name_text_TextChanged(object sender, EventArgs e)
        {


        }

        private void gender_text_TextChanged(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
 
            //try
            //{
            //    //for attendance
            //    if (pictureBox1.Image != null)
            //    {
            //        MemoryStream ms = new MemoryStream();
            //        pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //        byte[] Photo = new byte[ms.Length];
            //        ms.Position = 0;
            //        ms.Read(Photo, 0, Photo.Length);

            //        con.Open();
            //        MySqlCommand coman = new MySqlCommand();
            //        coman.Connection = con;
            //        coman.CommandText = "insert into attendancetb (ID,Name,EmailAddress,DateOfBirth,Class,PhoneNumber,Gender,InTime,Photo) values('" + ID_text.Text + " ', ' " + Name_text.Text + " ',' " + Email_text.Text + " ','" + Dateofbirth_text.Text + "','" + Class_text.Text + "','" + Phone_text.Text + "','" + gender_text.Text + "','" + time_text.Text + "',@photo)";
            //        coman.Parameters.AddWithValue("@photo", Photo);
            //        coman.ExecuteNonQuery();
            //        con.Close();
            //        MessageBox.Show("Data Saved Successfully!");
            //    }
            //    else
            //    {
            //        MessageBox.Show("Image not set. Please select an image.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error: " + ex.Message);
            //    // log the error for later review
            //    // Example: File.AppendAllText("error.log", ex.ToString());
            //}
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG|*.jpg;*.jpeg|PNG|*.png|BMP|*.bmp", ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox2.Image = Image.FromFile(ofd.FileName);
                    BarcodeReader reader = new BarcodeReader();
                    Result result = reader.Decode((Bitmap)pictureBox2.Image);
                    if (result != null)
                    {
                        
                        string decoded = result.ToString().Trim();
                        ID_text.Text = decoded;
                        if (decoded != null)
                        {
                            try
                            {
                                this.FetchData();
                               // MessageBox.Show("Data Saved Successfully!");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error in Browse Click: " + ex.Message);

                            }
                        }
                    }
                }
            }
        }

        private void FetchData()
        {
            con.Open();
            MySqlCommand coman = new MySqlCommand();
            coman.Connection = con;
            coman.CommandText = "SELECT * FROM  registrationtb  WHERE ID LIKE'%" + ID_text.Text + "%'";
            MySqlDataReader dr = coman.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                Name_text.Text = dr["Name"].ToString();
                Email_text.Text = dr["EmailAddress"].ToString();
                Dateofbirth_text.Text = dr["DateOfBirth"].ToString();
                Class_text.Text = dr["Class"].ToString();
                Phone_text.Text = dr["PhoneNumber"].ToString();
                gender_text.Text = dr["Gender"].ToString();
                byte[] img = ((byte[])dr["Photo"]);
                if (img != null)
                {
                    MemoryStream ms = new MemoryStream(img);
                    pictureBox1.Image = Image.FromStream(ms);
                }
                else
                {
                    MessageBox.Show("No Image Found");
                }
                MessageBox.Show("Data Fetched Successfully!");
                con.Close();
                this.SaveAttendanceData();

            }

            else
            {
                MessageBox.Show("No Data Found");
                con.Close();
            }
           
        }

        private void SaveAttendanceData()
        {

            MemoryStream ms = new MemoryStream();
            pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] Photo = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(Photo, 0, Photo.Length);



            con.Open();
            MySqlCommand coman = new MySqlCommand();
            coman.Connection = con;
            coman.CommandText = "insert into attendancetb (ID,Name,EmailAddress,DateOfBirth,Class,PhoneNumber,Gender,TimeIn,Photo) values('" + ID_text.Text + " ', ' " + Name_text.Text + " ',' " + Email_text.Text + " ','" + Dateofbirth_text.Text + "','" + Class_text.Text + "','" + Phone_text.Text + "','" + gender_text.Text + "','" + time_text.Text + "',@photo)";
            coman.Parameters.AddWithValue("@photo", Photo);
            coman.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Data Saved Successfully!");


        }
    }
}


