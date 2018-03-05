using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4_Files
{
    class CustomConsole
    {
        string path;
        

        public void ReadCommand()
        {

            Console.Write("Enter directory path: ");
            path = Console.ReadLine();
            DirectoryInfo di = new DirectoryInfo(@path);
            while (!di.Exists)
            {
                Console.WriteLine("Error: path doesn't exist!");
                Console.Write("Enter valid path: ");
                path = Console.ReadLine();
                di = new DirectoryInfo(@path);
            }

            
            
            while (true)
            {
                Console.Write(path + ">");
                string input = Console.ReadLine();

                if (input.StartsWith("drives"))
                {
                    DriveInfo[] driveIndo = DriveInfo.GetDrives();
                    Console.WriteLine("Name " + "Total Size (bytes)  " + "Free Space (bytes)");
                    foreach (DriveInfo drive in driveIndo)
                    {
                        if (drive.IsReady)
                        {
                            Console.WriteLine(drive + " " + drive.TotalSize + " bytes " + drive.TotalFreeSpace + " bytes  ");
                        }

                    }
                } else if (input.StartsWith("dir"))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(@path);
                    foreach (DirectoryInfo file in directoryInfo.GetDirectories())
                    {

                        Console.WriteLine(file + "  Creation time: " + file.CreationTime);

                    }
                    foreach (FileInfo file in directoryInfo.GetFiles())
                    {
                        Console.WriteLine(file + "  Creation Time: " + file.CreationTime + "  Size: " + file.Length);
                    }
                } else if (input.StartsWith("cd"))
                {
                    string addPath = input.Substring(3);
                    di = new DirectoryInfo(@path + addPath);
                    if (di.Exists)
                    {
                        path += addPath;
                    }
                    else
                    {
                        Console.WriteLine(path + "> invalid path");
                    }
                } else if (input.StartsWith("exit"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine(path + ">invalid command");
                }
            }
        }
    }
}
