using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosKernel1
{
    public class Commands
    {
        private Queue inputQueue;

        public void ParseInput(String input)
        {
            inputQueue = new Queue(input);
            String command = inputQueue.poll();

            switch (command)
            {
                case "echo":
                    Echo();
                    break;
                case "dir":
                    Dir();
                    break;
                case "mkdir":
                    MkDir();
                    break;
                case "deldir":
                    DelDir();
                    break;
                case "delfile":
                    DelFile();
                    break;
                case "cd":
                    Cd();
                    break;
                case "create":
                    Create();
                    break;
                case "run":
                    Run();
                    break;
                case "set":
                    Set();
                    break;
                case "add":
                    Add();
                    break;
                case "sub":
                    Sub();
                    break;
                case "mul":
                    Mul();
                    break;
                case "div":
                    Div();
                    break;
                case "print":
                    Print();
                    break;
                case "open":
                    Open();
                    break;
                default:
                    if (command != "")
                    {
                        Console.WriteLine("Not a valid command.");
                    }
                    inputQueue.clear();
                    break;
            }

        }

        private void DelFile()
        {
            if(inputQueue.getSize() == 1)
            {
                String token = inputQueue.poll();
                File f = Kernel.fs.GetCurrentDir().GetFile(token);
                if (f != null)
                {
                    Kernel.fs.GetCurrentDir().DelFile(f);
                }
                else
                {
                    Console.WriteLine("File not found in " + Kernel.fs.GetCurrentDir().GetName);
                }
            }
        }

        private void DelDir()
        {
            if(inputQueue.getSize() == 1)
            {                
                String token = inputQueue.poll();
                Directory d = Kernel.fs.GetCurrentDir().GetDirectory(token);
                if(d != null)
                {
                    d.GetParent().DelDir(token);
                }
                else
                {
                    Console.WriteLine("Directory not found in " + Kernel.fs.GetCurrentDir().GetName);
                }
            }
        }

        /// <summary>
        /// Creates a directory in the current directory the
        /// user is in.
        /// </summary>
        private void MkDir()
        {
            if(inputQueue.getSize() == 1)
            {
                String token = inputQueue.poll();                
                if(token.ToCharArray()[0] != '/')
                {
                    Kernel.fs.GetCurrentDir().AddDir("/" + token.Trim());
                }
                else
                {
                    Kernel.fs.GetCurrentDir().AddDir(token.Trim());
                }                
            }
        }

        private void Echo()
        {            
            while(inputQueue.getSize() > 0)
            {
                String token = inputQueue.poll();
                Variable v = Kernel.vs.getVariable(token);
                if(v != null)
                {
                    Console.Write(v + " ");
                }
                else
                {
                    Console.Write(token + " ");
                }
            }
            Console.WriteLine();                    
            //Variable var = Kernel.vs.getVariable(token);
            //if (var != null)
            //{
            //    if (inputQueue.getSize() == 0)
            //    {
            //        Console.WriteLine(var.getValue());
            //    }
            //    else
            //    {
            //        Console.WriteLine(token + inputQueue.ToString());
            //    }
            //}
            //else
            //{
            //    Console.WriteLine(token + inputQueue.ToString());
            //}
            //inputQueue.clear();
        }

        private void Dir()
        {
            Kernel.fs.GetCurrentDir().ListContents();
        }

        private void Cd()
        {
            if (inputQueue.getSize() == 1)
            {
                String directory = inputQueue.poll();
                //Console.WriteLine(directory);
                Kernel.fs.ChangeDirectory(directory);
            }
        }

        private void Create()
        {
            if (inputQueue.getSize() > 0)
            {
                String fileName = inputQueue.poll();

                //To hold lines entered by user.                    
                List<String> contents = new List<String>();
                String consoleInput = "";

                Console.Write("Enter Text: ");
                Console.WriteLine("(Enter \"\\save\" to finish)");
                consoleInput = Console.ReadLine();
                while (consoleInput != "\\save")
                {
                    contents.Add(consoleInput);
                    consoleInput = Console.ReadLine();
                }
                File newFile = new File(fileName, contents.ToArray());
                Kernel.fs.GetCurrentDir().AddFile(newFile);
            }
        }

        private void Run()
        {            
            if (inputQueue.getSize() == 2)
            {
                int times = 0;
                String file = "";
                try
                {
                    times = Int32.Parse(inputQueue.poll());
                    file = inputQueue.poll();
                    File f = Kernel.fs.GetCurrentDir().GetFile(file);
                    if (f != null && f.ext == "bat")
                    {
                        for (int i = 0; i < times; i++)
                        {
                            for (int j = 0; j < f.contents.Length; j++)
                            {
                                ParseInput(f.contents[j]);
                            }
                        }
                    }                                       
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please enter: run <number> <file>");                    
                }
            }
        }

        private void Set()
        {
            if (inputQueue.getSize() == 3)
            {
                String variable = inputQueue.poll();

                //get rid of '=' sign.
                String assignmentOp = inputQueue.poll();               

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
                    Kernel.vs.Add(newVar);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    try
                    {
                        //Check if user is attempting to assingn new variable
                        //to a pre-existing variable.
                        Variable temp = Kernel.vs.getVariable(value);
                        if (temp != null)
                        {
                            newVar = new Variable(variable, temp.getValue());
                            Kernel.vs.Add(newVar);
                        }
                    }
                    catch (Exception ea)
                    {
                        Console.WriteLine(ea.Message);
                        Console.WriteLine("ERROR: " + value + " is not a known variable!");
                    }
                }
            }
        }

        private void Add()
        {
            if (inputQueue.getSize() == 3)
            {
                String token1 = inputQueue.poll();
                String token2 = inputQueue.poll();
                String variableName = inputQueue.poll();
                int a = 0;
                int b = 0;

                if (Kernel.vs.Contains(token1))
                {
                    a = Kernel.vs.getVariable(token1).getValue();
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
                        return;
                    }
                }

                if (Kernel.vs.Contains(token2))
                {
                    b = Kernel.vs.getVariable(token2).getValue();
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
                        return;
                    }
                }

                int sum = a + b;
                Variable newVar = new Variable(variableName, sum);
                Kernel.vs.Add(newVar);
            }
        }

        private void Sub()
        {
            if (inputQueue.getSize() == 3)
            {
                String token1 = inputQueue.poll();
                String token2 = inputQueue.poll();
                String variableName = inputQueue.poll();
                int a = 0;
                int b = 0;

                if (Kernel.vs.Contains(token1))
                {
                    a = Kernel.vs.getVariable(token1).getValue();
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
                        return;
                    }
                }

                if (Kernel.vs.Contains(token2))
                {
                    b = Kernel.vs.getVariable(token2).getValue();
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
                        return;
                    }
                }

                int diff = a - b;
                Variable newVar = new Variable(variableName, diff);
                Kernel.vs.Add(newVar);
            }
        }

        private void Mul()
        {
            if (inputQueue.getSize() == 3)
            {
                String token1 = inputQueue.poll();
                String token2 = inputQueue.poll();
                String variableName = inputQueue.poll();
                int a = 0;
                int b = 0;

                if (Kernel.vs.Contains(token1))
                {
                    a = Kernel.vs.getVariable(token1).getValue();
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
                        return;
                    }
                }

                if (Kernel.vs.Contains(token2))
                {
                    b = Kernel.vs.getVariable(token2).getValue();
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
                        return;
                    }
                }

                int product = a * b;
                Variable newVar = new Variable(variableName, product);
                Kernel.vs.Add(newVar);
            }
        }

        private void Div()
        {
            if (inputQueue.getSize() == 3)
            {
                String token1 = inputQueue.poll();
                String token2 = inputQueue.poll();
                String variableName = inputQueue.poll();
                int a = 0;
                int b = 0;

                if (Kernel.vs.Contains(token1))
                {
                    a = Kernel.vs.getVariable(token1).getValue();
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
                        return;
                    }
                }

                if (Kernel.vs.Contains(token2))
                {
                    b = Kernel.vs.getVariable(token2).getValue();
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
                        return;
                    }
                }

                int quotient = a / b;
                if (b > a)
                {
                    Console.WriteLine("Currenty can't calculate correct decimal.");
                    Console.WriteLine("(Check for yourself...)");
                }
                Variable newVar = new Variable(variableName, quotient);
                Kernel.vs.Add(newVar);
            }
        }

        private void Print()
        {
            if (inputQueue.getSize() > 0)
            {
                Variable v = Kernel.vs.getVariable(inputQueue.poll());
                if (v != null)
                {
                    Console.WriteLine(v.getValue());
                }
                else
                {
                    Console.WriteLine("No such variable.");
                }
            }
        }

        private void Open()
        {
            if(inputQueue.getSize() == 1)
            {
                String file = inputQueue.poll();
                Kernel.fs.GetCurrentDir().GetFile(file).PrintContents();
            }
        }
    }
}
