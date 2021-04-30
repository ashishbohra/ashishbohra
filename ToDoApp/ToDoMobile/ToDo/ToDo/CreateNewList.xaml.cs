using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToDo.Constraints;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateNewList : ContentPage
    {
        public CreateNewList()
        {
            InitializeComponent();
            ErrorMesasge.IsVisible = false;
        }

        public void CreateToDoList_Clicked(object sender, EventArgs e)
        {
            if (NewToDoList.Text != null && !NewToDoList.Text.ToString().Equals(""))
                InsertNewToDolistMenu(NewToDoList.Text.ToString());
            else
                ErrorMesasge.IsVisible = true;
        }

        //function  to call /intrect with backend api
        public async void InsertNewToDolistMenu(string NewListName)
        {
            if (!NewListName.Equals(""))
            {
                using (var client = new HttpClient())
                {
                    // send a GET request  
                    var uri = Contraints.API_BaseURL + Contraints.GetListURL_JSON;

                    //API request data set to encodeded Form
                    var RequestBody = new { xmlString = "<InsertData><ToDoListName>" + NewListName + "</ToDoListName></InsertData>", type = "Insert" };

                    //Converting the object to a json string. NOTE: Make sure the object doesn't contain circular references.
                    string json = JsonConvert.SerializeObject(RequestBody);

                    //Needed to setup the body of the request
                    StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                    //Pass in the full URL and the json string content
                    var response = await client.PostAsync(uri, data);

                    //It would be better to make sure this request actually made it through
                    string resultContent = await response.Content.ReadAsStringAsync();

                    client.Dispose();
                }
                DisplayAlert("Notification", "List Has been created in system", "Ok");
                //redirect to Mavigation page which is top of navigation page means base page
                Navigation.PopAsync();
            }
            else
            {
                ErrorMesasge.IsVisible = true;
            }
        }

        private void NewToDoList_TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorMesasge.IsVisible = false;
        }
    }
}