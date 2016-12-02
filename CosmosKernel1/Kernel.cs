using System;
using System.Collections.Generic;
using Sys = Cosmos.System;
using System.Collections;

namespace CosmosKernel1
{
    public class Kernel : Sys.Kernel
    {
        public static CosmosFileSystem fs;
        public static VariableStorage vs;
        public Commands commandParser;
        protected override void BeforeRun()
        {
            //Console.WriteLine("Cosmos booted successfully. Type a line of text to get it echoed back.");
            Console.WriteLine("Cosmos booted succefully.");
            fs = new CosmosFileSystem();
            vs = new VariableStorage();
            commandParser = new Commands();
        }


        protected override void Run()
        {

            //I'm going for a Unix shell feel.
            Console.WriteLine();
            Console.WriteLine("Guest ~" + fs.GetCurrentDir().GetDirPath());
            Console.Write("$ ");
            string input = Console.ReadLine();

            commandParser.ParseInput(input);            
        }
    }

   
    //This class creates a queue of string objects.
    public class Queue
    {
        ArrayList queue;

        public Queue()
        {
            queue = new ArrayList();
        }

        //Initializes the queue with given string.
        public Queue(string str)
        {
            String[] input = str.Split(' ');
            queue = new ArrayList();
            foreach(string s in input)
            {
                queue.Add(s);
            }
        }

        //Removes first string from the queue and 
        //returns that string. Returns null if queue
        //is empty.
        public string poll()
        {
            if(queue.Count > 0)
            {
                string returnVal = (string)queue[0];
                queue.RemoveAt(0);
                return returnVal;
            }
            else
                return null;
        }

        //Adds a string to the queue.
        public void put(String str)
        {
            queue.Add(str);
        }

        //Returns the number of elements in the queue.
        public int getSize()
        {
            return queue.Count;
        }

        //Removes all elements from the queue.
        public void clear()
        {
            queue.Clear();
        }

        //Concatenates each queue element into a single
        //string and returns that string.
        public override string ToString()
        {
            string returnVal = "";
            for(int i = 0; i < queue.Count; i++)
            {
                returnVal = returnVal + " " + queue[i];
            }

            return returnVal;
        }
    }
}
