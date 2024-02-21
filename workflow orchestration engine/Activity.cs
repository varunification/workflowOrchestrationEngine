using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace workflow_orchestration_engine
{
    public class Activity { 
        public  string Name { get; set; } 
        public  string Description { get; set; } 
        public bool Critical { get; set; } 
        public bool Enabled { get; set; } 
        public bool Rollback { get; set; } 
        public int SleepPeriodInMilliseconds { get; set; }
        private static string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private string configFolder { get; set; }

        public void CreateFolder()
        {
             configFolder = Path.Combine(baseDirectory, $"{this.Name}");
            if (!Directory.Exists(configFolder))
            {
                DirectoryInfo di = Directory.CreateDirectory(configFolder);
            }
        }


        public void DeleteFile()
        {
            configFolder = Path.Combine(baseDirectory, $"{this.Name}");
            string Filename = $"{this.Description}.txt";
            Filename = Path.Combine(configFolder, Filename);
            if (File.Exists(Filename))
            {
                File.Delete(Filename);
            }

        }

        public void createFile()
        {
             string Filename = $"{this.Description}.txt";
            Filename = Path.Combine(configFolder, Filename);
            if (!File.Exists(Filename))
            {
                // Create the file
                using (var stream = File.Create(Filename))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write($"{this.Description}");
                    }
                }
                Console.WriteLine($"File created for {this.Description}");
            }

        }

        public async Task RollBack()
        {
            if (this.Rollback || this.Critical)
            {
                Console.Write($"Rolling back:{Description}");

                await Task.Delay((SleepPeriodInMilliseconds));
                Console.WriteLine($"Rolled back:{Description}");
                this.DeleteFile();
            }

        }

        public async Task<bool> Execute() { 
            if (Enabled)
            {
                
                try
                {
                    if (Description == "connect to payment gateway")
                    {
                        throw new Exception("cannot connect to payment engine");
                    }
                    Console.WriteLine($"Executing: {Description}");
                    await Task.Delay(SleepPeriodInMilliseconds);
                    Console.WriteLine($"Completed: {Description}");


                    this.CreateFolder();
                    this.createFile();
                    
                }
                catch {
                    this.RollBack();
                    return false;
                
                }
                
            }
            return true;
        } 
    }
}
