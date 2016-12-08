// Copyright (c) 2016 Project AIM
using CalculationCSharp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Project.Models
{
    public class ColumnRepository
    {

        public List<Column> GetColumns(ProjectBoard ProjectBoard)
        {
            if (HttpContext.Current.Cache["columns"] == null)
            {
                if (ProjectBoard.Configuration == "" || ProjectBoard.Configuration == null)
                {
                    var columns = new List<Column>();
                    var stories = new List<Stories>();
                    columns.Add(new Column { Description = "Backlog Column", Id = 1, Name = "Backlog", Stories = stories });
                    columns.Add(new Column { Description = "In Progress Column", Id = 2, Name = "In Progress", Stories = new List<Stories>() });
                    columns.Add(new Column { Description = "Test Column", Id = 3, Name = "Test", Stories = new List<Stories>() });
                    columns.Add(new Column { Description = "Release Column", Id = 4, Name = "Release", Stories = new List<Stories>() });
                    HttpContext.Current.Cache["columns"] = columns;
                }
                else
                {
                    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                    string jsonString = ProjectBoard.Configuration;
                    List<Column> columns = (List<Column>)javaScriptSerializ­er.Deserialize(jsonString, typeof(List<Column>));
                    HttpContext.Current.Cache["columns"] = columns;
                }
                
            }
            return (List<Column>)HttpContext.Current.Cache["columns"];
        }

        public Column GetColumn(int colId)
        {
            return (from c in this.GetColumns(null)
                    where c.Id == colId
                    select c).FirstOrDefault();
        }

        public Stories GetStories(int storyId)
        {
            var columns = this.GetColumns(null);            
            foreach (var c in columns)
            {                
                foreach (var story in c.Stories)
                {
                    if (story.Id == storyId)
                        return story;
                }
            }

            return null;
        }

        public void AddStory(int targetColId)
        {
            var columns = this.GetColumns(null);
            var targetColumn = this.GetColumn(targetColId);
            var StoryId = targetColumn.Stories.Count;
            // Add Story to the target column
            var Story = new Stories();
            Story.Description = "Story Description " + StoryId;
            Story.AcceptanceCriteria = "Acceptance";
            Story.Id = StoryId;
            Story.Name = "Story " + StoryId;
            Story.Moscow = "Must";
            Story.Timebox = "Timebox 1";
            Story.User = "Jayson Herbert";

            var sourceColId = Story.ColumnId;
            Story.ColumnId = targetColId;
            targetColumn.Stories.Add(Story);

            this.UpdateColumns(columns.OrderBy(c => c.Id).ToList());
        }

        public void DeleteStory(int StoryId, int targetColId)
        {
            var columns = this.GetColumns(null);

            // Add Story to the target column
            var Story = this.GetStories(StoryId);
            var sourceColId = Story.ColumnId;

            // Remove Story from source column
            var sourceCol = this.GetColumn(sourceColId);
            sourceCol.Stories.RemoveAll(t => t.Id == StoryId);

            this.UpdateColumns(columns.OrderBy(c => c.Id).ToList());
        }

        public void EditStory(int StoryId, int targetColId, JObject Data)
        {
            dynamic json = Data;
            var columns = this.GetColumns(null);
            var targetColumn = this.GetColumn(targetColId);
            List<Tasks> Tasks = new List<Tasks>();
            
            // Add Story to the target column
            var Story = this.GetStories(StoryId);
            Story.Name = json.Name;
            Story.Description = json.Description;
            Story.AcceptanceCriteria = json.AcceptanceCriteria;
            Story.DueDate = json.DueDate;
            Story.RAG = json.RAG;
            Story.Moscow = json.Moscow;
            Story.Timebox = json.Timebox;
            Story.User = json.User;
            
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string jsonString = Convert.ToString(json.Tasks);
            string jsonStringComments = Convert.ToString(json.Comments);

            //if(jsonString != ""){

            //    List<Tasks> jTasks = (List<Tasks>)javaScriptSerializ­er.Deserialize(jsonString, typeof(List<Tasks>));
            //    Story.Tasks = jTasks;
            //}

          
            //List<Comments> jComments = (List<Comments>)javaScriptSerializ­er.Deserialize(jsonStringComments, typeof(List<Comments>));
            
            //Story.Comments = jComments;


            var sourceColId = Story.ColumnId;

            if(targetColId != sourceColId)
            {
                Story.ColumnId = targetColId;
                targetColumn.Stories.Add(Story);

                // Remove Story from source column
                var sourceCol = this.GetColumn(sourceColId);
                sourceCol.Stories.RemoveAll(t => t.Id == StoryId);

                // Update column collection
                columns.RemoveAll(c => c.Id == sourceColId || c.Id == targetColId);
                columns.Add(targetColumn);
                columns.Add(sourceCol);
            }



            this.UpdateColumns(columns.OrderBy(c => c.Id).ToList());
        }

        private void UpdateColumns(List<Column> columns)
        {
            HttpContext.Current.Cache["columns"] = columns;
        }
    }
}