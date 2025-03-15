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
    public partial class AdcionarProduto : Form
    {
        static conBanco con = new conBanco();
        public AdcionarProduto()
        {
            InitializeComponent();
        }

        private void AdcionarProduto_Load(object sender, EventArgs e)
        {
            con.liga = new FireSharp.FirebaseClient(con.config);
            radioButton1.Checked = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Selecione uma imagem";
            ofd.Filter = "image files(*.jpeg) | *.jpeg";

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = new Bitmap(ofd.FileName);

                pictureBox1.Image = img.GetThumbnailImage(500,500,null,new IntPtr());
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            
        }

        private  void button3_Click(object sender, EventArgs e)
        {


        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox3.Text == ""|| textBox2.Text == ""|| pictureBox1.Image == null)
            {
                MessageBox.Show("Por Favor Insira todas as informaçoes");
            }
            else
            {
                try
                {
                    this.Enabled = false;

                    FirebaseResponse resp9 = await con.liga.GetAsync("CounterSabor/node/cnt");
                    int totall = resp9.ResultAs<int>();
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, ImageFormat.Jpeg);

                    byte[] a = ms.GetBuffer();

                    string output = Convert.ToBase64String(a);



                    var data = new AdicionarPed
                    {
                        id = totall + 1,
                        Ingredientes = textBox2.Text,
                        Nome = textBox1.Text,
                        Preco = float.Parse(textBox3.Text),
                        status = "off",
                        QuantPedida = "0",
                        image = output

                    };

                    int h = totall + 1;

                    for(int i = 1; i <= 12; i++)
                    {
                        SetResponse response2 = await con.liga.SetAsync($"HistRelatorio/Mes/{i}/TotalPedidos/{h}", 0);
                    }

                    SetResponse response = await con.liga.SetAsync("Produtos/"+h, data);
                    SetResponse response1 = await con.liga.SetAsync("CounterSabor/node/cnt", h);
                    textBox1.Clear();
                    textBox3.Clear();
                    textBox2.Clear();
                    pictureBox1.Image = null;

                    this.Enabled = true;

                }
                catch
                {
                    MessageBox.Show("Por Favor Coloque apenas numeros");
                }
            }

            



        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
            
            label2.Enabled = true;
            textBox2.Enabled = true;
            textBox2.Text = null;           
           
            
                
                
            

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label2.Enabled = false;
            textBox2.Enabled = false;
            textBox2.Text = "<Bebida>";
        }
    }
}
