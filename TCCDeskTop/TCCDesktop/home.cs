using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace TCCDesktop
{
    public partial class home : Form
    {
        static conBanco con = new conBanco();
        public home()
        {
            InitializeComponent();
        }

        private void home_Load(object sender, EventArgs e)
        {
            con.liga = new FireSharp.FirebaseClient(con.config);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.liga = new FireSharp.FirebaseClient(con.config);
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Selecione uma imagem";
            ofd.Filter = "image files(*.jpeg) | *.jpeg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = new Bitmap(ofd.FileName);

                pictureBox1.Image = img.GetThumbnailImage(500, 500, null, new IntPtr());
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {

            if(textBox1.Text != "")
            {
                con.liga = new FireSharp.FirebaseClient(con.config);
                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, ImageFormat.Jpeg);

                byte[] a = ms.GetBuffer();

                string output = Convert.ToBase64String(a);
                FirebaseResponse resp9 = await con.liga.GetAsync("CaunterImg");
                int b = resp9.ResultAs<int>();


                SetResponse response2 = await con.liga.SetAsync($"ImagensPI/{b}/img", output);
                SetResponse response3 = await con.liga.SetAsync($"ImagensPI/{b}/title", textBox1.Text);
                SetResponse response1 = await con.liga.SetAsync($"CaunterImg", b + 1);

                MessageBox.Show("A imagem foi enviada com sucesso");
                textBox1.Clear();
                pictureBox1.Image = null;
            }
            else
            {
                MessageBox.Show("De um titulo para a imagem");
            }
            
        }
    }
}
