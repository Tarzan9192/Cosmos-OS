using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosKernel1
{
    public class VariableStorage
    {
        private List<Variable> vs;

        public VariableStorage()
        {
            vs = new List<Variable>();

        }

        /// <summary>
        /// This method adds a new variable to variable storage.
        /// If variable is found, it replaces the value of the
        /// already stored variable with that of the new variable.
        /// </summary>
        /// <param name="newVar"></param>
        public void Add(Variable newVar)
        {
            //If there are no variables in memory.
            if(vs.Count == 0)
            {
                vs.Add(newVar);
            }
            else
            {
                //Check if variable is already in memory and reassign.
                for(int i = 0; i < vs.Count; i++)
                {
                    if (vs[i].getName() == newVar.getName())
                    {
                        vs[i].setValue(newVar.getValue());
                        return;
                    }
                }                
                //If not found in memory already, add to memory.
                vs.Add(newVar);
            }
        }

        public void Remove(String name)
        {
            
        }

        /// <summary>
        /// This method returns a variable with the same name
        /// as the parameter. Null if not found.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public Variable getVariable(String variable)
        {
            Variable returnVar = null;
            for(int i = 0; i < vs.Count; i++)
            {
                if (vs[i].getName() == variable)
                {
                    returnVar = vs[i];
                }
            }            

            return returnVar;
        }

        /// <summary>
        /// Searches the program memory for variable by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Boolean Contains(String name)
        {

            for(int i = 0; i < vs.Count; i++)
            {
                if (vs[i].getName() == name)
                {
                    return true;
                }
            }            

            //Indicates variable was not found in memory.
            return false;
        }
    }
}
