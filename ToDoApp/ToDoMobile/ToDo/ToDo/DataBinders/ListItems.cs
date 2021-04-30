using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.DataBinders
{
    //create getter setter class to bind list item data 
    public class ListItems
    {
        public string ToDoListItem { get; set; }
        public int priority { get; set; }
        public int ToDoListId { get; set; }
    }

    //create getter setter class to bind list 
    public class MyToDoList
    {
        public string ToDoListName { get; set; }
    }
}
