using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
namespace WService_ConceptTest_Client
{
    public partial class Form1 : Form
    {
        ServiceReference1.ExecQueryClient queryClient;
        public Form1()
        {
            InitializeComponent();
            queryClient = new ServiceReference1.ExecQueryClient();
            listBox1.Items.Add("Connection Openning - " + DateTime.Now.ToString("hh:mm:ss.fff"));
            queryClient.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add("<<<<<<Process compressed query>>>>>>");
            Cursor = Cursors.WaitCursor;
            DateTime startTime = DateTime.Now;
            listBox1.Items.Add("Client start - " + DateTime.Now.ToString());
            
            //ServiceQueryZip.QueriableClient QueryClient = new ServiceQueryZip.QueriableClient();

            //ServiceReference1.ExecQueryClient clientQueryDB = new ServiceReference1.ExecQueryClient();
            listBox1.Items.Add("Start server method - " + DateTime.Now.ToString());
            long notZipLength, zipLength;
            DateTime[] timeCaptures;
            String[] strTimeCaptures;
            int counter = 0;
            byte[] answer = queryClient.answerTest(textBox1.Text, out notZipLength, out zipLength, out timeCaptures, out strTimeCaptures);
            listBox1.Items.Add("Server process completed - " + DateTime.Now.ToString());
            // MessageBox.Show("not ziped length: " + notZipLength.ToString("n") + "\n" +
            //     "Zipped length: " + zipLength.ToString("n"));
            listBox1.Items.Add("-----------------------------");
            foreach (DateTime item in timeCaptures)
            {
                listBox1.Items.Add(strTimeCaptures[counter] + " - " + item.ToString("hh:mm:ss.fff"));
                counter = counter + 1;
            }
            //decompressing
            listBox1.Items.Add("-----------------------------");
            GZipStream zipDecompress = new GZipStream(new MemoryStream(answer), CompressionMode.Decompress);
            MemoryStream ms = new MemoryStream();
            zipDecompress.CopyTo(ms);
            listBox1.Items.Add("Stream decompresed - " + DateTime.Now.ToString());
            //deserializing
            BinaryFormatter bf = new BinaryFormatter();
            
            ms.Position = 0;
            //DataTable dataTResult = (DataTable)bf.Deserialize(ms);
            //var dataTResult = (DataTable)bf.Deserialize(ms);
            object objTest = bf.Deserialize(ms);
            var dataTResult = (DataTable)objTest;
            listBox1.Items.Add("Stream deserialized - " + DateTime.Now.ToString());
            dataGridView1.DataSource = dataTResult;
            listBox1.Items.Add("DataTable assigned - " + DateTime.Now.ToString());
            TimeSpan ts = DateTime.Now - startTime;
            label2.Text = "Compressed query results: " + ((decimal)notZipLength / (decimal)1048576).ToString("n")
                + " MB, compressed size: " + ((decimal)zipLength / (decimal)1048576).ToString("n") + " Start time: "
                + startTime.ToString() + " End time: " + DateTime.Now.ToString() + " - Elapsed time: " +
                ts.TotalSeconds.ToString() + " Seconds";
            Cursor = Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            DateTime startTime = DateTime.Now;
            listBox1.Items.Add("<<<<<<Regular query execution>>>>>>");
            string conStr = "Data Source=gvs21655,2048;User ID=sd_sdu;Password=Operation&2015;Initial Catalog=GENERAL_PURPOSE";
            //string conStr = "Data Source=localhost;Trusted_Connection=True;Initial Catalog=AdventureWorks2012";
            string querySelectCmd = textBox1.Text;
            SqlConnection sqlConn = new SqlConnection(conStr);
            listBox1.Items.Add("Connection created - " + DateTime.Now.ToString("hh:mm:ss.fff"));
            DataTable dataT = new DataTable();
            sqlConn.Open();
            listBox1.Items.Add("Connection Open - " + DateTime.Now.ToString("hh:mm:ss.fff"));
            SqlDataAdapter dataAd = new SqlDataAdapter(querySelectCmd, sqlConn);
            dataAd.Fill(dataT);
            listBox1.Items.Add("Query executed - " + DateTime.Now.ToString("hh:mm:ss.fff"));
            dataGridView1.DataSource = dataT;
            listBox1.Items.Add("Data table filled - " + DateTime.Now.ToString("hh:mm:ss.fff"));
            sqlConn.Close();
            TimeSpan ts = DateTime.Now - startTime;
            label3.Text = "Regular query exec method: Start time: " + startTime.ToString() + " End time: " + DateTime.Now.ToString() + " - Elapsed time: " +
                ts.TotalSeconds.ToString() + " Seconds";
            Cursor = Cursors.Default;
        }

        private void button3_Click(object sender, EventArgs e) //simple connection to get server time by adding a client value
        {
            Cursor = Cursors.WaitCursor;
            listBox1.Items.Add("<<<<<<Simple server connection to get date>>>>>>");
            listBox1.Items.Add("Query start - " + DateTime.Now.ToString("hh:mm:ss.fff"));
            ServiceReference1.ExecQueryClient queryClient1 = new ServiceReference1.ExecQueryClient();
            DateTime serverDateTime = queryClient1.GetServerDate(96);
            listBox1.Items.Add("Query end - " + DateTime.Now.ToString("hh:mm:ss.fff"));
            Cursor = Cursors.Default;
            MessageBox.Show(serverDateTime.ToString());
        }

        private void WCF_POC_WebTCP_Click(object sender, EventArgs e)
        {
            MessageBox.Show(queryClient.GetData(8));
        }
    }
}
