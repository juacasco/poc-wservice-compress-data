using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.IO.Compression;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WService_ConceptTest_Host
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ExecQuery" in both code and config file together.
    public class ExecQuery : IExecQuery
    {
        public byte[] answerTest(string querySelectCmd, out long notZipLength, out long zipLength, out List<DateTime> timeCaptures, out List<String> strTimeCaptures)
        {
            byte[] result;
            timeCaptures = new List<DateTime>();
            strTimeCaptures = new List<String>();
            timeCaptures.Add(DateTime.Now);//TS - Process start
            strTimeCaptures.Add("Process start");
            //setting up connection
            string conStr = "Data Source=bmmopsb;User ID=sd_sdu;Password=Operation&2015;Initial Catalog=GENERAL_PURPOSE";
            SqlConnection sqlConn = new SqlConnection(conStr);
            timeCaptures.Add(DateTime.Now);//TS - Creating connection
            strTimeCaptures.Add("Connection created");
            DataTable dataT = new DataTable();
            sqlConn.Open();
            timeCaptures.Add(DateTime.Now);//TS - Opening query
            strTimeCaptures.Add("Opening query");
            SqlDataAdapter dataAd = new SqlDataAdapter(querySelectCmd, sqlConn);
            timeCaptures.Add(DateTime.Now);//TS - Query executed
            strTimeCaptures.Add("Query executed");
            //returning datatable from command
            dataAd.Fill(dataT);
            timeCaptures.Add(DateTime.Now);//TS - Filling datatable with information
            strTimeCaptures.Add("Data table filled");
            //Create memoryStream
            using (MemoryStream memStr = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(memStr, dataT);
                timeCaptures.Add(DateTime.Now);//TS - Serializing
                strTimeCaptures.Add("Datatable serialized");
                notZipLength = memStr.Length;
                MemoryStream zipMemStr = new MemoryStream();
                using (GZipStream GZvarStr = new GZipStream(zipMemStr, CompressionMode.Compress))
                {
                    memStr.Position = 0;
                    memStr.CopyTo(GZvarStr);
                }
                timeCaptures.Add(DateTime.Now);//TS - Compressing
                strTimeCaptures.Add("Memory stream compressed");
                result = zipMemStr.ToArray();
                zipLength = result.Length;
                timeCaptures.Add(DateTime.Now); //TS - Returning
                strTimeCaptures.Add("Returning values");


            }
            //GZipStream zipStreamOut = new GZipStream(fsOut)

            //memStr.Close();
            //zipStream.Close();
            return result;
        }
    }
}
