using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeWPF.Entities
{
    class CpuAI
    {
        public string Name { get; } = nameof(CpuAI);
        public MarkType Mark;
        public int Score { get; set; }
    }
}
