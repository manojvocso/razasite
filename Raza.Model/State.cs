using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raza.Model
{
   public class State
    {

        public string Name { get; set; }
        public string Id { get; set; }
        
        public State()
        {

        }
        public State(string name, string id)
        {

            Name = name;
            Id = id;
            
        }


    }
}
