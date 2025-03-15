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
    public partial class Form1 : Form
    {
        
        static conBanco con = new conBanco();
        public DataTable dt = new DataTable();
        public DataTable dt2 = new DataTable();
        DataGridViewButtonColumn ex = new DataGridViewButtonColumn();
        DataGridViewButtonColumn ex2 = new DataGridViewButtonColumn();
        DataGridViewButtonColumn conc = new DataGridViewButtonColumn();
        DataGridViewButtonColumn status = new DataGridViewButtonColumn();
        float PrecoTotal = 0;
        public int NumPedido = 0;
        int NumPro = 0;
        public Form1()
        {



            
            InitializeComponent();
            label6.Hide();
            textBox3.Hide();

            

        }

        public async void Form1_Load(object sender, EventArgs e)
        {

            try {
                con.liga = new FireSharp.FirebaseClient(con.config);
                FirebaseResponse resp7 = await con.liga.GetAsync("Counter/node/cnt");
                NumPro = resp7.ResultAs<int>();
            }
            catch
            {
                MessageBox.Show("Em cima");
            }


            dt.Columns.Add("Numero Pedido");
            dt.Columns.Add("Status");
            dt.Columns.Add("Pedido");
            dt.Columns.Add("Pagamento");
            dt.Columns.Add("Bairro");
            dt.Columns.Add("Endereco");
            dt.Columns.Add("Preco");
            
            dataGridView1.DataSource = dt;
            


            status.HeaderText = "status";
            status.Name = "status";
            status.Text = "Trocar status";
            status.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(status);
            






            ex.HeaderText = "Cancelar";
            ex.Name = "delete";
            ex.Text = "Cancelar";
            ex.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(ex);

            
            conc.HeaderText = "Concluir";
            conc.Name = "conc";
            conc.Text = "Concluir";
            conc.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(conc);



            export();
            combo();


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddPedido a = new AddPedido();
            a.Show();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        public void button1_Click_1Async(object sender, EventArgs e)
        {


            
            




        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        public async void export()
        {

            dt.Rows.Clear();

            con.liga = new FireSharp.FirebaseClient(con.config);
            int i = 0;
            FirebaseResponse res1 = await con.liga.GetAsync("Counter/node");
            Counter_class obj1 = res1.ResultAs<Counter_class>();
            int cnt = Convert.ToInt32(obj1.cnt);
            

            while (true)
            {
                if(i == cnt)
                {
                    
                    
                    break;
                }

                i++;

                try
                {
                    FirebaseResponse resp2 = await con.liga.GetAsync("Pedido/" + i);
                    Pedido obj2 = resp2.ResultAs<Pedido>();



                    if (obj2 != null) {
                        
                        if (obj2.status != "Cancelado" && obj2.status != "Concluído")
                        {
                            
                            if (obj2.formaPagamento == "Cartão")
                            {

                                DataRow row = dt.NewRow();
                                row["Numero Pedido"] = obj2.id;
                                row["Pedido"] = obj2.Produtos;
                                row["Status"] = obj2.status;
                                row["Bairro"] = obj2.Bairro;
                                row["preco"] = (obj2.preco).ToString("F");
                                row["Pagamento"] = obj2.formaPagamento;
                                row["Endereco"] = obj2.endereco;


                                dt.Rows.Add(row);
                            }
                            else
                            {
                                DataRow row = dt.NewRow();
                                row["Numero Pedido"] = obj2.id;
                                row["Pedido"] = obj2.Produtos;
                                row["Status"] = obj2.status;
                                row["Bairro"] = obj2.Bairro;
                                row["preco"] = (obj2.preco).ToString("F");
                                row["Pagamento"] = $"Levar R${float.Parse(obj2.troco).ToString()} de troco";
                                row["Endereco"] = obj2.endereco;


                                dt.Rows.Add(row);
                            }

                            
                           
                        }
                        
                    }
                    else
                    {
                        
                    }

                }
                catch
                {
                    
                }
            }



        }

        public async void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 7)
            {
                NumPedido = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());

                AddPedido add = new AddPedido();
                add.label1.Text = NumPedido.ToString();
                add.comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                add.Show();


            }
            
            

            if (e.ColumnIndex == 8)
            {
                DialogResult result = MessageBox.Show("Deseja mesmo cancelar o pedido "+ this.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), "Confirmar", MessageBoxButtons.YesNo);
                

                if(result == DialogResult.Yes)
                {

                    con.liga = new FireSharp.FirebaseClient(con.config);

                    FirebaseResponse resp9 = await con.liga.GetAsync("HistRelatorio/Mes/" + DateTime.Now.Month + "/PedFeito");
                    float totall = resp9.ResultAs<float>();

                    SetResponse response8 = await con.liga.SetAsync("HistRelatorio/Mes/" + DateTime.Now.Month + "/PedFeito", totall + 1);

                    con.resonde = con.liga.Set("Pedido/" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "/status", "Cancelado");
                    MessageBox.Show("O pedido " + this.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + " foi cancelado");

                    export();
                }
                else
                {

                }
                

            }
            else if (e.ColumnIndex == 9)
            {


                DialogResult result = MessageBox.Show("Deseja mesmo concluir o pedido " + this.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), "Confirmar", MessageBoxButtons.YesNo);

                

                if (result == DialogResult.Yes)
                {
                    con.liga = new FireSharp.FirebaseClient(con.config);

                    con.resonde = con.liga.Set("Pedido/" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "/status", "Concluído");
                    MessageBox.Show("O pedido " + this.dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + " foi concluido e contabilizado");
                    
                    FirebaseResponse resp = await con.liga.GetAsync("Counter/node");
                    Counter_class1 get = resp.ResultAs<Counter_class1>();

                    int concl = Convert.ToInt32(get.pedidoConc) + 1;

                    var obj3 = new Counter_class1
                    {

                        pedidoConc = concl

                    };

                    con.resonde = await con.liga.SetAsync("Counter1/node", obj3);

                    FirebaseResponse resp7 = await con.liga.GetAsync("HistRelatorio/Mes/" + DateTime.Now.Month + "/ValorTotal");
                    float totalll = resp7.ResultAs<float>();
                    float tott = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString());
                    SetResponse response5 = await con.liga.SetAsync("HistRelatorio/Mes/" + DateTime.Now.Month + "/ValorTotal", tott + totalll);

                    FirebaseResponse resp9 = await con.liga.GetAsync("HistRelatorio/Mes/" + DateTime.Now.Month + "/NumVendas");
                    float totall = resp9.ResultAs<float>();
             
                    SetResponse response8 = await con.liga.SetAsync("HistRelatorio/Mes/" + DateTime.Now.Month + "/NumVendas", totall + 1);

                    FirebaseResponse resp11 = await con.liga.GetAsync("HistRelatorio/Mes/" + DateTime.Now.Month + "/PedFeito");
                    float totall11 = resp11.ResultAs<float>();

                    SetResponse response811 = await con.liga.SetAsync("HistRelatorio/Mes/" + DateTime.Now.Month + "/PedFeito", totall + 1);


                    export();
                }
                else
                {

                }

            }
            


        }

        




        private async void button2_Click_1(object sender, EventArgs e)
        {
            con.liga = new FireSharp.FirebaseClient(con.config);
            DialogResult result = MessageBox.Show("Deseja fechar as estatisticas do dia?\nTodos os Pedidos nao concluidos serão CANCELADOS", "Fechar dia", MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Yes)
            {
                this.Enabled = false;
                var obj = new Counter_class
                {
                    cnt = 0,

                };
                

                FirebaseResponse resp = await con.liga.GetAsync("CounterSabor/node/cnt");
                int get = resp.ResultAs<int>();

                progressBar2.Value = 0;
                progressBar2.Maximum = get;
                progressBar2.Visible = true;
                
                for (int i = 1; i <=get; i++)
                {
                    FirebaseResponse tot = await con.liga.GetAsync("Produtos/"+i+ "/QuantPedida");
                    int toto = tot.ResultAs<int>();
                    FirebaseResponse tot1 = await con.liga.GetAsync("HistRelatorio/Mes/" + DateTime.Now.Month + "/TotalPedidos/"+i);
                    int toto1 = tot1.ResultAs<int>();
                    SetResponse response1 = await con.liga.SetAsync("HistRelatorio/Mes/"+DateTime.Now.Month+ "/TotalPedidos/"+i, toto + toto1);

                    SetResponse response2 = await con.liga.SetAsync("Produtos/" + i + "/QuantPedida", "0");
                    progressBar2.Value = i;
                }





                MessageBox.Show("Dia Concluido com Susseço");
                progressBar2.Visible = false;
                SetResponse response6 = await con.liga.SetAsync("Counter/node/cnt", 0);




                this.Close();
            }
            else if(result == DialogResult.No)
            {
                this.Close();
            }
            else
            {

            }

        }

    

        private  void button4_Click(object sender, EventArgs e)
        {
            
        }

        public async void combo()
        {
            comboBox2.Items.Clear();

            con.liga = new FireSharp.FirebaseClient(con.config);
            int i = 0;
            FirebaseResponse res1 = await con.liga.GetAsync("CounterSabor/node");
            Counter_class obj1 = res1.ResultAs<Counter_class>();
            int cnt = Convert.ToInt32(obj1.cnt);
            

            while (true)
            {

                if (i == cnt)
                {
                    break;
                }

                i++;

                try
                {

                    FirebaseResponse resp2 = await con.liga.GetAsync("Produtos/" + i);
                    FirebaseResponse resp3 = await con.liga.GetAsync("Produtos/"+i+"/status");
                    string pup = resp3.ResultAs<string>();
                    Sabores obj2 = resp2.ResultAs<Sabores>();



                    if (obj2 != null)
                    {
                        if (pup != "off") { 
                            comboBox2.Items.Add(obj2.Nome);
                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }

                }
                catch
                {
                }
            }


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.Text != "")
            {
                button4.Enabled = true;

            }
            else
            {
                
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private async void button4_Click_2Async(object sender, EventArgs e)
        {

            

            comboBox2.Enabled = false;
            button4.Enabled = false;
            con.liga = new FireSharp.FirebaseClient(con.config);
            int i = 0;
            FirebaseResponse res1 = await con.liga.GetAsync("CounterSabor/node");
            Counter_class obj1 = res1.ResultAs<Counter_class>();
            int cnt = Convert.ToInt32(obj1.cnt);
            progressBar1.Visible = true;
            progressBar1.Maximum = cnt;

            while (true)
            {

                if (i == cnt)
                {
                    progressBar1.Visible = false;
                    break;
                }

                i++;

                    
                    FirebaseResponse resp2 = await con.liga.GetAsync("Produtos/" + i);
                    Sabores obj2 = resp2.ResultAs<Sabores>();
                    FirebaseResponse quantP = await con.liga.GetAsync("Produtos/" + i + "/QuantPedida");
                    int quant = quantP.ResultAs<int>();
                    progressBar1.Value = i;
                    int p = quant + 1;


                    if (obj2.Nome == comboBox2.Text)
                    {

                        SetResponse response1 = await con.liga.SetAsync("Produtos/" + i + "/QuantPedida", p.ToString());
                        
                        PrecoTotal = obj2.Preco + PrecoTotal;
                        progressBar1.Visible = false;
                        break;

                        



                    }
                    else
                    {

                    }

               
            }






            dataGridView2.Rows.Add(comboBox2.Text);
            comboBox2.Text = "";
            button4.Enabled = true;
            comboBox2.Enabled = true;



        }

        private async void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 1)
            {
                dataGridView2.Enabled = false;
                

                con.liga = new FireSharp.FirebaseClient(con.config);
                int i = 0;
                FirebaseResponse res1 = await con.liga.GetAsync("CounterSabor/node");
                Counter_class obj1 = res1.ResultAs<Counter_class>();
                int cnt = Convert.ToInt32(obj1.cnt);


                while (true)
                {
                    dataGridView2.Rows[e.RowIndex].Cells[1].Value = "carregando";
                    dataGridView2.Rows[e.RowIndex].Cells[1].ReadOnly = true;

                    if (i == cnt)
                    {
                        
                        break;
                    }

                    i++;

                    try
                    {

                        FirebaseResponse resp2 = await con.liga.GetAsync("Produtos/" + i);
                        Sabores obj2 = resp2.ResultAs<Sabores>();
                        FirebaseResponse quantP = await con.liga.GetAsync("Produtos/" + i + "/QuantPedida");
                        int quant = quantP.ResultAs<int>();


                        if (obj2.Nome == dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString())
                        {
                            SetResponse response1 = await con.liga.SetAsync("Produtos/" + i + "/QuantPedida", (quant - 1).ToString());
                            PrecoTotal = PrecoTotal - obj2.Preco;


                        }
                        else
                        {

                        }

                    }
                    catch
                    {

                    }
                }
                dataGridView2.Rows.RemoveAt(e.RowIndex);
                dataGridView2.Enabled = true;

            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
             
            string pedido1 = "";
            string pedido = "";

            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                pedido = dataGridView2.Rows[i].Cells[0].Value.ToString()+"\n";
                pedido1 = pedido + pedido1;

            }

            if (pedido1 == "" || textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Por favor preencha todos os campos");
                textBox2.Text = textBox3.Text;
            }
            else
            {


                if(comboBox1.Text == "Cartão") { 
                    con.liga = new FireSharp.FirebaseClient(con.config);

                    FirebaseResponse resp = await con.liga.GetAsync("Counter/node");
                    Counter_class get = resp.ResultAs<Counter_class>();



                    var a = new Pedido
                    {
                        id = (Convert.ToInt32(get.cnt) + 1),
                        status = "Em espera",
                        Produtos = pedido1,
                        endereco = textBox1.Text,
                        Bairro = textBox2.Text,
                        preco = PrecoTotal,
                        formaPagamento = "Cartão",
                        
                        

                    };


                    con.resonde = con.liga.Set("Pedido/" + a.id, a);

                    var obj = new Counter_class
                    {
                        cnt = a.id,

                    };

                    SetResponse response1 = await con.liga.SetAsync("Counter/node", obj);
                    PrecoTotal = 0;
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    dataGridView2.Rows.Clear();
                }
                else
                {
                    con.liga = new FireSharp.FirebaseClient(con.config);

                    FirebaseResponse resp = await con.liga.GetAsync("Counter/node");
                    Counter_class get = resp.ResultAs<Counter_class>();
                    float c = 0;
                    try 
                    {
                        c = float.Parse(textBox3.Text);
                        var a = new Pedido
                        {
                            id = (Convert.ToInt32(get.cnt) + 1),
                            Produtos = pedido1,
                            status = "Em espera",
                            endereco = textBox1.Text,
                            Bairro = textBox2.Text,
                            preco = PrecoTotal,
                            formaPagamento = "Dinheiro",
                            troco = (c - PrecoTotal).ToString("F"),
                        };


                        con.resonde = con.liga.Set("Pedido/" + a.id, a);

                        var obj = new Counter_class
                        {
                            cnt = a.id,

                        };

                        SetResponse response1 = await con.liga.SetAsync("Counter/node", obj);

                        FirebaseResponse resp7 = await con.liga.GetAsync("HistRelatorio/Mes/" + DateTime.Now.Month + "/ValorTotal");
                        float totalll = resp7.ResultAs<float>();
                        SetResponse response5 = await con.liga.SetAsync("HistRelatorio/Mes/" + DateTime.Now.Month + "ValorTotal", PrecoTotal + totalll);
                        PrecoTotal = 0;

                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Visible = false;
                        label6.Visible = false;
                        textBox3.Clear();
                        comboBox1.Text = "";
                        dataGridView2.Rows.Clear();
                    }
                    catch
                    {
                        MessageBox.Show("Por favor coloque apenas numeros");
                    }


                    



                    


                }
            }




            
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            label6.Hide();
            textBox3.Hide();
            if (comboBox1.Text == "Dinheiro")
            {
                label6.Show();
                textBox3.Show();
            }
            else
            {
                label6.Hide();
                textBox3.Hide();
            }
        }



        private void Form1_Activated(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            StatusProduto st = new StatusProduto();

            st.Show();


        }

        private void button6_Click(object sender, EventArgs e)
        {
            Relatorio chama = new Relatorio();
            chama.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            export();
            combo();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Att();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

            

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if(e.ColumnIndex == 1)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() == "Em produção")
                {
                    for(int i = 0; i < 10; i++)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[i].Style.BackColor = Color.Yellow;
                    }

                }
                else if (dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() == "Em rota de entrega")
                {
                    for (int i = 0; i < 10; i++)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[i].Style.BackColor = Color.Green;
                    }
                }
                else
                {
                    for (int i = 0; i < 10; i++)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[i].Style.BackColor = Color.Red;
                    }
                }
            }
            

        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
        }
        public async void Att()
        {
            con.liga = new FireSharp.FirebaseClient(con.config);
            FirebaseResponse resp = await con.liga.GetAsync("Counter/node/cnt");
            int att = resp.ResultAs<int>();
            if(att == NumPro)
            {

            }
            else
            {
                NumPro = att;
                export();

            }

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            home cham = new home();
            cham.Show();
        }
    }
}
