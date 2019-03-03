using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Data;
using log4net;
using LumenWorks.Framework.IO.Csv;

namespace Convert2OfxConsole
{
    /// <summary>
    /// 
    /// Format d'échange OFX : http://www.ofx.net/
    /// 
    /// Log4Net best practice : https://stackify.com/log4net-guide-dotnet-logging/
    /// 
    /// </summary>
    class Program
    {
        //Declare an instance for log4net
        private static readonly ILog Log =
              LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            
            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string dirtemp = dir + @"\Temp\";
            string dirlog = dir + @"\Logs\";

            DirectoryInfo d = new DirectoryInfo(dirlog);//Assuming Test is your Folder

            if (!d.Exists)
            {
                d.Create();
                Console.WriteLine("Répertoire log créé : " + d.ToString());
            }

            Log.Info("Start program...");
            
            d = new DirectoryInfo(dirtemp);//Assuming Test is your Folder

            if (!d.Exists)
            {
                d.Create();
                Console.WriteLine("Répertoire temp créé : " + d.ToString());
            }

            FileInfo[] Files = d.GetFiles("*.csv"); //Getting CSV files

            Console.WriteLine("Répertoire de travail : " + d.ToString());

            if (Files.Count() == 0)
            {
                Log.Error("Aucun fichier à traiter !");
            }
            else
            {
                string str = "";

                int i = 0;

                foreach (FileInfo file in Files)
                {
                    i++;

                    str = file.Name;
                    Console.WriteLine("fichier traité : " + str);

                    DataTable dt = GetTable();
                    dt = FetchFromCSVFile(dirtemp + str);

                    string fileOutput = string.Format(i + "-export-operations-{0}-{1}-{2}_{3}-{4}-{5}",
                                                DateTime.Now.Day,
                                                (DateTime.Now.Month).ToString(),
                                                (DateTime.Now.Year).ToString(),
                                                (DateTime.Now.Hour).ToString(),
                                                (DateTime.Now.Minute).ToString(),
                                                (DateTime.Now.Second).ToString()
                                                );

                    exportToQifFile(dirtemp + fileOutput + ".qif", dt);

                    File.Move(dirtemp + file.Name, dirtemp + i + "_traité_" + file.Name);
                }
            }

            

            //exportToOfxFile(dir + @"\Temp\" + fileOutput + ".ofx", dt);
            Log.Info("End program...");
        }

        private static DataTable GetTable()
        {
            DataTable table = new DataTable(); // New data table.
            table.Columns.Add("DateOp", typeof(DateTime)); // Add five columns.
            table.Columns.Add("DateVal", typeof(DateTime));
            table.Columns.Add("Label", typeof(string));
            table.Columns.Add("Category", typeof(string));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("AccounNum", typeof(int));
            table.Columns.Add("AccountLabel", typeof(string));
            table.Columns.Add("AccountBalance", typeof(decimal));

            return table; // Return reference.
        }

        private static DataTable FetchFromCSVFile(string filePath)
        {
            DataTable csvTable = new DataTable();
            using (CsvReader csvReader =
                new CsvReader(new StreamReader(filePath), true, ';'))
            {
                csvTable.Load(csvReader);
            }
            return csvTable;
        }

        private static void exportToQifFile(string fileOut, DataTable dt)
        {
            var utf8WithoutBom = new System.Text.UTF8Encoding(false);

            // Creates the OFX file as a stream, using the given encoding.
            StreamWriter sw = new StreamWriter(fileOut, false, utf8WithoutBom);

            sw.WriteLine("!Type:Bank");

            foreach (DataRow row in dt.Rows) // Loop over the rows.
            {
                string dateOp = row[0].ToString();

                string amount = string.Empty;

                amount = row[6].ToString().Replace(",", ".").Replace(" ", ",");

                if (!amount.StartsWith("-"))
                {
                    amount = "+" + amount;
                }


                string label = row[5].ToString();

                if (label.StartsWith("PAIEMENT CARTE"))
                {
                    label.Substring(10, label.Length - 26);
                }

                sw.WriteLine("D" + dateOp);
                sw.WriteLine("T" + amount);
                sw.WriteLine("PVIR " + label);
                sw.WriteLine("^");
            }

            // Closes the text stream.
            sw.Close();
        }


        private static void exportToOfxFile(string fileOut, DataTable dt)
        {
            // Creates the OFX file as a stream, using the given encoding.
            StreamWriter sw = new StreamWriter(fileOut, false, Encoding.UTF8);

            sw.WriteLine("OFXHEADER: 100");
            sw.WriteLine("DATA: OFXSGML");
            sw.WriteLine("VERSION:102");
            sw.WriteLine("SECURITY: NONE");
            sw.WriteLine("ENCODING:USASCII");
            sw.WriteLine("CHARSET:1252");
            sw.WriteLine("COMPRESSION: NONE");
            sw.WriteLine("OLDFILEUID:NONE");
            sw.WriteLine("NEWFILEUID:");
            sw.WriteLine("");

            sw.WriteLine("<OFX>");

            sw.WriteLine("<BANKTRANLIST>");

            foreach (DataRow row in dt.Rows) // Loop over the rows.
            {
                string dateOp = row["DateOp"].ToString();

                sw.WriteLine("" + dateOp);

                foreach (var item in row.ItemArray) // Loop over the items.
                {
                    sw.WriteLine(item);
                }
            }

            sw.WriteLine("</BANKTRANLIST>");

            sw.WriteLine("</OFX>");

            // Closes the text stream and the database connenction.
            sw.Close();

        }


    }
}
