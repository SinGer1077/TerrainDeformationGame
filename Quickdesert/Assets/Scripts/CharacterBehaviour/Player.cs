using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class Player
    {        
        public int HP { get; set; }        
        public IPRojectile CurrentWeapon { get; set; }      


        public Player()
        {            
            HP = 100;            
        }
    }

