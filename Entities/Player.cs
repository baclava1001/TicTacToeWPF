using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeWPF.Entities
{
    public class Player
    {
        public string Name { get; set; }
        public string Mark { get; set; }
        public bool FirstTurn { get; set; }
    }
}
