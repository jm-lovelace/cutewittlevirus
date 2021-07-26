using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CuteWittleVirus
{
    public class Antivirus
    {
        private FileSystemWatcher watcher;
        private Timer timer;
        private string FolderPath;

        public Antivirus(string folderPath)
        {
            FolderPath = folderPath;
            if (GameManager.AntivirusCount < GameManager.MaxAntivirus)
            {
                //kill viruses already in folder
                IEnumerable<string> existingFiles = Directory.EnumerateFiles(folderPath, "cutewittlevirus*", SearchOption.TopDirectoryOnly);
                foreach (string virus in existingFiles)
                {
                    File.Delete(virus);
                }

                //create filesystemwatcher for folder
                watcher = new FileSystemWatcher(folderPath);
                watcher.IncludeSubdirectories = false;
                watcher.Created += FileSystemWatcher_Changed;
                watcher.Renamed += FileSystemWatcher_Changed;
                watcher.Deleted += FileSystemWatcher_Changed;
                watcher.EnableRaisingEvents = true;

                //create decay timer
                timer = new Timer(Tick, null, GameManager.SpawnRate * GameManager.AntivirusDecay * 1000, Timeout.Infinite);

                GameManager.AntivirusCount++;
            }
            else //delete antivirus if max already used
            {
                File.Delete(folderPath + "\\antivirus.txt");
            }
        }

        private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.Name == "antivirus.txt" && e.ChangeType == WatcherChangeTypes.Deleted)
            {
                Kill();
            }
            else if (e.Name.Contains("cutewittlevirus") && e.ChangeType == WatcherChangeTypes.Created)
            {
                TryDeleteFile(e.FullPath);
            }
        }

        public void TryDeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
                System.Threading.Thread.Sleep(1000);
                TryDeleteFile(path);
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
        }

        private void Tick(Object stateInfo)
        {
            TryDeleteFile(FolderPath + "\\antivirus.txt");
            Kill();
        }
    }
}
