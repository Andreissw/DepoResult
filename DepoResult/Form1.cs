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
        ////var line2 = Con.Depo_Result.Where(c => c.SN == 2).Select(c => c.ResultData).FirstOrDefault();
        // var line3 = System.Text.Encoding.UTF8.GetString(line2);

        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (TB1.Text == "" || TB2.Text == "")
                return;

           var list =  Load(Convert.ToInt32(TB1.Text), Convert.ToInt32(TB2.Text));
            if (list.Count == 0)
            { MessageBox.Show("Диапозон номеров не найден"); return;     }

          folderBrowserDialog1.ShowDialog();
          var path = folderBrowserDialog1.SelectedPath;

            for (int i = 0; i < list.Count; i++)
            {
                string Path = path + @"\" + list[i].ResultFileName;

                var fs = File.Create(Path);
                fs.Write(list[i].ResultData,0,list[i].ResultData.Length);
                fs.Close();

            }

        }

        List<Depo_Result> Load(int start, int end)
        {
            using (RealBaseEntities Con = new RealBaseEntities())
            {
                return (from i in Con.Depo_Result
                        where (i.SN <= start && i.SN >= end)
                        select i).ToList();
            }
        }
        async void ASProgress()
        {
            await Task.Run(() => Progress());
        }

         void Progress()
        {
            Invoke((Action)(() =>       {    PB1.Value = 0;       }));
            string _path = @"\\192.168.180.2\logs\Passed\Test";

            //Список полного имени с путем
            var ListPath = Directory.GetFiles(_path);
            //Список полного имени 
            var ListFullName = ListPath.Select(c => c.Substring(_path.Length + 1, c.Length - (_path.Length + 1))).ToList();
            //SN
            var ListSN = ListFullName.Select(c => Convert.ToInt32(c.Substring(6, 6), 16)).ToList();
            if (ListPath.Count() == 0)
                return;

            Invoke((Action)(() =>  {   PB1.Maximum = ListPath.Count() - 1;  }));

            for (int i = 0; i < ListPath.Count(); i++)
            {
                using (var str = new StreamReader(ListPath[i]))
                {
                    var Byte = System.Text.Encoding.UTF8.GetBytes(str.ReadToEnd());
                    switch (Check(ListSN[i]))
                    {
                        case true:
                            Update(ListSN[i], ListFullName[i], Byte);
                            break;
                        case false:
                            Add(ListSN[i], ListFullName[i], Byte);
                            break;
                    }
                }
                File.Delete(ListPath[i]);

                Invoke((Action)(() =>  { PB1.Value = i;  }));                   
            }
        }

        bool Check(int SN)
        {
            using (RealBaseEntities Con = new RealBaseEntities())
            {              
                return Con.Depo_Result.Where(c => c.SN == SN).Select(c => c.SN == SN).FirstOrDefault();
            }
        }

        void Add(int SN, string FullName, byte[] Byte)
        {
            using (RealBaseEntities Con = new RealBaseEntities())
            {
                var depo = new Depo_Result()
                {
                    SN = SN,
                    ResultFileName = FullName,
                    ResultData = Byte,
                    RegDate = DateTime.Now,
                    Count = "1"
                    
                };

                Con.Depo_Result.Add(depo);
                Con.SaveChanges();
            }
        }

        void Update(int SN, string FullName, byte[] Byte)
        {
            using (RealBaseEntities Con = new RealBaseEntities())
            {
                var _count = Con.Depo_Result.Where(c => c.SN == SN).Select(c => c.Count).FirstOrDefault();
                var A = Con.Depo_Result.Where(c => c.SN == SN);
                A.FirstOrDefault().ResultFileName = FullName;
                A.FirstOrDefault().ResultData = Byte;
                A.FirstOrDefault().RegDate = DateTime.Now;
                A.FirstOrDefault().Count = (Convert.ToInt32(_count) + 1).ToString();

                Con.SaveChanges();

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ASProgress();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (sec.ToString("HH:mm:ss") == "00:00:00")     
                sec = Convert.ToDateTime("00:00:10");

            sec =  sec.AddSeconds(-1);
            label1.Text = sec.ToString("HH:mm:ss");
        }

        DateTime sec = new DateTime();
        private void button2_Click(object sender, EventArgs e)
        {
            label3.Text = "Таймер Работает!";
            label3.BackColor = Color.Green;
            sec = Convert.ToDateTime("00:00:10");
            timer1.Enabled = true;
            timer2.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label3.Text = "Таймер выключен";
            label3.BackColor = Color.Red;
            label1.Text = "00:00:00";
            sec = Convert.ToDateTime("00:00:00");
            timer1.Enabled = false;
            timer2.Enabled = false;
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
    }
}
