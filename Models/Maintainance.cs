using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework03.Models
{
    public class Maintainance
    {
        public List<author> AuthorsList { get; set; }
        public List<borrow> BorrowsList { get; set; }
        public List<type> TypesList { get; set; }
       
    }
}