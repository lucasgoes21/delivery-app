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
    public partial class EditarProduto : Form
    {
        static conBanco con = new conBanco();
        
        public EditarProduto()
        {
            InitializeComponent();
        }

        private void EditarProduto_Load(object sender, EventArgs e)
        {

            con.liga = new FireSharp.FirebaseClient(con.config);
            Edit();
            


        }

        public async void Edit()
        {
            FirebaseResponse res1 = await con.liga.GetAsync("Produtos/"+label5.Text);
            AdicionarPed edit = res1.ResultAs<AdicionarPed>();

            if(edit.Ingredientes == "Bebida")
            {
                textBox1.Text = edit.Nome;
                textBox2.Text = edit.Ingredientes;
                textBox3.Text = edit.Preco.ToString("F");

                textBox2.Enabled = false;
            }
            else
            {
                textBox1.Text = edit.Nome;
                textBox2.Text = edit.Ingredientes;
                textBox3.Text = edit.Preco.ToString("F");
            }

            

            byte[] b = Convert.FromBase64String(edit.image);

            MemoryStream ms = new MemoryStream();

            ms.Write(b, 0, Convert.ToInt32(b.Length));

            Bitmap bm = new Bitmap(ms, false);

            pictureBox1.Image = bm;

            pictureBox2.Image = bm;



        }
        public async void Adetar()
        {
            if (textBox1.Text == "" || textBox3.Text == "" || textBox2.Text == "" || pictureBox1.Image == null)
            {
                MessageBox.Show("Por Favor Insira todas as informaçoes");
            }
            else
            {
                try
                {
                    this.Enabled = false;


                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, ImageFormat.Jpeg);

                    byte[] a = ms.GetBuffer();

                    string output = Convert.ToBase64String(a);
                    FirebaseResponse tot = await con.liga.GetAsync($"Produtos/{label5.Text}/QuantPedida");
                    string toto = tot.ResultAs<string>();


                    var data = new AdicionarPed
                    {
                        Ingredientes = textBox2.Text,
                        Nome = textBox1.Text,
                        Preco = float.Parse(textBox3.Text),
                        status = "off",
                        QuantPedida = toto,
                        image = output

                    };

                    SetResponse response = await con.liga.SetAsync("Produtos/" + label5.Text, data);
                    textBox1.Clear();
                    textBox3.Clear();
                    textBox2.Clear();
                    pictureBox1.Image = null;



                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Por Favor Coloque apenas numeros");
                }
            }
        }


        private  void button2_Click(object sender, EventArgs e)
        {
            Adetar();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Selecione uma imagem";
            ofd.Filter = "image files(*.jpeg) | *.jpeg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = new Bitmap(ofd.FileName);

                pictureBox1.Image = img.GetThumbnailImage(500, 500, null, new IntPtr());
            }
        }

        public async void button3_Click(object sender, EventArgs e)
        {
            FirebaseResponse res1 = await con.liga.GetAsync("Produtos/" + label5.Text);
            AdicionarPed edit = res1.ResultAs<AdicionarPed>();

            if (pictureBox2.Image == pictureBox1.Image && textBox1.Text == edit.Nome && textBox2.Text == edit.Ingredientes && float.Parse( textBox3.Text) == edit.Preco)
            {
                this.Close();
            }
            else
            {
                DialogResult result = MessageBox.Show("Deseja Salvar as alteraçoes", "Confirmar", MessageBoxButtons.YesNoCancel);
                if(result == DialogResult.Yes)
                {

                    Adetar();
                }
                else if(result == DialogResult.No)
                {
                    this.Close();
                }
                else
                {

                }
            }



        }
    }
}
