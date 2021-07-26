using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CuteWittleVirus
{
    public class Virus
    {
        private FileSystemWatcher watcher;
        private string Id;
        private Timer timer;
        private string FolderPath;

        private string FileName
        {
            get
            {
                return "cutewittlevirus_" + Id + ".txt";
            }
        }

        public Virus(string folderPath, List<string> visitedfolders)
        {
            FolderPath = folderPath;

            //add folder to visited folders list so virus spawn does not double back on itself
            //will cause endless loop if this is not done
            visitedfolders.Add(folderPath);

            //check number of viruses already in folder, spawn to other folders if so
            IEnumerable<string> existingFiles = Directory.EnumerateFiles(folderPath, "cutewittlevirus*", SearchOption.TopDirectoryOnly);
            int viruscount = existingFiles.Count();  
            if (GameManager.Playing && viruscount >= GameManager.InfectionThreshold)
            {
                string[] subfolders = Directory.GetDirectories(folderPath, "*", SearchOption.TopDirectoryOnly);
                if (subfolders.Any() && !subfolders.All(value => visitedfolders.Contains(value)))
                {
                    //spawn into each subfolder [that hasn't been visited by this spawn chain]
                    foreach (string subfolder in subfolders)
                    {
                        if (!visitedfolders.Contains(subfolder))
                        {
                            Virus newVirus = new Virus(subfolder, visitedfolders);
                        }
                    }
                }
                else if (folderPath != GameManager.RootFolder)
                {
                    //spawn into parent folder (unless we're in the root folder)
                    Virus newVirus = new Virus(Directory.GetParent(folderPath).FullName, visitedfolders);
                }
                else
                {
                    //lose game if in root folder and it is infected
                    GameManager.LoseGame();
                }
            }
            else if (GameManager.Playing)
            {
                //spawn into current folder
                Id = Guid.NewGuid().ToString();
                File.Create(folderPath + "\\" + FileName).Close();
                watcher = new FileSystemWatcher(folderPath);
                watcher.IncludeSubdirectories = false;
                watcher.Created += FileSystemWatcher_Changed;
                watcher.Renamed += FileSystemWatcher_Changed;
                watcher.Deleted += FileSystemWatcher_Changed;
                watcher.EnableRaisingEvents = true;

                GameManager.UpdateVirusCount(1);

                timer = new Timer(Tick, null, GameManager.SpawnRate * 1000, GameManager.SpawnRate * 1000);
            }
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //kill watchers and spawn activity if virus file has been deleted
            if (e.Name == FileName && e.ChangeType == WatcherChangeTypes.Deleted)
            {
                Kill();
            }
        }

        public void Kill()
        {
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
            if (timer != null)
            {
                timer.Dispose();
            }
            GameManager.UpdateVirusCount(-1);

            if (GameManager.TotalVirusCount == 0)
            {
                GameManager.WinGame();
            }
        }

        private void Tick(Object stateInfo)
        {
            if (GameManager.Playing)
            {
                Virus newVirus = new Virus(FolderPath, new List<string>());
            }
        }
    }
}
