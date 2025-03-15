using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCCDesktop
{
    public partial class AddPedido : Form
    {

        static conBanco con = new conBanco();
        public int a;

        public  AddPedido()
        {
            InitializeComponent();

            label1.Text = a.ToString();
            CarregarImp();

            


        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            



        }

        private void button1_Click(object sender, EventArgs e)
        {
            


        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void AddPedido_Load(object sender, EventArgs e)
        {
            
            nota();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private  void button3_Click(object sender, EventArgs e)
        {
 




            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            con.liga = new FireSharp.FirebaseClient(con.config);

            con.resonde = con.liga.Set("Pedido/" + label1.Text+"/status",comboBox1.Text);
            Form1 f = new Form1();

            this.Close();

            

            


        }

        private void AddPedido_FormClosed(object sender, FormClosedEventArgs e)
        {

           
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void CarregarImp()
        {
            comboBox2.Items.Clear();
            foreach (var impressoras in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                comboBox2.Items.Add(impressoras);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            using (var pd = new System.Drawing.Printing.PrintDocument())
            {
                pd.PrinterSettings.PrinterName = comboBox2.SelectedItem.ToString();
                pd.PrintPage += Pd_Printpage;
                pd.Print();
            }
        }

        private void Pd_Printpage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            using (var font = new Font("Times New Roman", 14))
            using (var brush = new SolidBrush(Color.Black))
            {
                e.Graphics.DrawString(label3.Text, font, brush, e.MarginBounds);
            }
        }

        public async void nota()
        {
            con.liga = new FireSharp.FirebaseClient(con.config);
            FirebaseResponse resp2 = await con.liga.GetAsync("Pedido/"+label1.Text);
            Pedido obj2 = resp2.ResultAs<Pedido>();



            label3.Text = "Pedido: "+obj2.id+"\n---------------------------------\n"+obj2.Produtos+ "\n---------------------------------\n"+"Endereço: \n"+obj2.endereco+"\nBairro: \n"+obj2.Bairro+ "\n---------------------------------\n"+ obj2.formaPagamento +"\nValor: R$"+obj2.preco.ToString("F");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
