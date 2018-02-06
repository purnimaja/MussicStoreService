using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MussicStoreService.Models
{
    public class Album
    {
        public int albumid { get; set; }
        public string albumname { get; set; }
        public Genere generedetails { get; set; }
        public Artist artistdetails { get; set; }

       
    }
}