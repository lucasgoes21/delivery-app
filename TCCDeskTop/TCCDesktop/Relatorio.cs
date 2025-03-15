using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace TCCDesktop
{
    public partial class Relatorio : Form
    {

        static conBanco con = new conBanco();
        public Relatorio()
        {
            con.liga = new FireSharp.FirebaseClient(con.config);
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Relatorio_Load(object sender, EventArgs e)
        {
            CarregarImp();
            comboBox1.Text = DateTime.Now.Month.ToString();
            label1.Text = "";


        }
        public async void relatorio()
        {
            this.Enabled = false;
            int maior = 0;
            int menos = 999999999;
            string nomeMa = "";
            string nomeMe = "";


            label1.Text = "Produtos Vendidos\n-------------------------------------------------------\n";
            
            FirebaseResponse res1 = await con.liga.GetAsync("CounterSabor/node/cnt");
            int a = res1.ResultAs<int>();
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            label5.Visible = true;
            progressBar1.Maximum = a;


            for (int i = 1; i <= a; i++)
            {

                FirebaseResponse res2 = await con.liga.GetAsync("Produtos/" + i + "/Nome");
                string nome = res2.ResultAs<string>();
                FirebaseResponse res3 = await con.liga.GetAsync("HistRelatorio/Mes/" + comboBox1.Text + "/TotalPedidos/" + i);
                int numero = res3.ResultAs<int>();
                if (numero >= maior)
                {
                    if (numero == maior)
                    {
                        nomeMa = nomeMa + "\n" + nome;
                    }
                    else
                    {
                        maior = numero;
                        nomeMa = "\n" + nome;
                    }
                   
                }
                if(numero <= menos)
                {
                    if (numero == menos)
                    {
                        nomeMe = nomeMe + "\n" + nome;
                    }
                    else
                    {
                        menos = numero;
                        nomeMe = "\n" + nome;
                    }

                    
                }
                label1.Text = label1.Text + nome + ": "+numero+"\n";
                progressBar1.Value = i;



            }
            label1.Text = label1.Text + "-------------------------------------------------------\n";
            FirebaseResponse res4 = await con.liga.GetAsync("HistRelatorio/Mes/"+comboBox1.Text);
            RelaClass rel = res4.ResultAs<RelaClass>();
            label1.Text = label1.Text +"Numero de Pedidos Realizados: "+ rel.PedFeito + "\n" + "Numero de Pedidos Concluidos: " + rel.NumVendas + "\n-------------------------------------------------------\n" + "Produto mais pedido: \n" + nomeMa+ "\n-------------------------------------------------------\n" + "Produto menos pedido: \n" + nomeMe + "\n-------------------------------------------------------\n" + "Valor Gasto: R$" + rel.ValorGasto.ToString("F") + "\n" + "Valor Ganho: R$" + rel.ValorTotal.ToString("F") + "\n" + "Lucro: R$" + rel.Lucro.ToString("F");
            label5.Visible = false;
            progressBar1.Visible = false;
            this.Enabled = true;

        }
        public async void AttRelatorio()
        {
            this.Enabled = false;
            FirebaseResponse resp = await con.liga.GetAsync("CounterSabor/node/cnt");
            int get = resp.ResultAs<int>();
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            label5.Visible = true;
            progressBar1.Maximum = get;

            for (int i = 1; i <= get; i++)
            {
                FirebaseResponse tot = await con.liga.GetAsync("Produtos/" + i + "/QuantPedida");
                int toto = tot.ResultAs<int>();
                FirebaseResponse tot1 = await con.liga.GetAsync("HistRelatorio/Mes/" + DateTime.Now.Month + "/TotalPedidos/" + i);
                int toto1 = tot1.ResultAs<int>();
                SetResponse response1 = await con.liga.SetAsync("HistRelatorio/Mes/" + DateTime.Now.Month + "/TotalPedidos/" + i, toto + toto1);

                SetResponse response2 = await con.liga.SetAsync("Produtos/" + i + "/QuantPedida", "0");
                progressBar1.Value = i;

            }
            relatorio();

            MessageBox.Show("Atualizado");
            label5.Visible = false;
            progressBar1.Visible = false;
            this.Enabled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            relatorio();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AttRelatorio();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            label3.Visible = true;
            button3.Visible = false;
            button4.Visible = true;

        }

        public async void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                try { 
                    float a = float.Parse(textBox1.Text);
                    FirebaseResponse ValT = await con.liga.GetAsync("HistRelatorio/Mes/" + DateTime.Now.Month + "/ValorTotal");
                    float b = ValT.ResultAs<float>();
                    float h = b - a;


                    SetResponse response1 = await con.liga.SetAsync("HistRelatorio/Mes/" + DateTime.Now.Month + "/ValorGasto", (a));
                    SetResponse response8 = await con.liga.SetAsync("HistRelatorio/Mes/" + DateTime.Now.Month + "/Lucro", (h));
                    MessageBox.Show("Gastos Adicionados com sucesso");
                }
                catch
                {
                    MessageBox.Show("Por favor insira apenas numeros");
                }
                

            }
            else{
                MessageBox.Show("Por favor insira todas as informaçoes");
            }

            

            textBox1.Visible = false;
            label3.Visible = false;
            button3.Visible = true;
            button4.Visible = false;


        }
        private void CarregarImp()
        {
            comboBox2.Items.Clear();
            foreach (var impressoras in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                comboBox2.Items.Add(impressoras);
            }
        }

        private void button5_Click(object sender, EventArgs e)
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
                e.Graphics.DrawString(label1.Text, font, brush, e.MarginBounds);
            }
        }
    }
}
