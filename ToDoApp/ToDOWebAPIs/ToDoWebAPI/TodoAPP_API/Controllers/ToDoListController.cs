
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TodoAPP_API.Helpers;
using TodoAPP_API.Models;

namespace TodoAPP_API.Controllers
{
    // define route of api call after base url
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        // create of response class of list type
        List<ListResponseBody> _ToDoList = new List<ListResponseBody>();

        // create object of confiruration interface
        public IConfiguration Configuration { get; }
        string ToDoDB_constr = string.Empty;

        // call counstructior of type configuration to read appsettings.json properties
        public ToDoListController(IConfiguration config)
        {
            // define configuration object and read sql server connection string from appsettings.json
            Configuration = config;
            ToDoDB_constr = this.Configuration.GetConnectionString("ToDodataConnection");
        }


        // POST api/<ToDoListControllercs>
        //{format}is used to define api response type  exe json or xml
        // Create  method of List response type
        [HttpPost("post.{format}"), FormatFilter]
        public List<ListResponseBody> Post([FromBody] ListRequestBody doList)
        {
            // check and validate request state of api 
            if (ModelState.IsValid)
            {
                // read list data item dataset received from sql server with help of DB helper classs
                DataSet MyList = DBHelper.GetOrAddListToDoDB(doList, "CreateAndGetToDoList", ToDoDB_constr);

                // bind list data item  dataset to response body
                if (MyList != null && MyList.Tables.Count > 0)
                {
                    foreach (DataRow dr in MyList.Tables[0].Rows)
                    {
                        _ToDoList.Add(new ListResponseBody { ToDoListName = Convert.ToString(dr["ToDoListName"]) });
                    };
                }

                return _ToDoList;
            }
            return null;
        }
    }
}
