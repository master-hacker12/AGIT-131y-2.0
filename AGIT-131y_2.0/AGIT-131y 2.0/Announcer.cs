using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AGIT_131y_2._0
{
    class Announcer
    {
        
      public  struct  ODZ
        {
            public static int timeout = 4000;
            public static string path = "Sound\\Шаблоны\\ОДЗ.wav";
        }

        public struct End
        {
            public static int timeout = 1000;
            public static string path = "Sound\\Шаблоны\\Конечная.wav";
        }
        public struct Remember
        {
            public static int timeout = 9000;
            public static string path = "Sound\\Шаблоны\\Напоминаем2.wav";
        }
        public struct Start_moution
        {
            public static int timeout = 6000;
            public static string path = "Sound\\Шаблоны\\Начало движения.wav";
        }
        public struct Pay
        {
            public static int timeout = 5000;
            public static string path = "Sound\\Шаблоны\\Оплатив проезд.wav";
        }
        public struct Button
        {
            public static int timeout = 3200;
            public static string path = "Sound\\Шаблоны\\Кнопка на поручне.wav";
        }
        public struct Stop_on_out
        {
            public static int timeout = 7000;
            public static string path = "Sound\\Шаблоны\\Остановка на выход.wav";
        }
        public struct Before_out
        {
            public static int timeout = 8500;
            public static string path = "Sound\\Шаблоны\\перед выходом.wav";
        }
        public struct Entry_out
        {
            public static int timeout = 5100;
            public static string path = "Sound\\Шаблоны\\посадка закончена.wav";
        }
        public struct Route_part
        {
            public static int timeout = 5000;
            public static string path = "Sound\\Шаблоны\\проезжая часть.wav";
        }
        public struct Only_out
        {
            public static int timeout = 2000;
            public static string path = "Sound\\Шаблоны\\Только для высадки.wav";
        }
        public struct Give_in
        {
            public static int timeout = 8000;
            public static string path = "Sound\\Шаблоны\\уступите.wav";
        }
        public struct Shtraf
        {
            public static int timeout = 10000;
            public static string path = "Sound\\Шаблоны\\штраф.wav";
        }
        public struct Ad
        {
            public static int timeout = 54000;
            public static string path = "Sound\\Шаблоны\\Реклама.wav";
        }
        public struct Move
        {
            public static int timeout = 2500;
            public static string path = "Sound\\Шаблоны\\следует до.wav";
        }
        public struct Out
        {
            public static int timeout = 7000;
            public static string path = "Sound\\Шаблоны\\при выходе.wav";
        }
        public struct Tickets
        {
            public static int timeout = 7000;
            public static string path = "Sound\\Шаблоны\\Билеты.wav";
        }
        public static int timeout_default = 5200;
        public static int timeout_default2 = 9000;
    }
}
