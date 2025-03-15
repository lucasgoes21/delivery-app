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
    public partial class StatusProduto : Form
    {
        static conBanco con = new conBanco();
        public DataTable dt = new DataTable();
        DataGridViewCheckBoxColumn checkon = new DataGridViewCheckBoxColumn();
        DataGridViewCheckBoxColumn checkoff = new DataGridViewCheckBoxColumn();
        public StatusProduto()
        {
            InitializeComponent();

            /*dt.Columns.Add("Produto");
            dataGridView1.DataSource = dt;
            

            checkon.HeaderText = "Ativo";
            checkon.Name = "Ativo";
            dataGridView1.Columns.Add(checkon);

            checkoff.HeaderText = "Desativo";
            checkoff.Name = "Desativo";
            dataGridView1.Columns.Add(checkoff);

            dataGridView1.AutoResizeColumns();*/

            
            export();
            
        }

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 2)
            {
                EditarProduto editar = new EditarProduto();
                editar.label5.Text = (e.RowIndex + 1).ToString();
                editar.Show();
            }



        }
        public async void export()
        {
            button1.Enabled = false;
            dataGridView1.Rows.Clear();

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
                    stat obj2 = resp2.ResultAs<stat>();



                    if (obj2 != null)
                    {

                        dataGridView1.Rows.Add(obj2.Nome);
                        dataGridView1.Rows[i - 1].Cells[0].Value = obj2.Nome;
                        if (obj2.status == "on")
                        {
                            dataGridView1.Rows[i - 1].Cells[1].Value = "true";
                        }
                        else
                        {
                            dataGridView1.Rows[i - 1].Cells[1].Value = "false";
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
            button1.Enabled = true;


        }

        private async void button1_Click(object sender, EventArgs e)
        {

            con.liga = new FireSharp.FirebaseClient(con.config);
            FirebaseResponse res1 = await con.liga.GetAsync("CounterSabor/node");
            Counter_class obj1 = res1.ResultAs<Counter_class>();
            int cnt = Convert.ToInt32(obj1.cnt);
            progressBar1.Visible = true;
            progressBar1.Maximum = cnt;
            for (int i = 1; i <= cnt; i++)
            {



                if(dataGridView1.Rows[i-1].Cells[1].Value.ToString() == "true" || dataGridView1.Rows[i - 1].Cells[1].Value.ToString() == "True")
                {

                    con.resonde = con.liga.Set("Produtos/" + i + "/status", "on");
                    progressBar1.Value = i;
                }
                else{

                    con.resonde = con.liga.Set("Produtos/" + i + "/status", "off");
                    progressBar1.Value = i;
                }


            }
            MessageBox.Show("Todos os Produtos Foram Atualizados");
            progressBar1.Visible = false;
            progressBar1.Value = 0;
            this.Close();



        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AdcionarProduto open = new AdcionarProduto();
            open.Show();



        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            export();
        }

        private void StatusProduto_Load(object sender, EventArgs e)
        {

        }
    }
}
