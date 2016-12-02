using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosKernel1
{
    public class Variable
    {
        private String name;
        private int value;

        public Variable(String name, int value)
        {
            this.name = name;
            this.value = value;
        }

        public void setValue(int value)
        {
            this.value = value;
        }

        public int getValue()
        {
            return  this.value;
        }

        public String getName()
        {
            return this.name;
        }

        public override string ToString()
        {
            return "" + this.getValue();
        }

    }
}
