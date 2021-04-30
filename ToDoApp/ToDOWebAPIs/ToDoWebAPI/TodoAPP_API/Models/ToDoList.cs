using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPP_API.Models
{
    public class ToDoList
    {
    }

    /*
    getter setters of list data apis which is used to bind data receiveed from Requested body
     */
    public class ListRequestBody
    {
        public string xmlString { get; set; }
        public string type { get; set; }
    }

    /*
    getter setters of list data apis which is used to bind data receiveed from sql query
     */
    public class ListResponseBody
    {
        public string ToDoListName { get; set; }
    }
}
