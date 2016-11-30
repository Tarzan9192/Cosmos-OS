using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosKernel1
{
    class Commands
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
                default:
                    if (command != "")
                    {
                        Console.WriteLine("Not a valid command.");
                    }
                    inputQueue.clear();
                    break;
            }

        }

        private void Echo()
        {
            String token = inputQueue.poll();
            Variable var = Kernel.vs.getVariable(token);
            if (var != null)
            {
                if (inputQueue.getSize() == 0)
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
        }

        private void Dir()
        {

        }

        private void Cd()
        {

        }

        private void Create()
        {

        }

        private void Run()
        {

        }

        private void Set()
        {

        }

        private void Add()
        {

        }

        private void Sub()
        {
            
        }

        private void Mul()
        {

        }

        private void Div()
        {

        }

        private void Print()
        {

        }
    }
}
