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
using Un4seen.Bass;
using Un4seen.BassAsio;
using Un4seen.BassWasapi;


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
        bool end = false;
        string endstr = "";
        public static int ping = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = null;
            label2.Text = null;
            label3.Text = null;
            textBox1.BackColor = Color.DimGray;
            textBox2.BackColor = Color.DimGray;
            textBox3.BackColor = Color.DimGray;
            button1.Text = "";
            button2.Text = "2";
            button3.Text = "";
            button4.Text = "Обновить";
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
                    label6.Text += '\n' + war;
                    while (!warning.EndOfStream)
                    {
                        war = warning.ReadLine();
                        label6.Text = label6.Text + '\n' + war;
                        
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
                for (int i = 0; i<announcer.Length;i++)
                {
                    announcer[i] = new Announcer();
                    
                    announcer[i].name = templsound[i];
                    announcer[i].path = "Sound\\Шаблоны\\" + templsound[i] + ".wav";
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
            ping = Properties.Settings.Default.timeout;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = "| Режим выбора маршрута";
            endstr = "";
            end = false;
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
            label3.Text = null;
            sb = label3.Text;
            files = new string[1];
            files[0] = stop;
            bg = false;
            button1.Text = "1";
            button2.Text = "2";
            Play_sound();
          
        }


        public void Play_sound()
        {
            toolStripStatusLabel1.Text = "Идет вопроизведение |";
           
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
                        st = "Sound\\" + "Названия" + "\\" + files[0] + ".wav";                   
                    int state = BassLike.Play(st, BassLike.Volume);
                    int timeout = BassLike.GetTimeStream(BassLike.Stream);
                    if (bg)
                    {                     
                        if (state != 0)
                        Thread.Sleep(timeout);
                        else
                            throw new FileNotFoundException("");
                    }

                }
                catch (FileNotFoundException ex)
                {
                    try
                    {
                        SP = new SoundPlayer("Sound\\Шаблоны\\ping.wav");
                        SP.Play();
                        Thread.Sleep(320);
                        SP.Stop();
                    }
                    catch (Exception exp)
                    {

                    }
                }
                catch(TypeInitializationException ex)
                {
                    MessageBox.Show("Возможно отстутсвует библиотека bass.dll. Скачайте bass.dll с официального сайта и скопируйте в C:\\windows\\system32", "Ошибка воспроизведения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 
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
                    toolStripStatusLabel1.Text = "Идет вопроизведение |";
                    label3.Invoke(new Action(() => label3.Text = ""));
                    button1.Invoke(new Action(() => button1.Enabled = false));
                    button1.Invoke(new Action(() => button1.Text = ""));
                    button3.Invoke(new Action(() => button3.Text = ""));
                    button3.Invoke(new Action(() => button3.Enabled = false));
                    textBox1.Invoke(new Action(() => textBox1.BackColor = Color.Lime));
                    bool find = false;
                    int find_pos = -1;
                    for (int j = 0; j < announcer.Length; j++)
                    {
                        if (files[i] == announcer[j].name)
                        {
                            find = true;
                            find_pos = j;
                            break;
                        }
                    }
                    try
                    {
                        string p;
                        if (find)
                        p = "Sound\\" + "Шаблоны" + "\\" + files[i] + ".wav";
                        else
                        p = "Sound\\" + "Названия" + "\\" + files[i] + ".wav";
                        int state = BassLike.Play(p, BassLike.Volume);
                        if (state != 0)
                        {

                            int sl = BassLike.GetTimeStream(BassLike.Stream);                            
                            Thread.Sleep(sl+ping);
                        }
                        else
                            throw new FileNotFoundException("");
                    }
                    catch (FileNotFoundException e)
                    {
                        try
                        {
                            SP = new SoundPlayer("Sound\\Шаблоны\\ping.wav");
                            SP.Play();
                            Thread.Sleep(320);
                        }
                        catch (Exception ex)
                        {

                        }
                        continue;
                    }

                    catch (TypeInitializationException ex)
                    {
                        MessageBox.Show("Возможно отстутсвует библиотека bass.dll. Скачайте bass.dll с официального сайта и скопируйте в C:\\windows\\system32", "Ошибка воспроизведения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        

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
            toolStripStatusLabel1.Text = "  ";

            if ((end) && (endstr!=""))
            {
                textBox2.Invoke(new Action(() => textBox2.BackColor = Color.Lime));
                label2.Invoke(new Action(() => label2.Text = endstr));
                
            }
            endstr = "";
            SP = null;
        }

        private string delete_slash(string str)
        {
            string result = str;
            int proverka = 0;
            bool flag = false;
            for (int i = 0; i < result.Length; i++)
            {
                if ((result[i] == '\\') || (result[i] == '/'))
                {
                    proverka = i;
                    flag = true;
                }

            }
            if (flag)
            {
                    result = result.Substring(proverka + 1, str.Length - 1 - proverka);
                    return result;
            }
            else
                return str;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (textBox2.BackColor == Color.DimGray)
            {
                textBox2.BackColor = Color.Lime;
            }
            else
            {
                textBox2.BackColor = Color.DimGray;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            short key1 = GetAsyncKeyState(106);
            short key2 = GetAsyncKeyState(109);
            short key3 = GetAsyncKeyState(107);
            short key4 = GetAsyncKeyState(34);

            if ((textBox2.BackColor == Color.Lime) && (!timer1.Enabled))
            {
                toolStripStatusLabel2.Text = "| Конечная остановка";
            }
            if ((textBox2.BackColor == Color.DimGray) && (!timer1.Enabled))
            {
                toolStripStatusLabel2.Text = " ";
            }

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
                BassLike.Stop();
                key = '+';
                button1_Click(sender, e);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bg = true;
            end = false;
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
                end = true;
                stop = stops.ReadLine();
                endstr = stop;
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
                    if (((delete_slash(files[i]) == "Отправление") && (i!=count-1)))
                    {
                        label3.Text = "следующая остановка " + delete_slash(f[i + 1]);
                        continue;
                    }
                    if (( (delete_slash(files[i]) == "Посадка закончена")) && (i != count - 1)  )
                    {
                        try
                        {
                            label3.Text = "следующая остановка " + delete_slash(f[i + 2]);
                        }
                        catch(Exception exp)
                        {
                            label3.Text = "Ошибка файла";
                        }
                        continue;
                    }
                    if (delete_slash(files[i]) == "Конечная")
                    {
                        if (i == 1)
                        {
                            label3.Text = delete_slash(files[i - 1]) + " - конечная";
                            continue;
                        }
                    }
                    if ((delete_slash(files[i]) == "Техническая остановка") && ((delete_slash(files[0]) != "Отправление") ||(delete_slash(files[0]) != "Посадка закончена")))
                    {
                        label3.Text = delete_slash(files[0]) + " - конечная";
                    }

                    if ((delete_slash(files[i])=="По требованию")&&(i!=0))
                    {
                        label3.Text = label3.Text + " (по требованию)";
                    }

                    if ((delete_slash(files[i]) != "Отправление") && (delete_slash(files[i]) != "Конечная") && (delete_slash(files[i]) != "Посадка закончена")
                        && (i == 0))

                    {
                        label3.Text = delete_slash(f[0]);
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
                    try
                    {
                        string st = "Sound\\Шаблоны\\ping.wav";
                        SoundPlayer SP1 = new SoundPlayer(st);
                        SP1.Play();
                        if (end)
                        {
                            textBox2.BackColor = Color.Lime;
                            label2.Text = endstr;
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                key = ' ';
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
                        if( (files[i] == "ОДЗ") || (files[i] == "Посадка закончена"))
                        {
                            label3.Text = "следующая остановка " + delete_slash( f[i + 1]);
                            continue;
                        }
                        if (files[i] == "Конечная")
                        {
                            if (i == 1)
                            {
                                label3.Text = delete_slash(files[i - 1]) + " - конечная";
                                continue;
                            }
                        }

                    if ((files[i] == "Техническая остановка") && ((files[0] != "ОДЗ") || (files[0] != "Посадка закончена")))
                    {
                        label3.Text = delete_slash (files[0]) + " - конечная";
                    }
                    if ((files[i] != "ОДЗ") && (files[i] != "Конечная")  && (files[i] != "Посадка закончена") 
                        && (i == 0))

                        {
                            label3.Text = delete_slash( f[0]);
                        }

                    
                    }
                sb = label3.Text;
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
            Properties.Settings.Default.timeout = ping;
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
