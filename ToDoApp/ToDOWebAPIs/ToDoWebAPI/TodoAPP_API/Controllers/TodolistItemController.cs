using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TodoAPP_API.Helpers;
using TodoAPP_API.Models;

namespace TodoAPP_API.Controllers
{
    // define route of api call after base url
    [Route("api/[controller]")]
    [ApiController]
    public class TodolistItemController : ControllerBase
    {
        // create of response class of list type
        List<ListItemResponseBody> _ToDoList = new List<ListItemResponseBody>();

        // create object of confiruration interface
        public IConfiguration Configuration { get; }
        string ToDoDB_constr = string.Empty;

        // call counstructior of type configuration to read appsettings.json properties
        public TodolistItemController(IConfiguration config)
        {
            // define configuration object and read sql server connection string from appsettings.json
            Configuration = config;
            ToDoDB_constr = this.Configuration.GetConnectionString("ToDodataConnection");
        }


        // POST api/<ToDoListControllercs>
        //{format}is used to define api response type  exe json or xml
        // Create  method of List response type
        [HttpPost("post.{format}"), FormatFilter]
        public List<ListItemResponseBody> Post([FromBody] ListItemRequestBOdy doList)
        {
            // check and validate request state of getlist data api 
            if (ModelState.IsValid)
            {
                // read listdata  dataset received from sql server with help of DB helper classs
                DataSet MyList = DBHelper.GetOrAddListItemToDoDB(doList, "CreateAndGetToDoListItemByName", ToDoDB_constr);

                // bind listdata dataset to response body
                if (MyList != null && MyList.Tables.Count > 0)
                {
                    foreach (DataRow dr in MyList.Tables[0].Rows)
                    {
                        _ToDoList.Add(new ListItemResponseBody { ToDoListId =Convert.ToInt32(Convert.ToString(dr["ToDoListId"])),ToDoListItem = Convert.ToString(dr["ToDoListItem"]), priority = Convert.ToString(dr["priority"]) });
                    };
                }

                // return list data json/xml on success
                return _ToDoList;
            }
            // return fail response
            return null;
        }
    }
}
