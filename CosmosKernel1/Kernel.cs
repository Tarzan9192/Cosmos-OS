using System;
using System.Collections.Generic;
using Sys = Cosmos.System;
using System.Collections;

namespace CosmosKernel1
{
    public class Kernel : Sys.Kernel
    {
        private CosmosFileSystem fs;
        public static VariableStorage vs;
        protected override void BeforeRun()
        {
            //Console.WriteLine("Cosmos booted successfully. Type a line of text to get it echoed back.");
            Console.WriteLine("Cosmos booted succefully.");
            fs = new CosmosFileSystem();
            vs = new VariableStorage();
        }        
        

        protected override void Run()
        {                             

            //I'm going for a Unix shell feel.
            Console.WriteLine();
            Console.Write("$ ");
            string input = Console.ReadLine();

            //To buffer user commands
            Queue inputQueue = new Queue(input);
            
            while(inputQueue.getSize() > 0)
            {
                //Grab the first word in the queue.
                string command = inputQueue.poll();                
                switch (command)
                {
                    case "echo":
                        String token = inputQueue.poll();                        
                        Variable var = vs.getVariable(token);
                        if(var != null)
                        {
                            if(inputQueue.getSize() == 0)
                            {
                                Console.WriteLine(var.getValue());
                            }
                            else
                            {
                                Console.WriteLine(token + inputQueue.ToString());
                            }                          
                        }
                        else
                        {
                            Console.WriteLine(token + inputQueue.ToString());
                        }                        
                        inputQueue.clear();                                              
                        break;

                    case "dir":
                        fs.GetCurrentDir().ListContents();
                        break;
                    case "cd":
                        if (inputQueue.getSize() == 1)
                        {
                            String directory = inputQueue.poll();
                            Console.WriteLine(directory);
                           fs.ChangeDirectory(directory);
                        }
                        break;                       
                    case "create":
                        if(inputQueue.getSize() > 0)
                        {
                            String fileName = inputQueue.poll();   
                                
                            //To hold lines entered by user.                    
                            List<String> contents = new List<String>();
                            String consoleInput = "";

                            Console.Write("Enter Text: ");
                            Console.WriteLine("(Enter \"\\save\" to finish)");
                            consoleInput = Console.ReadLine();
                            while(consoleInput != "\\save")
                            {
                                contents.Add(consoleInput);
                                consoleInput = Console.ReadLine();
                            }
                            File newFile = new File(fileName, contents.ToArray());
                            fs.GetCurrentDir().AddFile(newFile);
                        }
                        break;
                    case "run":
                        if(inputQueue.getSize() == 2)
                        {
                            int times = 0;
                            String file = "";
                            try
                            {
                                times = Int32.Parse(inputQueue.poll());
                                file = inputQueue.poll();
                                fs.GetCurrentDir().GetFile(file);

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Please enter: run <number> <file>");
                                break;
                            }
                        }
                        break;
                    case "set":
                        if (inputQueue.getSize() == 3)
                        {
                            String variable = inputQueue.poll();

                            //get rid of '=' sign.
                            String assignmentOp = inputQueue.poll();
                            //if(assignmentOp != "=")
                            //{                                
                            //    break;
                            //}

                            //Get assignment value.
                            String value = inputQueue.poll();
                            //Console.WriteLine(value);
                            Variable newVar;

                            try
                            {
                                Boolean negative = false;
                                char[] number = value.ToCharArray();
                                if (number[0] == '-')
                                {
                                    negative = true;
                                    value = value.Substring(1);
                                }
                                int val = Int32.Parse(value);
                                if (negative == true)
                                {
                                    val = 0 - val;
                                }                                
                                newVar = new Variable(variable, val);
                                vs.Add(newVar);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                try
                                {
                                    //Check if user is attempting to assingn new variable
                                    //to a pre-existing variable.
                                    Variable temp = vs.getVariable(value);
                                    if (temp != null)
                                    {
                                        newVar = new Variable(variable, temp.getValue());
                                        vs.Add(newVar);
                                    }
                                }
                                catch (Exception ea)
                                {
                                    Console.WriteLine(ea.Message);
                                    Console.WriteLine("ERROR: " + value + " is not a known variable!");
                                }
                            }
                        }
                        break;
                    case "add":
                        if(inputQueue.getSize() == 3)
                        {
                            String token1 = inputQueue.poll();
                            String token2 = inputQueue.poll();
                            String variableName = inputQueue.poll();
                            int a = 0;
                            int b = 0;

                            if (vs.Contains(token1))
                            {
                                a = vs.getVariable(token1).getValue();
                            }
                            else
                            {
                                //try to parse an int
                                try
                                {
                                    a = Int32.Parse(token1);
                                }   
                                catch(Exception e)
                                {
                                    Console.WriteLine("Not a number");
                                    break;
                                }
                            }

                            if (vs.Contains(token2))
                            {
                                b = vs.getVariable(token2).getValue();
                            }
                            else
                            {
                                //try to parse int
                                try
                                {
                                    b = Int32.Parse(token2);
                                } 
                                catch(Exception ea)
                                {
                                    Console.WriteLine("Not a number");
                                    break;
                                }
                            }

                            int sum = a + b;
                            Variable newVar = new Variable(variableName, sum);
                            vs.Add(newVar);
                        }                        
                        break;

                    case "sub":
                        if (inputQueue.getSize() == 3)
                        {
                            String token1 = inputQueue.poll();
                            String token2 = inputQueue.poll();
                            String variableName = inputQueue.poll();
                            int a = 0;
                            int b = 0;

                            if (vs.Contains(token1))
                            {
                                a = vs.getVariable(token1).getValue();
                            }
                            else
                            {
                                //try to parse an int
                                try
                                {
                                    a = Int32.Parse(token1);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Not a number");
                                    break;
                                }
                            }

                            if (vs.Contains(token2))
                            {
                                b = vs.getVariable(token2).getValue();
                            }
                            else
                            {
                                //try to parse int
                                try
                                {
                                    b = Int32.Parse(token2);
                                }
                                catch (Exception ea)
                                {
                                    Console.WriteLine("Not a number");
                                    break;
                                }
                            }

                            int diff = a - b;
                            Variable newVar = new Variable(variableName, diff);
                            vs.Add(newVar);
                        }
                        break;

                    case "mul":
                        if (inputQueue.getSize() == 3)
                        {
                            String token1 = inputQueue.poll();
                            String token2 = inputQueue.poll();
                            String variableName = inputQueue.poll();
                            int a = 0;
                            int b = 0;

                            if (vs.Contains(token1))
                            {
                                a = vs.getVariable(token1).getValue();
                            }
                            else
                            {
                                //try to parse an int
                                try
                                {
                                    a = Int32.Parse(token1);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Not a number");
                                    break;
                                }
                            }

                            if (vs.Contains(token2))
                            {
                                b = vs.getVariable(token2).getValue();
                            }
                            else
                            {
                                //try to parse int
                                try
                                {
                                    b = Int32.Parse(token2);
                                }
                                catch (Exception ea)
                                {
                                    Console.WriteLine("Not a number");
                                    break;
                                }
                            }

                            int product = a * b;
                            Variable newVar = new Variable(variableName, product);
                            vs.Add(newVar);
                        }
                        break;

                    case "div":
                        if (inputQueue.getSize() == 3)
                        {
                            String token1 = inputQueue.poll();
                            String token2 = inputQueue.poll();
                            String variableName = inputQueue.poll();
                            int a = 0;
                            int b = 0;

                            if (vs.Contains(token1))
                            {
                                a = vs.getVariable(token1).getValue();
                            }
                            else
                            {
                                //try to parse an int
                                try
                                {
                                    a = Int32.Parse(token1);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Not a number");
                                    break;
                                }
                            }

                            if (vs.Contains(token2))
                            {
                                b = vs.getVariable(token2).getValue();
                            }
                            else
                            {
                                //try to parse int
                                try
                                {
                                    b = Int32.Parse(token2);
                                }
                                catch (Exception ea)
                                {
                                    Console.WriteLine("Not a number");
                                    break;
                                }
                            }
                            
                            int quotient = a / b;
                            if(b > a)
                            {
                                Console.WriteLine("Currenty can't calculate correct decimal.");
                                Console.WriteLine("(Check for yourself...)");
                            }
                            Variable newVar = new Variable(variableName, quotient);
                            vs.Add(newVar);
                        }
                        break;
                    case "print":
                        if(inputQueue.getSize() > 0)
                        {
                            Variable v = vs.getVariable(inputQueue.poll());
                            if(v != null)
                            {
                                Console.WriteLine(v.getValue());
                            }
                            else
                            {
                                Console.WriteLine("No such variable.");
                            }                            
                        }
                        break;
                    default:  
                        if(command != "")
                        {
                            Console.WriteLine("Not a valid command.");
                        }                        
                        inputQueue.clear();
                        break;
                }
            }
                        
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
