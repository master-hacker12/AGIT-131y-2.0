using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Un4seen.Bass;
using Un4seen.BassAsio;
using Un4seen.BassWasapi;

namespace AGIT_131y_2._0
{
    class BassLike
    {
        private static int Hz = 44100; // частота дискретизации

        public static bool InitDefaultDevice;// состояние инициализации

        public static int Stream;// поток

        public static int Volume = 100;

        public static bool InitBass(int hz)
        {
            if (!InitDefaultDevice)
                InitDefaultDevice = Bass.BASS_Init(-1, hz, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            return InitDefaultDevice;
        }

        public static void Stop()
        {
            Bass.BASS_ChannelStop(Stream);
            Bass.BASS_StreamFree(Stream);
            Stream = 0;
        }

        public static void SetVolumeStream (int stream, int vol)
        {
            Volume = vol;
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100);
        }

        public static int Play (string filename, int vol)
        {


                 Stop();
            if (InitBass(Hz))
            {
                Stream = Bass.BASS_StreamCreateFile(filename, 0, 0, BASSFlag.BASS_DEFAULT);
                if (Stream != 0)
                {
                    Volume = vol;
                    Bass.BASS_ChannelSetAttribute(Stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100);
                    Bass.BASS_ChannelPlay(Stream, false);
                    return 1;
                }
                else
                    return 0;
            }
            else
                return 0;
        }

        public static int GetTimeStream (int stream)
        {
            long TimeBytes = Bass.BASS_ChannelGetLength(stream);
            double Time = Bass.BASS_ChannelBytes2Seconds(stream, TimeBytes);
            return (int)(Time*1000);
        }

    }
}
