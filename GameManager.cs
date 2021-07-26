using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Haikunator;

namespace CuteWittleVirus
{
    public static class GameManager
    {
        public static bool Playing { get; set; }

        public static Form1 Form { get; set; }

        public static int SpawnRate { get; set; }
        public static int InfectionThreshold { get; set; }
        public static int AntivirusDecay { get; set; }
        public static int MaxFolderDepth { get; set; }
        public static int MaxSubfolders { get; set; }
        public static int MaxAntivirus { get; set; }
        public static int AntivirusCount { get; set; }

        private static Random rnd { get; set; }

        private static Haikunator.Haikunator haikunator { get; set; }

        public static string RootFolder { get; set; }

        public static int StartingViruses { get; set; }

        public static List<string> MasterFolderList { get; set; }

        public static int TotalVirusCount { get; set; }

        public static FileSystemWatcher RootWatcher { get; set; }

        public static void StartGame()
        {
            AntivirusCount = 0;
            TotalVirusCount = 0;
            rnd = new Random();
            haikunator = new Haikunator.Haikunator();

            GenerateFolderTree();

            Playing = true;

            PlaceViruses();

            RootWatcher = new FileSystemWatcher(RootFolder);
            RootWatcher.IncludeSubdirectories = true;
            RootWatcher.Created += FileSystemWatcher_Changed;
            RootWatcher.Renamed += FileSystemWatcher_Changed;
            RootWatcher.EnableRaisingEvents = true;

            Process.Start(RootFolder);
        }

        public static void WinGame()
        {
            if (Playing)
            {
                StopGame();
                MessageBox.Show(new Form { TopMost = true }, "Ok, ok, you win this time.But you just watch your wittle self!", "cutewittlevirus", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public static void LoseGame()
        {
            if (Playing)
            {
                StopGame();
                MessageBox.Show(new Form { TopMost = true }, "Hahaha LOSER LOSER! Better luck next time squirt!", "cutewittlevirus", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public static void StopGame()
        {
            Playing = false;

            Form.Invoke((MethodInvoker)(() =>
                {
                    Form.playButton.Text = "Play";
                    Form.playButton.BackColor = Color.Transparent;
                    Form.Refresh();
                }
            ));

            if (RootWatcher != null)
            {
                RootWatcher.EnableRaisingEvents = false;
                RootWatcher.Dispose();
                RootWatcher = null;
            }

            try
            {
                Directory.Delete(RootFolder, true);
            }
            catch (Exception)
            {
                System.Threading.Thread.Sleep(1000);
                StopGame();
            }
            RootFolder = null;
            MasterFolderList.Clear();
            AntivirusCount = 0;
        }

        private static void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.Name.Contains("antivirus"))
            {
                Antivirus antivirus = new Antivirus(Path.GetDirectoryName(e.FullPath));
            }
        }

        //procedurally generates folder tree using parameters and randomness
        public static void GenerateFolderTree()
        {
            RootFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\" + GenerateName();
            Directory.CreateDirectory(RootFolder);

            MasterFolderList = new List<string>() { RootFolder };

            for (int i = 0; i < MaxSubfolders; i++)
            {
                RecursDirectory(RootFolder + "\\" + GenerateName(), 1);
            }
        }

        private static void RecursDirectory(string folderPath, int level)
        {
            try
            {
                Directory.CreateDirectory(folderPath);
                MasterFolderList.Add(folderPath);

                int numSubfolders = rnd.Next(0, MaxSubfolders);
                if (level < MaxFolderDepth)
                {
                    for (int i = 0; i < numSubfolders; i++)
                    {
                        RecursDirectory(folderPath + "\\" + GenerateName(), level + 1);
                    }
                }
            }
            catch (IOException)
            {

            }
        }

        //places the initial viruses
        private static void PlaceViruses()
        {
            for (int i = 0; i < StartingViruses; i++)
            {
                int folderIndex = rnd.Next(0, MasterFolderList.Count - 1);

                Virus virus = new Virus(MasterFolderList[folderIndex], new List<string>());
            }
        }

        public static void UpdateVirusCount(int increment)
        {
            if (Playing)
            {
                GameManager.TotalVirusCount += increment;

                Form.Invoke((MethodInvoker)(() =>
                {
                    Form.playButton.Text = "PLAYING - PRESS TO STOP (" + GameManager.TotalVirusCount.ToString() + " viruses remaining)";
                }
                ));
            }
        }

        private static string GenerateName()
        {
            return haikunator.Haikunate(tokenLength: 0);
        }
    }
}
