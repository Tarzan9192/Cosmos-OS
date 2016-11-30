using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CosmosKernel1
{
    [Serializable]
    public class File
    {
        public String fullName;
        public String name;
        private String[] contents;
        public String ext;
        private String dateCreated;        
        
        public File(String name, String[] contents)
        {
            String[] s = name.Split('.');
            this.name = s[0];
            ext = s[1];
            this.contents = contents;
            fullName = name;
            //dateCreated = Hardware.Time.getDate();
                     
        }
        
        /// <summary>
        /// Prints the contents for the file to Console.
        /// </summary>
        public void PrintContents()
        {
            foreach(String s in contents)
            {
                Console.WriteLine(s);
            }
        }

        /// <summary>
        /// Returns the size of the file in bytes.
        /// </summary>
        /// <returns></returns>
        public long getSize()
        {
            long size = 0;
            int sizeOfCharInBytes = sizeof(char);
            foreach(String s in contents)
            {
                foreach(char c in s)
                {
                    size += sizeOfCharInBytes;
                }
            }

            return size;
        }
    }
}
