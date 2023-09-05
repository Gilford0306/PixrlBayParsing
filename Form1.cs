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
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Windows.Forms;

namespace PixrlBayParsing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            saveFileDialog1.Filter = "Picture files(*.jpg)|*.jpg|All files(*.*)|*.*";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text.ToLower().Replace(' ', '+');
            string url = $"https://pixabay.com/api/?key=39207646-8ccbcc7ff50179f772c7bccff&q={str}";
            WebClient client = new WebClient();
            string result = client.DownloadString(url);
            Root root = JsonSerializer.Deserialize<Root>(result);
            
            for (int i = 0; i < 20; i++)
            {
                try { client.DownloadFile(root.hits[i].largeImageURL, $"{i}.jpg");  }     

                catch { }
                
            }

            //for (int i = 0; i < 10; i++)
            //{
            //    PictureBox picture = new PictureBox
            //    {
            //        Name = $"pictureBox{i}",
            //        ImageLocation = $@"{i}.jpg",
            //        SizeMode = PictureBoxSizeMode.StretchImage
            //    };
            //    pictureBox.Controls.Add(picture);
            //}
            //PictureBox picture = new PictureBox
            //{
            //    Name = "pictureBox",
            //    ImageLocation = @"0.jpg",
            //    SizeMode = PictureBoxSizeMode.StretchImage
            //};

            //pictureBox1.Controls.Add(picture);
            //picture = new PictureBox
            //{
            //    Name = "pictureBox",
            //    ImageLocation = @"1.jpg",
            //    SizeMode = PictureBoxSizeMode.StretchImage
            //};
            //pictureBox2.Controls.Add(picture);


            List<PictureBox> picturebox = new List<PictureBox>();
            var CurDir = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = new DirectoryInfo(CurDir);
            var recentpics = directoryInfo.GetFiles().ToList();
            var y = 10;
            flowLayoutPanel1.Controls.Clear();
            for (int i = 0; i < 20; i++)
                {
                try
                {
                    var pb = new PictureBox();

                    pb.Location = new Point(picturebox.Count * 120 + 20, y);
                    pb.Size = new Size(200, 120);
                    pb.Name = i.ToString();
                    try
                    {

                        pb.Image = Image.FromFile($"{i}.jpg");
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
    }
}
