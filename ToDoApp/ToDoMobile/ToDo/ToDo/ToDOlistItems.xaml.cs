using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToDo.Constraints;
using ToDo.DataBinders;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToDOlistItems : ContentPage
    {


        List<ListItems> listdata = new List<ListItems>();
        List<ListItems> Updatedlistdata = new List<ListItems>();

        string ReceivedListNAme = string.Empty;
        public ToDOlistItems(string listNAme)
        {
            InitializeComponent();

            ReceivedListNAme = listNAme;
            ErrorMesasge.IsVisible = false;
            ToDoList_HeaderText.Text = listNAme + " List Items";
            //!willbe replaced by ID passed by usner from mainpage dropdown
            GetToDOlistItemsByID("<InsertData><ToDoListName></ToDoListName></InsertData>", ReceivedListNAme, "Get");
        }

        //function  to call /intrect with backend api 
        public async void GetToDOlistItemsByID(string xml,string listName,string type)
        {

            using (var client = new HttpClient())
            {
                // send a post request  
                var uri = Contraints.API_BaseURL + Contraints.GetListITEMURL_JSON;

                //API request data set to encodeded Form
                var RequestBody = new { xmlString = xml, ListName= listName, type = type };

                //Converting the object to a json string. NOTE: Make sure the object doesn't contain circular references.
                string json = JsonConvert.SerializeObject(RequestBody);

                //Needed to setup the body of the request
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                //Pass in the full URL and the json string content
                var response = await client.PostAsync(uri, data);

                //It would be better to make sure this request actually made it through
                string resultContent = await response.Content.ReadAsStringAsync();

                ////Bind API response to my list Model
                listdata =JsonConvert.DeserializeObject<List<ListItems>>(resultContent);


                //Bind data to listView
                if (listdata.Count > 0)
                {
                    Updatedlistdata = listdata;
                    SelectedTODOList_Itmes.ItemsSource = null;
                    SelectedTODOList_Itmes.ItemsSource = listdata;
                }
            }

        }


        private void NewEntryTOAddToListpriority_TextChanged(object sender, TextChangedEventArgs e)
        {
            //to validate new list item feild is blank or not and show message as per response
            if ((NewEntryTOAddToListpriority.Text != null && !NewEntryTOAddToListpriority.Text.ToString().Equals("")))
            {
                ErrorMesasge.IsVisible = false;
            }
            else
                ErrorMesasge.IsVisible = true;
        }

        private void AddToDOList_item_Clicked(object sender, EventArgs e)
        {
            //check data is not blank to add new item to list 
            if ((NewEntryTOAddToListItem.Text != null && !NewEntryTOAddToListItem.Text.ToString().Equals("")) &&
                (NewEntryTOAddToListpriority.Text != null && !NewEntryTOAddToListpriority.Text.ToString().Equals("")))
            {
                //check data is available or not on basis of prority
                bool alreadyExists = Updatedlistdata.Any(x =>  x.priority == Convert.ToInt32(NewEntryTOAddToListpriority.Text.ToString()));

                //check max id from previouslyupdated list if no item in list then it is 1
                int maxID = 1;
                if (Updatedlistdata.Count > 0)
                    maxID = Updatedlistdata.Max(s => s.ToDoListId);

                //check inserted priorityis not available in system add to list if not then add else show error message
                if (!alreadyExists)
                {
                    //if inserted priorityis not available in system add to list 
                    Updatedlistdata.Add(new ListItems()
                    {
                        ToDoListId=maxID+1,
                        ToDoListItem = NewEntryTOAddToListItem.Text.ToString(),
                        priority = Convert.ToInt32(NewEntryTOAddToListpriority.Text.ToString())
                    });
                }
                else
                {
                    ErrorMesasge.Text = "Already has  a task with inserted priority";
                    ErrorMesasge.IsVisible = true;
                }

                //update the list after clearing the  old data
                SelectedTODOList_Itmes.ItemsSource = null;
                SelectedTODOList_Itmes.ItemsSource = Updatedlistdata;

                //set feild to black after updating the list
                NewEntryTOAddToListItem.Text = "";
                NewEntryTOAddToListpriority.Text = "";
            }
            else
                ErrorMesasge.IsVisible = true;
        }

        private void NewEntryTOAddToListItem_TextChanged(object sender, TextChangedEventArgs e)
        {
            //to validate new list item feild is blank or not and show message as per response
            if ((NewEntryTOAddToListItem.Text != null && !NewEntryTOAddToListItem.Text.ToString().Equals("")))
            {
                ErrorMesasge.IsVisible = false;
            }
            else
                ErrorMesasge.IsVisible = true;
        }


        public void Delete_Clicked(object sender, EventArgs e)
        {
            //Get ID of button from listView to get ID which need to remove
            Button clickedListID_btn = (Xamarin.Forms.Button)sender;

            //check if ID isnull or non zero value
            if (clickedListID_btn.CommandParameter != null && Convert.ToInt32(clickedListID_btn.CommandParameter) != 0)
            {
                //checking received ID iss available in  list or not
                bool alreadyExists = Updatedlistdata.Any(x => x.ToDoListId == Convert.ToInt32(clickedListID_btn.CommandParameter));

                if (alreadyExists)
                {
                    //if received filter that item on basis of ID using lamda expression and then remove that 
                    var itemToRemove = Updatedlistdata.Single(r => r.ToDoListId == Convert.ToInt32(clickedListID_btn.CommandParameter));
                    Updatedlistdata.Remove(itemToRemove);

                    //After remove data from list clear old data and refresh the list 
                    SelectedTODOList_Itmes.ItemsSource = null;
                    SelectedTODOList_Itmes.ItemsSource = Updatedlistdata;

                    //after update text feild is set to blank for new values
                    NewEntryTOAddToListItem.Text = "";
                    NewEntryTOAddToListpriority.Text = "";
                }
            }
        }

        private void SaveToDOList_item_Clicked(object sender, EventArgs e)
        {
            //send updated list to DB for updation
            if (Updatedlistdata.Count > 0)
            {
                //Bind list data to single XML string to update all data for list name
                string ModifiedXML = string.Empty;
                for(int ModifiedList=0;ModifiedList <Updatedlistdata.Count;ModifiedList++)
                {
                    ModifiedXML += "<DataRow>" +
                        "<ToDoListName>" + ReceivedListNAme + "</ToDoListName>" +
                        "<ToDoListItem>" + Updatedlistdata[ModifiedList].ToDoListItem + "</ToDoListItem>" +
                        "<priority>" + Updatedlistdata[ModifiedList].priority + "</priority>" +
                    "</DataRow>";
                }

                //pass data to function which is used to call api to update
                GetToDOlistItemsByID("<InsertData>"+ ModifiedXML + "</InsertData>", ReceivedListNAme, "Insert");
                DisplayAlert("Notification", "List Has been Updated in system", "Ok");
            }
        }

        private void backTOMain_Clicked(object sender, EventArgs e)
        {
            //redirect to Mavigation page which is top of navigation page means base page and remove all stacks created by navigation
            Navigation.PopToRootAsync();
        }
    }
}