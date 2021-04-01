using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DepoResult
{
    public partial class Form1 : Form
    {
        //// с byte в string
        ////var line2 = Con.Fas_Depo_Test_Result.Where(c => c.SN == 2).Select(c => c.ResultData).FirstOrDefault();
        // var line3 = System.Text.Encoding.UTF8.GetString(line2);

        public Form1()
        {
            InitializeComponent();
            //Progress(@"\\192.168.180.2\logs\Failed\DEPO Computers");
            //Progress(@"\\192.168.180.2\logs\Passed\DEPO Computers");
        }

        bool Get;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Get)
            { Progress(@"\\192.168.180.2\logs\Failed\DEPO Computers");
                Progress(@"\\192.168.180.2\logs\Passed\DEPO Computers");
            }



        }


        private void button1_Click(object sender, EventArgs e)
        {
            //if (TB1.Text == "" || TB2.Text == "")
            //    return;

            //var list = Loads(Convert.ToInt32(TB1.Text), Convert.ToInt32(TB2.Text));         
            var list = Loads();



            if (list.Count == 0)
            { MessageBox.Show("Диапозон номеров не найден"); return;     }

          folderBrowserDialog1.ShowDialog();
          var path = folderBrowserDialog1.SelectedPath;

            for (int i = 0; i < list.Count; i++)
            {
                string Path = path + @"\" + list[i].ResultFileName;

                var fs = File.Create(Path);
                fs.Write(list[i].ResultData, 0, list[i].ResultData.Length);
                fs.Close();

            }

         

        }

        List<Fas_Depo_Test_Result> Loads(int start, int end)
        {
            using (FASEntities Con = new FASEntities())
            {
                return (from i in Con.Fas_Depo_Test_Result
                        where (i.SN <= start && i.SN >= end)
                        select i).ToList();
            }
        }

        List<Fas_Depo_Test_Result> Loads()
        {
            using (FASEntities Con = new FASEntities())
            {
                var inq = new int[] {  41849,
42891,
43575,
43575,
43575,
43575,
44114,
42983,
42335,
47192,
19438,
42928,
42369,
44972,
36102,
44165,
44891,
03702,
03686,
03685
 };
                return (from i in Con.Fas_Depo_Test_Result
                        where ( inq.Contains(i.SN)  )
                        select i).ToList();
            }
        }



        void Progress(string path)
        {

            //Список полного имени с путем
            var _path = path;
            var ListFullPath = Directory.GetFiles(_path);

            List <string> list = new List<string>();

           
            foreach (var item in Directory.GetFiles(_path))
            {
                try
                {  
                    var ResultFileName = item.Substring((item.LastIndexOf('\\') + 1));                    
                    var SN =  ResultFileName.Substring(6, 6);
                    var SN2 = Convert.ToInt32(SN,16);
               

                    using (var str = new StreamReader(item))
                    {
                        var Byte = System.Text.Encoding.UTF8.GetBytes(str.ReadToEnd());
                        switch (Check(SN2))
                        {
                            case true:
                                Update(SN2, ResultFileName, Byte);
                                break;
                            case false:
                                Add(SN2, ResultFileName, Byte);
                                break;
                        }
                        File.Delete(item);
                    }
                }
                catch (Exception)
                {

                    continue;
                }
            }
            #region
            //var Path = e.FullPath;
            //var Path = ListPath.Select(c=>c.sub)
            //Список полного имени 
            //var FullName = e.Name;
            //SN
            //var ListSN = Convert.ToInt32(e.Name.Substring(6, 6));

            //var Byte = System.Text.Encoding.UTF8.GetBytes(new StreamReader(Path).ReadToEnd());



            //for (int i = 0; i < ListPath.Count(); i++)
            //{
            //    using (var str = new StreamReader(ListPath[i]))
            //    {
            //        var Byte = System.Text.Encoding.UTF8.GetBytes(str.ReadToEnd());

            //        switch (Check(ListSN[i]))
            //        {
            //            case true:
            //                Update(ListSN[i], ListFullName[i], Byte);
            //                break;
            //            case false:
            //                Add(ListSN[i], ListFullName[i], Byte);
            //                break;
            //        }
            //    }
            //    File.Delete(ListPath[i]);


            //}
            #endregion
        }

        bool Check(int SN)
        {
            using (FASEntities Con = new FASEntities())
            {              
                return Con.Fas_Depo_Test_Result.Where(c => c.SN == SN).Select(c => c.SN == SN).FirstOrDefault();
            }
        }

            void Add(int SN, string FullName, byte[] Byte)
            {
                using (FASEntities Con = new FASEntities())
                {
                    var depo = new Fas_Depo_Test_Result()
                    {
                        SN = SN,
                        ResultFileName = FullName,
                        ResultData = Byte,
                        RegDate = DateTime.Now,
                        Count = "1"
                    };
                    Con.Fas_Depo_Test_Result.Add(depo);
                    Con.SaveChanges();
                }
            }

            void Update(int SN, string FullName, byte[] Byte)
        {
            using (FASEntities Con = new FASEntities())
            {
                var _count = Con.Fas_Depo_Test_Result.Where(c => c.SN == SN).Select(c => c.Count).FirstOrDefault();
                var A = Con.Fas_Depo_Test_Result.Where(c => c.SN == SN);
                A.FirstOrDefault().ResultFileName = FullName;
                A.FirstOrDefault().ResultData = Byte;
                A.FirstOrDefault().RegDate = DateTime.Now;
                A.FirstOrDefault().Count = (Convert.ToInt32(_count) + 1).ToString();
                Con.SaveChanges();
            }
        }
                   
       
        private void button2_Click(object sender, EventArgs e)
        {
            Progress(@"\\192.168.180.2\logs\Failed\DEPO Computers");
            Progress(@"\\192.168.180.2\logs\Passed\DEPO Computers");
            //label3.Text = "Цикл Запущен";
            //label3.BackColor = Color.Green;           
            //Get = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            label3.Text = "Цикл Запущен";
            label3.BackColor = Color.Red;                   
            Get = false;
        
        }

       

        private void TB1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if ((e.KeyChar <= 48 || e.KeyChar >= 59) && e.KeyChar != 8)
            //    e.Handled = true;
        }

        private void TB2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if ((e.KeyChar <= 48 || e.KeyChar >= 59) && e.KeyChar != 8)
            //    e.Handled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
