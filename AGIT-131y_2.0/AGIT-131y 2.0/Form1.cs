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
        public static string sb = null;
        Announcer[] announcer = null;

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
            timer3.Enabled = true;
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
                    MessageBox.Show("Файл files.cfg не доступен", "Ошибка файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            
                StreamWriter route = new StreamWriter("files.cfg");
            
            
            string[] fs = Directory.GetFiles(@"Route\", "*.txt");
            for (int i = 0; i < fs.Length; i++)
            {
                route.WriteLine(fs[i]);
            }
            label5.Text = fs.Length.ToString();
            route.Close();
            string templates = "Шаблоны.txt";
            StreamReader temp = null;
            try
            {
                temp = new StreamReader(templates);
                button2.Enabled = true;
                string[] templsound = File.ReadAllLines(templates);
                int count = 0;
                for (int i = 0;i<templsound.Length;i++)
                {
                    if (templsound[i] == "---")
                        count++;
                }
                if (count == 0)
                    throw new ArgumentException("Шаблоны не найдены");
                announcer = new Announcer[count-1];
                int j = 0;
                for (int i = 0; i<announcer.Length;i++)
                {
                    announcer[i] = new Announcer();
                    while (templsound[0]!="---")
                    {
                        j++;
                    }
                    j++;
                    announcer[i].name = templsound[j];
                    announcer[i].path = "Sound\\Шаблоны\\" + templsound[j] + ".wav";
                    j++;
                    announcer[i].timeout = Convert.ToInt32(templsound[j]);
                    j++;
                }
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Файл с шаблонами не найден", "Ошибка файла",MessageBoxButtons.OK,MessageBoxIcon.Error);
                button2.Enabled = false;
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show("Файл с шаблонами не найден", "Ошибка файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button2.Enabled = false;
            }
            catch (IOException e)
            {
                MessageBox.Show("Файл недоступен для чтения", "Ошибка файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button2.Enabled = false;
            }
            catch (ArgumentException e)
            {
                MessageBox.Show("Шаблоны не найдены", "Ошибка файла", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button2.Enabled = false;
            }



            temp.Close();
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
            sb = label3.Text;
            button1.Text = "1";
            button2.Text = "2";
            Play_sound();
        }
        public void Play_sound()
        {
           
            if (files.Length == 1)
            {

                if ((bg) && (!timer1.Enabled))
                {
                    textBox1.Invoke(new Action(() => textBox1.BackColor = Color.Lime));
                    label3.Invoke(new Action(() => label3.Text = ""));
                }
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
                    SP = new SoundPlayer("Sound\\Шаблоны\\ping.wav");
                    SP.Play();
                    Thread.Sleep(320);
                    SP.Stop();
                }

                textBox1.Invoke(new Action(() => textBox1.BackColor = Color.DimGray));
                if (timer1.Enabled)
                {
                    button1.Invoke(new Action(() => button1.Text = "1"));
                    button3.Invoke(new Action(() => button3.Text = ""));
                    button2.Invoke(new Action(() => button2.Text = "2"));
                }
            }
            else
            {
               
                for (int i = 0; i < files.Length; i++)
                {
                    label3.Invoke(new Action(() => label3.Text = ""));
                    button1.Invoke(new Action(() => button1.Enabled = false));
                    button1.Invoke(new Action(() => button1.Text = ""));
                    button3.Invoke(new Action(() => button3.Text = ""));
                    button3.Invoke(new Action(() => button3.Enabled = false));
                    textBox1.Invoke(new Action(() => textBox1.BackColor = Color.Lime));
                    bool find = false;
                    int find_pos = -1;
                   for (int j = 0;j<announcer.Length;j++)
                    {
                        if (files[i] == announcer[j].name)
                        {
                            find = true;
                            find_pos = j;
                            break;
                        }
                    }
                  if (find)
                    {
                        try
                        {
                            SP = new SoundPlayer(announcer[find_pos].path);
                            SP.Play();
                            Thread.Sleep(announcer[find_pos].timeout);
                            continue;
                        }
                        catch (Exception e)
                        {
                            SP = new SoundPlayer("Sound\\Шаблоны\\ping.wav");
                            SP.Play();
                            Thread.Sleep(320);
                            continue;
                        }
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
                            SP = new SoundPlayer("Sound\\Шаблоны\\ping.wav");
                            SP.Play();
                            Thread.Sleep(320);
                            continue;
                        }

                    }
                }

                button1.Invoke(new Action(() => button1.Text = "Воспроизвести"));
                button3.Invoke(new Action(() => button3.Text = "В конец маршрута"));
                if (timer1.Enabled)
                {
                    button1.Invoke(new Action(() => button1.Text = "1"));
                    button3.Invoke(new Action(() => button3.Text = ""));
                    button2.Invoke(new Action(() => button2.Text = "2"));
                }
            }
            label3.Invoke(new Action(() => label3.Text = sb));
            button1.Invoke(new Action(() => button1.Enabled = true));
            button3.Invoke(new Action(() => button3.Enabled = true));
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
            if ((key4 != 0))
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

                    if ((files[i] == "Техническая остановка") && ((files[0] != "ОДЗ")||(files[0] != "Посадка закончена")))
                    {
                        label3.Text = files[0] + " - конечная";
                    }

                    if ((files[i] != "ОДЗ") && (files[i] != "Конечная") && (files[i] != "Посадка закончена")
                        && (i == 0))

                    {
                        label3.Text = f[0];
                    }


                }

                sb = label3.Text;

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

                    if ((files[i] == "Техническая остановка") && ((files[0] != "ОДЗ") || (files[0] != "Посадка закончена")))
                    {
                        label3.Text = files[0] + " - конечная";
                    }
                    if ((files[i] != "ОДЗ") && (files[i] != "Конечная")  && (files[i] != "Посадка закончена") 
                        && (i == 0))

                        {
                            label3.Text = f[0];
                        }

                    sb = label3.Text;
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

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (button1.Text == "")
                button1.Enabled = false;
            else
                button1.Enabled = true;
            if (button3.Text == "")
                button3.Enabled = false;
            else
                button3.Enabled = true;
        }
    }
}
