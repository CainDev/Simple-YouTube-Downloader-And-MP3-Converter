using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using VideoLibrary;
using MediaToolkit;
using MediaToolkit.Model;
using System.Threading;
using System.IO;

namespace YT_Vid_Converter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<string> videoURL = new List<string>();


        public void downLoadVideo()
        {
            try
            {
                var source = $"{textBox2.Text}";
                var youtube = YouTube.Default;

                foreach (string link in videoURL)
                {
                    var vid = youtube.GetVideo(link);
                    File.WriteAllBytes(source + vid.FullName, vid.GetBytes());
                    var inputFile = new MediaFile { Filename = source + vid.FullName };
                    var outputFile = new MediaFile { Filename = $"{source + vid.FullName.Remove(vid.FullName.Length - 14, 14)}.mp3" };

                    using (var engine = new Engine())
                    {
                        engine.GetMetadata(inputFile);

                        engine.Convert(inputFile, outputFile);
                    }

                    if (checkBox1.Checked)
                    {
                        File.Delete($"{source}{vid.FullName}");
                    }

                    GC.Collect();
                }
            }
            catch { MessageBox.Show("Something went wrong. Try Restarting the program");  }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread dlVideo = new Thread(downLoadVideo);
            dlVideo.Start();
        }

        public void grabVideoName()
        {
            try
            {
                var youTibe = YouTube.Default;
                var vidName = youTibe.GetVideo(textBox1.Text);
                string videoFullName = vidName.FullName;

                this.listBox1.Invoke((MethodInvoker)delegate
                {
                    this.listBox1.Items.Add(videoFullName.Remove(videoFullName.Length - 14, 14));
                });

                videoURL.Add(textBox1.Text);
            }
            catch
            {
                MessageBox.Show("Please check your video link. If it isn't working make sure there aren't any white spaces.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread grabVideo = new Thread(grabVideoName);
            grabVideo.Start();
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            videoURL.Clear();
            listBox1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (videoURL.Any())
            {
                videoURL.RemoveAt(videoURL.Count - 1);
                listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
            }
        }
    }
}
