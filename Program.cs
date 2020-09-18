using System;
using System.IO;
using System.Collections;

public struct ReadResult
{
    public ArrayList toInstall;
    public ArrayList installed;
}

public struct Package
{
    public string name;
    public string version;
}

namespace dependencies
{

    public class Functions
    {
        public static bool isFileExist(string path)
        {
            try
            {
                File.Exists(path);
            }
            catch (FileNotFoundException ex)
            {
                System.Console.WriteLine("File " + path + " not found ");
            }

            return File.Exists(path);
        }
        public static bool isOneArgument(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a path to file as an argument");
                return false;
            }

            if (args.Length > 1)
            {
                Console.WriteLine("Too many arguments");
                return false;
            }

            return true;
        }
        public static ReadResult Read(string path)
        {
            ArrayList dependenciesToInstall = new ArrayList();
            ArrayList dependenciesInstalled = new ArrayList();
            StreamReader file = new StreamReader(path);

            string N = file.ReadLine();
            for (int i = 0; i < Convert.ToInt32(N); i++)
            {
                string line = file.ReadLine();
                string[] array = line.Split(",");

                for (int j = 0; j < array.Length /2 ; j++)
                {
                    string name = array[j];
                    string version = array[j + 1];
                    Package package = new Package();
                    package.name = name;
                    package.version = version;

                    dependenciesToInstall.Add(package);
                }

            }

            string M = file.ReadLine();
            // TODO reduce duplicate code
            for (int i = 0; i < Convert.ToInt32(M); i++)
            {
                string line = file.ReadLine();
                string[] array = line.Split(",");

                for (int j = 0; j < array.Length /2 ; j++)
                {
                    string name = array[j];
                    string version = array[j + 1];

                    Package package = new Package();
                    package.name = name;
                    package.version = version;

                    dependenciesInstalled.Add(package);
                }

            }

            file.Close();
            ReadResult result = new ReadResult
            {
                toInstall = dependenciesToInstall,
                installed = dependenciesInstalled
            };

            return result;
        }

        public static bool Resolve(ReadResult arg)
        {
            ArrayList toInstall = arg.toInstall;
            ArrayList installed = arg.installed;

            if (installed.Contains(toInstall[0]))
            {
                Object nextPackageToInstall;
                Object nextPackageInstalled;
                try
                {
                    nextPackageToInstall = toInstall[1];
                }
                catch(ArgumentOutOfRangeException outOfRange)
                {
                    return true;
                }
                try
                {
                    int index = installed.IndexOf(toInstall[0]);
                    nextPackageInstalled = installed[index + 1];
                }
                catch (ArgumentOutOfRangeException outOfRange)
                {
                    return false;
                }
                return nextPackageToInstall.Equals(nextPackageInstalled);
            }

            return true;
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            if (Functions.isOneArgument(args) && Functions.isFileExist(args[0]))
            {
                ReadResult data = Functions.Read(args[0]);
                Boolean result = Functions.Resolve(data);

                System.Console.WriteLine(result);
                // TODO check result with output file
            }
        }
    }
}
