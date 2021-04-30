using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using ToDo.DataBinders;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ToDo.Constraints;

namespace ToDo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            ErrorMesasge.IsVisible = false;
        }

        //this function always call once once page will awake 
        protected override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            BindToDolistItems("NoData", "Get");
        }

        //function  to call /intrect with backend api
        public async void BindToDolistItems(string xmldata,string type)
        {
            using (var client = new HttpClient())
            {
                // send a post request  
                var url = Contraints.API_BaseURL + Contraints.GetListURL_JSON;

                //API request data set to encodeded Form
                var RequestBody = new { xmlString = xmldata,type = type};

                //Converting the object to a json string. NOTE: Make sure the object doesn't contain circular references.
                string json = JsonConvert.SerializeObject(RequestBody);

                //Needed to setup the body of the request
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //Pass in the full URL and the json string content
                var response = await client.PostAsync(url, data);

                //It would be better to make sure this request actually made it through
                string resultContent = await response.Content.ReadAsStringAsync();

                ////Bind API response to my list Model
                List<MyToDoList> ToDoList = JsonConvert.DeserializeObject<List<MyToDoList>>(resultContent);


                //Bind data to main page drop down
                if (ToDoList.Count > 0)
                {
                    //clear old data befor binding new 
                    TODoListPiker.Items.Clear();

                    //binding new data
                    for (int listCount= 0;listCount < ToDoList.Count; listCount++)
                    {
                        TODoListPiker.Items.Add(ToDoList[listCount].ToDoListName);
                    }
                }
                else
                {
                    DisplayAlert("Notification", "NO lists found in system", "Ok");
                }

                client.Dispose();
            }
        }

        private void TODoListPiker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TO DO drop down on  change  functionlaity has been created here
            ErrorMesasge.IsVisible = false;
        }

        private void DeleteToDOList_item_Clicked(object sender, EventArgs e)
        {
            //TO DO delete functionlaity has been created here
            if (TODoListPiker.SelectedIndex != null && TODoListPiker.SelectedIndex != -1)
            {
                if (!TODoListPiker.Items[TODoListPiker.SelectedIndex].Equals(""))
                {
                    //get listName which we want to delete
                    var SelectedToDO_Index = TODoListPiker.Items[TODoListPiker.SelectedIndex];

                    //call API function to delete the list
                    //Bind data XML so it will easy to perform multi data manupulation on single HIT
                    BindToDolistItems("<InsertData><ToDoListName>" + SelectedToDO_Index + "</ToDoListName></InsertData>", "Delete");
                    DisplayAlert("Notification", "List Has been Deleted from system", "Ok");
                }
                else
                {
                    ErrorMesasge.IsVisible = true;
                }
            }
        }

        private  void OpenToDOById_Clicked(object sender, EventArgs e)
        {

            //Navigating to List item page
            if(TODoListPiker.SelectedIndex != null && TODoListPiker.SelectedIndex != -1)
            {
                if (!TODoListPiker.Items[TODoListPiker.SelectedIndex].Equals(""))
                {
                    //get listName which we want to get list ITEM
                    var SelectedToDO_Index = TODoListPiker.Items[TODoListPiker.SelectedIndex];
                    Navigation.PushAsync(new ToDOlistItems(SelectedToDO_Index));
                }
            }
            else
            {
                ErrorMesasge.IsVisible = true;
            }
        }

        private  void CreateNewList_btn_Clicked(object sender, EventArgs e)
        {
            //Navigating to create New PAge
            Navigation.PushAsync(new CreateNewList());
        }
    }
}
