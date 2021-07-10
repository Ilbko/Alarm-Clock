using System.ServiceProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using NAudio.Wave.SampleProviders;
using System.ComponentModel.Composition;
using System.IO;
using System.Management;
using System.Xml.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace DateTimeAppService
{
    public partial class Service1 : ServiceBase
    {
        private string settingsPath;
        private XmlSerializer settingsSerializer = new XmlSerializer(typeof(Settings));
        public bool alarmWorked;

        public Service1()
        {
            InitializeComponent();
        }

        private string GetUserName()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem");
            ManagementObjectCollection collection = searcher.Get();

            return collection.Cast<ManagementBaseObject>().First()["UserName"].ToString().Split('\\')[1].Split('-').Last();
        }

        protected override void OnStart(string[] args)
        {
            Task.Run(() => this.Start());
        }

        private void Start()
        {
            EventLog.WriteEntry("start");
            DateTime alarmTime = new DateTime(2020, 1, 1);
            settingsPath = Path.GetPathRoot(Environment.SystemDirectory) + @"Users\" + $"{GetUserName()}" + @"\AppData\Roaming\AlarmSettings.xml";

            try
            {
                using (FileStream stream = new FileStream(this.settingsPath, FileMode.OpenOrCreate))
                {
                    try
                    {
                        alarmTime = ((Settings)this.settingsSerializer.Deserialize(stream) as Settings).alarmTime;
                    }
                    catch (System.Exception e) { EventLog.WriteEntry(e.Message); }
                }

                if (DateTime.Now < alarmTime)
                    this.alarmWorked = false;
                else
                    this.alarmWorked = true;

                if (!alarmWorked)
                {
                    try
                    {
                        EventLog.WriteEntry("alarm has not worked yet");
                        WASAPI Player = null;

                        while (!alarmWorked)
                        {
                            if (DateTime.Now >= alarmTime)
                            {
                                Player = new WASAPI(Path.GetPathRoot(Environment.SystemDirectory) + @"Users\" + $"{GetUserName()}" + @"\AppData\Roaming\Alarm.wav");
                                this.alarmWorked = true;
                            }

                            Thread.Sleep(5000);
                        }
                    } catch (System.Exception e) { EventLog.WriteEntry(e.Message); }

                }
            }
            catch (System.Exception e) { EventLog.WriteEntry(e.Message); }
        }

        protected override void OnStop()
        {
        }

        [Serializable]
        public class Settings
        {
            public Settings() { }

            public bool timeFormat { get; set; }
            public string dateBrush { get; set; }
            public string timeBrush { get; set; }
            public string backgroundBrush { get; set; }
            public bool isAutorun { get; set; }
            public bool nightTheme { get; set; }
            public bool nightThemeToggled { get; set; }
            public bool monthFormat { get; set; }
            public DateTime alarmTime { get; set; }
            public bool isAlarmSet { get; set; }
        }

        public interface IInputFileFormatPlugin
        {
            string Name { get; }
            string Extension { get; }
            WaveStream CreateWaveStream(string fileName);
        }

        [Export(typeof(IInputFileFormatPlugin))]
        class WaveInputFilePlugin : IInputFileFormatPlugin
        {
            public string Name
            {
                get { return "WAV file"; }
            }

            public string Extension
            {
                get { return ".wav"; }
            }

            public WaveStream CreateWaveStream(string fileName)
            {
                WaveStream readerStream = new WaveFileReader(fileName);

                if (readerStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm
                    && readerStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
                {
                    readerStream = WaveFormatConversionStream.CreatePcmStream(readerStream);
                    readerStream = new BlockAlignReductionStream(readerStream);
                }

                return readerStream;
            }
        }

        public class WASAPI
        {
            public static void Concatenate(string outputFile, IEnumerable<string> sourceFiles)
            {
                byte[] buffer = new byte[1024];
                WaveFileWriter waveFileWriter = null;
                
                try
                {
                    foreach (string sourceFile in sourceFiles)
                    {
                        using (WaveFileReader reader = new WaveFileReader(sourceFile))
                        {
                            if (waveFileWriter == null)
                                waveFileWriter = new WaveFileWriter(outputFile, reader.WaveFormat);
                            else
                            {
                                if (!reader.WaveFormat.Equals(waveFileWriter.WaveFormat))
                                    throw new InvalidOperationException("Can't concatenate WAV Files that don't share the same format");
                            }

                            int read;
                            while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                                waveFileWriter.Write(buffer, 0, read);
                        }
                    }
                }
                finally
                {
                    if (waveFileWriter != null)
                        waveFileWriter.Dispose();
                }
            }

            WaveStream fileWaveStream;
            Action<float> setVolumeDelegate;

            public ISampleProvider CreateInputStream(string fileName)
            {
                var plugin = new WaveInputFilePlugin();
                if (plugin == null)
                    throw new InvalidOperationException("Unsupported file extension");

                fileWaveStream = plugin.CreateWaveStream(fileName);

                var waveChannel = new NAudio.Wave.SampleProviders.SampleChannel(fileWaveStream);
                setVolumeDelegate = (vol) => waveChannel.Volume = vol;
                var postVolumeMeter = new MeteringSampleProvider(waveChannel);

                return postVolumeMeter;
            }

            private IWavePlayer waveOut = new WasapiOut(NAudio.CoreAudioApi.AudioClientShareMode.Shared, 300);

            public WASAPI(string fileName)
            {
                ISampleProvider sampleProvider = null;
                sampleProvider = CreateInputStream(fileName);
                waveOut.Init(new SampleToWaveProvider(sampleProvider));
                waveOut.Play();
                return;
            }
        }


    }
}
