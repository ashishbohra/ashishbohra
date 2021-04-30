using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.Constraints
{
    //create class to use single object multiple time
    public class Contraints
    {
        //API base URL
        public static string API_BaseURL = "http://192.168.20.2:45455";

        /*
         post.json // if want api response in json post type api
        post.xml // if want api response in xml of post type api
         */
        //Get/Create/Delete List API URL
        public static string GetListURL_JSON = "/api/ToDoList/post.json";
        public static string GetListURL_XML = "/api/ToDoList/post.xml";

        //Get/Create/Delete List Item API URL
        public static string GetListITEMURL_JSON = "/api/TodolistItem/post.json";
        public static string GetListITEMURL_XML = "/api/TodolistItem/post.xml";

    }
}
