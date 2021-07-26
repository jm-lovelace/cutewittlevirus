using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CuteWittleVirus
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        private double timeElapsed;

        public Form1()
        {
            InitializeComponent();
            GameManager.Form = this;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (!GameManager.Playing)
            {
                GameManager.MaxAntivirus = (int)maxAntivirusBox.Value;
                GameManager.AntivirusDecay = (int)antivirusDecayBox.Value;
                GameManager.MaxFolderDepth = (int)maxDepthBox.Value;
                GameManager.MaxSubfolders = (int)maxSubfoldersBox.Value;
                GameManager.StartingViruses = (int)startingVirusesBox.Value;
                GameManager.InfectionThreshold = (int)infectionThresholdBox.Value;
                GameManager.SpawnRate = (int)spawnRateBox.Value;

                playButton.Text = "PLAYING - PRESS TO STOP";
                playButton.BackColor = Color.Red;
                timeElapsed = 00.00;
                GameManager.StartGame();

                //myTimer.Tick += new EventHandler(TimerEventProcessor);

                //// Sets the timer interval to 5 seconds.
                //myTimer.Interval = 1000;
                //myTimer.Start();
            }
            else
            {
                GameManager.StopGame();
                Close();
            }
        }

        private void TimerEventProcessor(object sender, EventArgs e)
        {
            timeElapsed += 0.01;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
