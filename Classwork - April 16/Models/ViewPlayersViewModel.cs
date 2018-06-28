using Classwork___April_16.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Classwork___April_16.Models
{
    public class ViewPlayersViewModel
    {
        public Event Event { get; set; }
        public IEnumerable<Player> Players { get; set; }
    }
}