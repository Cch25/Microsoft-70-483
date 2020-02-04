using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Files
{
    public class FilesExample
    {
        public void FileStream() => new Streams().FileStreamExmpl();
        public void FileStreamImprove() => new Streams().FileStreamImprove();
        public void StreamWriterAndReader() => new Streams().StreamWriterAndReader();
        public void ChainStreams() => new Streams().ChainStreams();
        public void FileHelperClass() => new FileHelper().MakeUseOfFileClass();
        public void ExceptionHandling() => new HandlingStreamExceptions().HandleExceptionForFiles();
        public void FileStorage() => new FilesStorage().FileStorage();
        public void FileInfo() => new FilesStorage().FileInformation();
        public void Directory() => new FilesStorage().DirectoryAndDirectoryInfo();
        public void SearchFiles() => new FilesStorage().SearchingForFiles();

    }
    public class Streams
    {
        /// <summary>
        /// FileMode.Append Open a file for appending to the end. If the file exists, move the
        /// seek position to the end of this file.If the file does not exist; create it.This mode can only
        /// be used if the file is being opened for writing.
        /// FileMode.Create Create a file for writing.If the file already exists, it is overwritten.
        /// Note that this means the existing contents of the file are lost.
        /// FileMode.CreateNew Create a file for writing.If the file already exists, an exception is
        /// thrown.
        /// FileMode.Open Open an existing file.An exception is thrown if the file does not exist.
        /// This mode can be used for reading or writing.
        /// FileMode.OpenOrCreate Open a file for reading or writing.If the file does not exist,
        /// an empty file is created.This mode can be used for reading or writing.
        /// FileMode.Truncate Open a file for writing and remove any existing contents
        /// </summary>
        public void FileStreamExmpl()
        {
            FileStream fs = new FileStream("Output.txt", FileMode.OpenOrCreate, FileAccess.Write);
            string outputMessage = "Hello world";
            byte[] outputBytesMessage = Encoding.UTF8.GetBytes(outputMessage);
            fs.Write(outputBytesMessage, 0, outputBytesMessage.Length);
            fs.Close();

            FileStream inputStream = new FileStream("Output.txt", FileMode.Open, FileAccess.Read);
            long fileLength = inputStream.Length;
            byte[] readBytes = new byte[fileLength];
            inputStream.Read(readBytes, 0, (int)fileLength);
            string readString = Encoding.UTF8.GetString(readBytes);
            inputStream.Close();
            Console.WriteLine($"Read message {readString}");
        }
        public void FileStreamImprove()
        {
            using (FileStream fs = new FileStream("Output.txt", FileMode.OpenOrCreate, FileAccess.Write))
            {
                string outputMessage = "Hi there. I'm disposable :)";
                byte[] array = Encoding.UTF8.GetBytes(outputMessage);
                fs.Write(array, 0, array.Length);
                //there's no need to close the stream.
            }
            using (FileStream fs = new FileStream("Output.txt", FileMode.Open, FileAccess.Read))
            {
                byte[] array = new byte[fs.Length];
                fs.Read(array, 0, array.Length);
                string outputMessage = Encoding.UTF8.GetString(array);
                Console.WriteLine($"Newly added message\n{outputMessage}.");
            }
        }
        /// <summary>
        /// StreamWriter and StreamReader extend TextWriter and TextReader
        /// </summary>
        public void StreamWriterAndReader()
        {
            using (StreamWriter sw = new StreamWriter("Output.txt"))
            {
                sw.Write("Hello from the StreamWriter");
            }

            using (StreamReader sr = new StreamReader("Output.txt"))
            {
                Console.WriteLine(sr.ReadToEnd());
            }
        }

        public void ChainStreams()
        {
            using (FileStream writeFile = new FileStream("Compress.zip", FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (GZipStream zip = new GZipStream(writeFile, CompressionLevel.Fastest))
                {
                    using (StreamWriter sw = new StreamWriter(zip))
                    {
                        sw.Write("Hi there, I'm an archived text");
                    }
                }
            }

            using (FileStream readFile = new FileStream("Compress.zip", FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (GZipStream zip = new GZipStream(readFile, CompressionMode.Decompress))
                {
                    using (StreamReader sr = new StreamReader(zip))
                    {
                        Console.WriteLine($"{sr.ReadToEnd()}");
                    }
                }
            }
        }

    }
    public class FileHelper
    {
        public void MakeUseOfFileClass()
        {
            File.WriteAllText(path: "TextFile.txt", contents: "This text goes in the file");
            File.AppendAllText(path: "TextFile.txt", contents: " - This goes on the end");
            if (File.Exists("TextFile.txt"))
            {
                Console.WriteLine("Text File exists");
            }
            string contents = File.ReadAllText(path: "TextFile.txt");
            Console.WriteLine($"File contents: {contents}");

            File.Copy("TextFile.txt", "CopyTextFile.txt");
            using (TextReader tr = File.OpenText("CopyTextFile.txt"))
            {
                Console.WriteLine($"{tr.ReadToEnd()}");
            }
        }
    }
    public class HandlingStreamExceptions
    {
        public void HandleExceptionForFiles()
        {
            try
            {
                File.ReadAllText("txt.txt");
            }
            catch (FileNotFoundException fnfe)
            {
                Console.WriteLine(fnfe.Message);
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    public class FilesStorage
    {
        public void FileStorage()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                Console.WriteLine($"Name: {drive.Name}");
                if (drive.IsReady)
                {
                    Console.WriteLine($"Type {drive.DriveType}\nFormat {drive.DriveFormat}\nFree space {drive.AvailableFreeSpace / (1024 * 1024 * 1024)}GB");
                    Console.WriteLine("==========");
                }
                else
                {
                    Console.WriteLine("Drive not ready");
                }
            }
        }
        public void FileInformation()
        {
            string path = "Output.txt";
            File.WriteAllText(path, "Hi how are you?");
            FileInfo info = new FileInfo(path);
            Console.WriteLine($"Name: {info.Name}");
            Console.WriteLine($"Full path: {info.FullName}");
            Console.WriteLine($"Last acces {info.LastAccessTime}");
            Console.WriteLine($"Length {info.Length}");
            Console.WriteLine($"Attributes {info.Attributes}");
            Console.WriteLine($"Make the file readonly");
            info.Attributes |= FileAttributes.ReadOnly;
            Console.WriteLine($"Attributes {info.Attributes}");
            Console.WriteLine($"Remove readonly attribute");
            info.Attributes &= ~FileAttributes.ReadOnly;

            Console.WriteLine($"Attributes {info.Attributes}");

        }
        public void DirectoryAndDirectoryInfo()
        {
            Directory.CreateDirectory("MyFolder");
            if (Directory.Exists("MyFolder"))
            {
                Console.WriteLine("Directory MyFolder successfully created! (directory)");
            }
            Directory.Delete("MyFolder");
            Console.WriteLine("Directory sucessfully deleted");

            DirectoryInfo info = new DirectoryInfo("MyFolder");
            info.Create();
            if (info.Exists)
            {
                Console.WriteLine("Directory MyFolder sucesfully created (directory info)");
            }
            info.Delete();
            Console.WriteLine("Directory sucessfully deleted");
        }
        public void SearchingForFiles()
        {
            DirectoryInfo directory = new DirectoryInfo(@"D:\Filme");
            FindFiles(directory, "*");
        }
        private void FindFiles(DirectoryInfo directoryInfo, string pattern)
        {
            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
            {
                FindFiles(directory, pattern);
            }
            FileInfo[] filesInfo = directoryInfo.GetFiles();
            foreach (FileInfo fi in filesInfo)
            {
                Console.WriteLine($"{fi.Name}");
            }
        }


    }
}
