using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CosmosKernel1
{
    public class CosmosFileSystem
    {
        private Directory root;
        private Directory currentDir;
        //private List<Directory> children;

        public CosmosFileSystem()
        {
            root = new Directory("/home");
            root.AddDir("/Documents");            
            currentDir = root;            
        }

        public Directory GetCurrentDir()
        {
            return currentDir;
        }
        

        //For adding a file to a specific path.
        public void CreateFile(String name)
        {
            currentDir.AddDir(name);
        }

        //For adding a directory to a specific path.
        public void CreateDirectory(String name)
        {
            currentDir.AddDir(name);
        }

        //Change currentDir to specified destination dir.
        public void ChangeDirectory(String destination)
        {
            //Go up one directory
            if (destination == "..")
            {
                currentDir = currentDir.GetParent();
                return;
            }

            Directory destDir = currentDir.GetDirectory(destination);            
            if(destDir != null)
            {
                currentDir = destDir;
            }
            else
            {
                Console.WriteLine("Directory does not exist.");
            }
        }



    }

    public class Directory
    {
        private String name;
        private Directory parent;
        private List<Directory> children;
        private List<File> fileList;

        public Directory()
        {
            parent = null;
            children = new List<Directory>();
            fileList = new List<File>();
            name = "/home";
        }

        public Directory(String dirName)
        {
            parent = null;
            name = dirName;
            children = new List<Directory>();
            fileList = new List<File>();
        }

        public void SetParent(Directory d)
        {
            parent = d;
        }

        public Directory GetParent()
        {
            return parent;
        }

        public void AddDir(String dir)
        {
            Directory newDir = new Directory(dir);  //Create Directory object with name <dir>.
            newDir.parent = this;                   //Make link subdirectory to this directory.
            children.Add(newDir);
        }

        /// <summary>
        /// Add a file to the directory's file list.
        /// </summary>
        /// <param name="file"></param>
        public void AddFile(File file)
        {
            fileList.Add(file);            
        }

        /// <summary>
        /// Searches for, and returns a file in the current directory,
        /// returns null if file is not found.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public File GetFile(String file)
        {
            File target = null;

            for(int i = 0; i < fileList.Count; i++)
            {
                if(fileList[i].name == file)
                {
                    target = fileList[i];
                }
            }

            return target;
        }

        /// <summary>
        /// Returns a specified directory if located in list of
        /// children.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Directory GetDirectory(String name)
        {            
            Directory target = null;
            for(int i = 0; i < children.Count; i++)
            {
                if(children[i].name == name)
                {
                    target = children[i];
                    break;
                }
            }                  

            return target;                          
        }

        /// <summary>
        /// Returns the list of children for this directory.
        /// </summary>
        /// <returns></returns>
        public List<Directory> GetDirs()
        {
            return children;
        }

        //Returns the names of directories in String format.
        public List<String> GetDirNames()
        {
            List<String> list = new List<String>();
            foreach(Directory d in children)
            {
                list.Add(d.name);
            }
            list.Sort();

            return list;
        }

        /// <summary>
        /// This method will list the files and directories 
        /// stored in the current directory.
        /// </summary>
        public void ListContents()
        {
            Console.WriteLine("Contents\t\tFile Size");
            Console.WriteLine("********\t\t*********");
            for(int i = 0; i < children.Count; i++)
            {
                Console.WriteLine(((Directory)children[i]).name);                
            }
            for(int i = 0; i < fileList.Count; i++)
            {                             
                Console.Write(fileList[i].fullName);
                Console.WriteLine("\t\t" + fileList[i].getSize() + "Bytes");
            }
        }

    }
    
}
