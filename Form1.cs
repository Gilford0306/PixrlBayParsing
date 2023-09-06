using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Windows.Forms;

namespace PixrlBayParsing
{
    public partial class Form1 : Form
    {
        string search = string.Empty;
        public Form1()
        {
            InitializeComponent();
            saveFileDialog1.Filter = "Picture files(*.jpg)|*.jpg|All files(*.*)|*.*";
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string url = $"https://pixabay.com/api/?key=39207646-8ccbcc7ff50179f772c7bccff&q={search}";
            WebClient client = new WebClient();
            string result = client.DownloadString(url);
            Root root = JsonSerializer.Deserialize<Root>(result);

            for (int i = 0; i < 15; i++)
            {

                try {
                        client.DownloadFile(root.hits[i].largeImageURL, $"{i}.jpg");

                    }     

                catch { }
                
            }

            List<PictureBox> picturebox = new List<PictureBox>();
            var CurDir = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = new DirectoryInfo(CurDir);
            var recentpics = directoryInfo.GetFiles().ToList();
            var y = 10;
            flowLayoutPanel1.Controls.Clear();
            for (int i = 0; i < 15; i++)
            {
                
                try
                {
                    var pb = new PictureBox();

                    pb.Location = new Point(picturebox.Count * 120 + 20, y);
                    pb.Size = new Size(200, 120);
                    pb.Name = i.ToString();

                    try
                    {
                        Image img;
                        using (var bmpTemp = new Bitmap($"{i}.jpg"))
                        {
                            img = new Bitmap(bmpTemp);
                        }
                        pb.Image = img;

                    }
                    catch (OutOfMemoryException) { continue; }
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    flowLayoutPanel1.Controls.Add(pb);
                    picturebox.Add(pb);
                    pb.Click += Pb_Click;
                }
                catch (Exception)
                {


                }

            }
        }

        private void Pb_Click(object sender, EventArgs e)
        {

            flowLayoutPanel2.Controls.Clear();
            pictureBox1.Image = Image.FromFile($"{(sender as PictureBox).Name.ToString()}.jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            flowLayoutPanel2.Controls.Add(pictureBox1);
        }



        private void button2_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = @"JPG|*.jpg" })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pictureBox1.Image.Save(saveFileDialog.FileName);
                        MessageBox.Show("File is save");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("File not saved");

                    }
                    
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
             search = textBox1.Text.ToLower().Replace(' ', '+');
        }


        
    }
}
