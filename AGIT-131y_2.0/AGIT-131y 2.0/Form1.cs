using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Media;
using System.Runtime.InteropServices;

namespace AGIT_131y_2._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("User32.dll")]
        internal static extern short GetAsyncKeyState(int vkey);
        public static StreamReader routers = null;
        public static StreamReader stops = null;
        public static string rout = " ";
        public static string number = " ";
        public static string stop = " ";
        public static string depo = " ";
        public static string[] files = null;
        SoundPlayer SP = null;
        public static char key = ' ';
        bool bg = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = null;
            label2.Text = null;
            label3.Text = null;
            textBox1.BackColor = Color.DimGray;
            textBox2.BackColor = Color.DimGray;
            textBox3.BackColor = Color.DimGray;
            button1.Text = "1";
            button2.Text = "2";
            button3.Text = "";
            button4.Text = "Обновить";
            button1.Enabled = false;
            button3.Enabled = false;
            label6.Text = " ";
            Active();
          


        }

        public void Active()
        {
      
            timer2.Enabled = true;
            if (routers != null)
            {
                routers.Close();
                routers = null;
            }
            StreamReader warning = null;
            try
            {
                warning = new StreamReader("warning.txt");
                string war = warning.ReadLine();
                if (war != null)
                {
                    label6.Text = "Изменение маршрутов:";
                    label6.Text = label6.Text + '\n' + war;
                    while (!warning.EndOfStream)
                    {
                        label6.Text = label6.Text + '\n' + war;
                        war = warning.ReadLine();
                    }

                }
                warning.Close();
            }
            catch (ArgumentNullException e)
            {
                File.Create("warning.txt");
            }
            catch (FileNotFoundException e)
            {
                File.Create("warning.txt");
            }

            if (File.Exists("files.cfg"))
                try
                {
                    File.Delete("files.cfg");
                }
                catch (IOException e)
                {

                }
            
                StreamWriter route = new StreamWriter("files.cfg");
            
            
            string[] fs = Directory.GetFiles(@"Route\", "*.txt");
            for (int i = 0; i < fs.Length; i++)
            {
                route.WriteLine(fs[i]);
            }
            label5.Text = fs.Length.ToString();
            route.Close();
            label1.Font = Properties.Settings.Default.Font1;
            label2.Font = Properties.Settings.Default.Font2;
            label3.Font = Properties.Settings.Default.Font3;
            label1.ForeColor = Properties.Settings.Default.Color1;
            label2.ForeColor = Properties.Settings.Default.Color2;
            label3.ForeColor = Properties.Settings.Default.Color3;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.BackColor = Color.Red;
            timer1.Enabled = true;
            button1.Enabled = true;
            button4.Enabled = true;
            if (routers == null)
                routers = new StreamReader("files.cfg");
            if (stops != null)
                stops.Close();
            if (routers.EndOfStream)
            {
                routers.Close();
                routers = new StreamReader("files.cfg");
            }
            rout = routers.ReadLine();
            if (rout == null)
            {
                MessageBox.Show("Маршруты не найдены", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                stops = new StreamReader(rout);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Файл "+ rout +" не найден, возможно он был удален", "Ошибка выбор маршрута", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Active();
                return;
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Файл " + rout + " не найден, возможно он был удален", "Ошибка выбор маршрута", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Active();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неизвестная ошибка", "Ошибка выбор маршрута", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Active();
                return;
            }
            stop = stops.ReadLine();
            label1.Text = stop;
            if (label1.Text == "В депо")
            {
                depo = label1.Text;
                label1.Text = "В" + '\n' + "депо";
            }
            else
            {
                depo = " ";
            }
            stop = stops.ReadLine();
            label2.Text = stop;
            stop = stops.ReadLine();
            label3.Text = stop;
            files = new string[1];
            files[0] = stop;
            bg = false; 
            Play_sound();
        }
        public void Play_sound()
        {
            if (files.Length == 1)
            {

                if (bg)
                    textBox1.Invoke(new Action(() => textBox1.BackColor = Color.Lime));
                   
                
                string st;
                try
                {
                    if (depo == "В депо")
                    {
                        st = "Sound\\" + depo + "\\" + files[0] + ".wav";
                    }
                    else
                    {
                        st = "Sound\\" + label1.Text + "\\" + files[0] + ".wav";
                    }
                    SP = new SoundPlayer(st);
                 

                    SP.Play();
                    if (bg)
                        Thread.Sleep(Announcer.timeout_default2);
                }
                catch (Exception ex)
                {
                    SP.Stop();
                }

                textBox1.Invoke(new Action(() => textBox1.BackColor = Color.DimGray));
            }
            else
            {

                for (int i = 0; i < files.Length; i++)
                {
                    button1.Invoke(new Action(() => button1.Enabled = false));
                    textBox1.Invoke(new Action(() => textBox1.BackColor = Color.Lime));


                    if (files[i] == "ОДЗ")
                    {
                        //label3.Text = "следующая остановка " + files[i + 1];
                        SP = new SoundPlayer(Announcer.ODZ.path);
                        SP.Play();
                        Thread.Sleep(Announcer.ODZ.timeout);
                        continue;
                    }
                    if (files[i] == "Конечная")
                    {
                        //if (i == 1)
                        //{
                        //    label3.Text = files[i - 1] + " - конечная";
                        //}
                        SP = new SoundPlayer(Announcer.End.path);
                        SP.Play();
                      
                            Thread.Sleep(Announcer.End.timeout);
                        continue;
                    }
                    if (files[i] == "Напоминаем")
                    {

                        SP = new SoundPlayer(Announcer.Remember.path);
                        SP.Play();
                      
                            Thread.Sleep(Announcer.Remember.timeout);
                        continue;
                    }
                    if (files[i] == "Начало движения")
                    {

                        SP = new SoundPlayer(Announcer.Start_moution.path);
                        SP.Play();
                    
                            Thread.Sleep(Announcer.Start_moution.timeout);
                        continue;
                    }
                    if (files[i] == "Оплатив проезд")
                    {

                        SP = new SoundPlayer(Announcer.Pay.path);
                        SP.Play();
                       
                            Thread.Sleep(Announcer.Pay.timeout);
                        continue;
                    }
                    if (files[i] == "Напоминаем")
                    {

                        SP = new SoundPlayer(Announcer.Remember.path);
                        SP.Play();
                      
                            Thread.Sleep(Announcer.Remember.timeout);
                        continue;
                    }
                    if (files[i] == "Кнопка на поручне")
                    {

                        SP = new SoundPlayer(Announcer.Button.path);
                        SP.Play();
                       
                            Thread.Sleep(Announcer.Button.timeout);
                        continue;
                    }
                    if (files[i] == "Остановка на выход")
                    {

                        SP = new SoundPlayer(Announcer.Stop_on_out.path);
                        SP.Play();
                      
                            Thread.Sleep(Announcer.Stop_on_out.timeout);
                        continue;
                    }
                    if (files[i] == "Перед выходом")
                    {

                        SP = new SoundPlayer(Announcer.Before_out.path);
                        SP.Play();
                     
                            Thread.Sleep(Announcer.Before_out.timeout);
                        continue;
                    }
                    if (files[i] == "Посадка закончена")
                    {

                        SP = new SoundPlayer(Announcer.Entry_out.path);
                        SP.Play();
                     
                            Thread.Sleep(Announcer.Entry_out.timeout);
                        continue;
                    }
                    if (files[i] == "Проезжая часть")
                    {

                        SP = new SoundPlayer(Announcer.Route_part.path);
                        SP.Play();
                    
                            Thread.Sleep(Announcer.Route_part.timeout);
                        continue;
                    }
                    if (files[i] == "Только для высадки")
                    {

                        SP = new SoundPlayer(Announcer.Only_out.path);
                        SP.Play();

                        Thread.Sleep(Announcer.Only_out.timeout);
                        continue;
                    }
                    if (files[i] == "Уступите")
                    {

                        SP = new SoundPlayer(Announcer.Give_in.path);
                        SP.Play();
                        
                            Thread.Sleep(Announcer.Give_in.timeout);
                        continue;
                    }
                    if (files[i] == "Штраф")
                    {

                        SP = new SoundPlayer(Announcer.Shtraf.path);
                        SP.Play();
                      
                            Thread.Sleep(Announcer.Shtraf.timeout);
                        continue;
                    }
                    if (files[i] == "Реклама")
                    {

                        SP = new SoundPlayer(Announcer.Ad.path);
                        SP.Play();
                     
                            Thread.Sleep(Announcer.Ad.timeout);
                        continue;
                    }
                    if (files[i] == "Следует до")
                    {
                        SP = new SoundPlayer(Announcer.Move.path);
                        SP.Play();
                        Thread.Sleep(Announcer.Move.timeout);
                        continue;
                    }
                    if (files[i] == "При выходе")
                    {
                        SP = new SoundPlayer(Announcer.Out.path);
                        SP.Play();
                    
                            Thread.Sleep(Announcer.Out.timeout);
                        continue;
                    }
                    if (files[i] == "Билеты")
                    {
                        SP = new SoundPlayer(Announcer.Tickets.path);
                        SP.Play();
                 
                            Thread.Sleep(Announcer.Tickets.timeout);
                        continue;
                    }
                    else
                    {
                        try
                        {

                            string p = "Sound\\" + label1.Text + "\\" + files[i] + ".wav";
                            SP = new SoundPlayer(p);
                            SP.Play();
                        
                                Thread.Sleep(Announcer.timeout_default);

                        
                        }
                        catch (Exception e)
                        {
                          
                                Thread.Sleep(Announcer.timeout_default);
                            continue;
                        }

                    }
                }

                
            }
            button1.Invoke(new Action(() => button1.Enabled = true));
            textBox1.Invoke(new Action(() => textBox1.BackColor = Color.DimGray));
            SP = null;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (textBox2.BackColor == Color.DimGray)
                textBox2.BackColor = Color.Lime;
            else
                textBox2.BackColor = Color.DimGray;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            short key1 = GetAsyncKeyState(106);
            short key2 = GetAsyncKeyState(109);
            short key3 = GetAsyncKeyState(107);
            short key4 = GetAsyncKeyState(34);

            if ((key1 != 0) && (button1.Enabled))
            {
                if (SP!=null)
                SP.Stop();
                key = '*';
                button1_Click(sender, e);
            }
            if ((key2 != 0) && (button2.Enabled))
            {
                button2_Click(sender, e);
            }
            if ((key3 != 0) && (button3.Enabled))
            {
                button3_Click(sender, e);
            }
            if ((key4 != 0) && (button3.Enabled))
            {
                if (SP != null)
                    SP.Stop();
                key = '+';
                button1_Click(sender, e);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bg = true;
            button1.Text = "Воспроизвести";
            button2.Text = "Сменить маршрут";
            button3.Text = "В конец маршрута";
            timer1.Enabled = false;
            button4.Enabled = false;
            button3.Enabled = true;
            if ((stop != "--") && (stop != "----------"))
            {
                stop = stops.ReadLine();
            }
            if (stops.EndOfStream)
            {
                stops.Close();
                stops = new StreamReader(rout);
                stop = stops.ReadLine();
                stop = stops.ReadLine();
                stop = stops.ReadLine();
                stop = stops.ReadLine();
            }
            if (stop == "----------")
            {
                textBox2.BackColor = Color.Lime;
                stop = stops.ReadLine();
                label2.Text = stop;
                stop = stops.ReadLine();
                if (stop == null)
                {
                    stops.Close();
                    stops = new StreamReader(rout);
                    stop = stops.ReadLine();
                    stop = stops.ReadLine();
                    stop = stops.ReadLine();
                    stop = stops.ReadLine();
                    stop = stops.ReadLine();
                    stop = stops.ReadLine();
                }
            }
            else
                textBox2.BackColor = Color.DimGray;
            if (stop == "--")
            {
                string[] f = new string[15];
                stop = stops.ReadLine();

                f[0] = stop;
                int count = 1;
                stop = stops.ReadLine();
                while ((stop != "--") && (stop != "----------"))
                {

                    f[count] = stop;
                    count++;
                    stop = stops.ReadLine();
                }
                files = null;
                files = new string[count];

                for (int i = 0; i < count; i++)
                {
                    files[i] = f[i];
                    if (files[i] == "ОДЗ")
                    {
                        label3.Text = "следующая остановка " + f[i + 1];
                        continue;
                    }
                    if (files[i] == "Конечная")
                    {
                        if (i == 1)
                        {
                            label3.Text = files[i - 1] + " - конечная";
                            continue;
                        }
                    }
                    if (files[i] == "Посадка закончена")
                    {
                        label3.Text = "следующая остановка " + f[i + 1];
                    }

                    if ((files[i] != "ОДЗ") && (files[i] != "Конечная") && (files[i] != "Напоминаем") &&
                    (files[i] != "Кнопка на поручне") && (files[i] != "Остановка на выход") && (files[i] != "Перед выходом") && (files[i] != "Посадка закончена") && (files[i] != "Проезжая часть")
                    && (files[i] != "Только для высадки") && (files[i] != "Уступите") && (files[i] != "Штраф") &&
                    (files[i] != "Реклама") && (files[i] != "Следует до") && (files[i] != "При выходе") && (files[i] != "Билеты") && (i == 0))

                    {
                        label3.Text = f[0];
                    }


                }

                if (key != '+')
                {
                    Thread tr = new Thread(new ThreadStart(Play_sound));
                    tr.Start();
                }
                else
                {
                    string st = "Sound\\Шаблоны\\ping.wav";
                    SoundPlayer SP1 = new SoundPlayer(st);
                    SP1.Play();

                }
                key = ' ';
                //Play_sound();
           
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            while (stop != "----------")
            {
                stop = stops.ReadLine();
            }
            stop = stops.ReadLine();
            textBox2.BackColor = Color.Lime;
            label2.Text = stop;
            stop = stops.ReadLine();
            if (stops.EndOfStream)
            {
                stops.Close();
                stops = new StreamReader(rout);
                stop = stops.ReadLine();
                stop = stops.ReadLine();
                stop = stops.ReadLine();
                stop = stops.ReadLine();
                stop = stops.ReadLine();
                stop = stops.ReadLine();
            }
            if (stop == "--")
            {
                
                      string[] f = new string[15];
                    stop = stops.ReadLine();

                    f[0] = stop;
                    int count = 1;
                    stop = stops.ReadLine();
                    while ((stop != "--") && (stop != "----------"))
                    {

                        f[count] = stop;
                        count++;
                        stop = stops.ReadLine();
                    }
                    files = null;
                    files = new string[count];

                    for (int i = 0; i < count; i++)
                    {
                        files[i] = f[i];
                        if (files[i] == "ОДЗ")
                        {
                            label3.Text = "следующая остановка " + f[i + 1];
                            continue;
                        }
                        if (files[i] == "Конечная")
                        {
                            if (i == 1)
                            {
                                label3.Text = files[i - 1] + " - конечная";
                                continue;
                            }
                        }
                        if (files[i] == "Посадка закончена")
                        {
                            label3.Text = "следующая остановка " + files[i + 1];
                        }

                        if ((files[i] != "ОДЗ") && (files[i] != "Конечная") && (files[i] != "Напоминаем") &&
                        (files[i] != "Кнопка на поручне") && (files[i] != "Остановка на выход") && (files[i] != "Перед выходом") && (files[i] != "Посадка закончена") && (files[i] != "Проезжая часть")
                        && (files[i] != "Только для высадки") && (files[i] != "Уступите") && (files[i] != "Штраф") &&
                        (files[i] != "Реклама") && (files[i] != "Следует до") && (files[i] != "При выходе") && (i == 0))

                        {
                            label3.Text = f[0];
                        }


                    }
                    Thread tr = new Thread(new ThreadStart(Play_sound));
                    tr.Start();

                
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Font1 = label1.Font;
            Properties.Settings.Default.Font2 = label2.Font;
            Properties.Settings.Default.Font3 = label3.Font;
            Properties.Settings.Default.Color1 = label1.ForeColor;
            Properties.Settings.Default.Color2 = label2.ForeColor;
            Properties.Settings.Default.Color3 = label3.ForeColor;
            //сохранение настроек
            Properties.Settings.Default.Save();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();

            f2.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Active();
        }
    }
}
