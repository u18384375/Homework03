using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homework03.Models
{
    public class HomeViewModel
    {
        public PagedList.IPagedList<student> StudentList { get; set; }
        public PagedList.IPagedList<book> BookList { get; set; }
        public student students { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}