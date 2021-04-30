using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPP_API.Models
{
    public class ToDoListItem
    {
    }
    /*
     getter setters of list item apis which is used to bind data receiveed from Requested body
      */
    public class ListItemRequestBOdy
    {
        public string xmlString { get; set; }
        public string type { get; set; }
        public string ListName { get; set; }
    }


    /*
     getter setters of list item apis which is used to bind data receiveed from sql query
      */
    public class ListItemResponseBody
    {
        public string ToDoListItem { get; set; }
        public string priority { get; set; }
        public int ToDoListId { get; set; }
    }
}
