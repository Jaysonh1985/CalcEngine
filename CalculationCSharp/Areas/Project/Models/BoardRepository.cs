using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Areas.Project.Models
{
    public class BoardRepository
    {
        public List<Column> GetColumns()
        {
            if (HttpContext.Current.Cache["columns"] == null)
            {
                var columns = new List<Column>();
                var stories = new List<Stories>();
                for (int i = 1; i < 6; i++)
                {
                    stories.Add(new Stories { ColumnId = 1, Id = i, Name = "Story " + i, Description = "Story " + i + " Description", Moscow = "Must", User = "Jayson Herbert" });
                }
                columns.Add(new Column { Description = "to do column", Id = 1, Name = "Backlog", Stories = stories });
                columns.Add(new Column { Description = "in progress column", Id = 2, Name = "In Progress", Stories = new List<Stories>() });
                columns.Add(new Column { Description = "test column", Id = 3, Name = "Test", Stories = new List<Stories>() });
                columns.Add(new Column { Description = "done column", Id = 4, Name = "Release", Stories = new List<Stories>() });
                HttpContext.Current.Cache["columns"] = columns;
            }
            return (List<Column>)HttpContext.Current.Cache["columns"];
        }

        public Column GetColumn(int colId)
        {
            return (from c in this.GetColumns()
                    where c.Id == colId
                    select c).FirstOrDefault();
        }

        public Stories GetStories(int storyId)
        {
            var columns = this.GetColumns();            
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

        public void MoveStory(int StoryId, int targetColId)
        {
            var columns = this.GetColumns();
            var targetColumn = this.GetColumn(targetColId);
            
            // Add Story to the target column
            var Story = this.GetStories(StoryId);
            var sourceColId = Story.ColumnId;
            Story.ColumnId = targetColId;
            targetColumn.Stories.Add(Story);

            // Remove Story from source column
            var sourceCol = this.GetColumn(sourceColId);
            sourceCol.Stories.RemoveAll(t => t.Id == StoryId);

            // Update column collection
            columns.RemoveAll(c => c.Id == sourceColId || c.Id == targetColId);
            columns.Add(targetColumn);
            columns.Add(sourceCol);

            this.UpdateColumns(columns.OrderBy(c => c.Id).ToList());
        }

        public void AddStory(int targetColId)
        {
            var columns = this.GetColumns();
            var targetColumn = this.GetColumn(targetColId);
            var StoryId = targetColumn.Stories.Count;
            // Add Story to the target column
            var Story = new Stories();
            Story.Description = "Story Description " + StoryId;
            Story.Id = StoryId;
            Story.Name = "Story " + StoryId;
            Story.Moscow = "Must";
            Story.User = "Jayson Herbert";

            var sourceColId = Story.ColumnId;
            Story.ColumnId = targetColId;
            targetColumn.Stories.Add(Story);

            this.UpdateColumns(columns.OrderBy(c => c.Id).ToList());
        }

        public void AddTask(int StoryId, int targetColId, JObject Data)
        {
            dynamic json = Data;
            var columns = this.GetColumns();
            var targetColumn = this.GetColumn(targetColId);
            List<Tasks> Tasks = new List<Tasks>();

            // Add Story to the target column
            var Story = this.GetStories(StoryId);
            var sourceColId = Story.ColumnId;
            Story.Name = json.Name;
            Story.Description = json.Description;
            Story.Moscow = json.Moscow;
            Story.User = json.User;

            Tasks.Add(new Tasks() { Id = 1, Name = "Task 1", User = "Jayson Herbert", RemainingTime = 0, StoryId = StoryId });

            if(Story.Tasks == null){
                Story.Tasks = Tasks;
            }
            else
            {
                Tasks.AddRange(Story.Tasks);
                Story.Tasks = Tasks;
            }
            
             this.UpdateColumns(columns.OrderBy(c => c.Id).ToList());
        }

        public void DeleteStory(int StoryId, int targetColId)
        {
            var columns = this.GetColumns();

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
            var columns = this.GetColumns();
            var targetColumn = this.GetColumn(targetColId);

            // Add Story to the target column
            var Story = this.GetStories(StoryId);
            var sourceColId = Story.ColumnId;
            Story.Name = json.Name;
            Story.Description = json.Description;
            Story.Moscow = json.Moscow;
            Story.User = json.User;

            this.UpdateColumns(columns.OrderBy(c => c.Id).ToList());
        }

        private void UpdateColumns(List<Column> columns)
        {
            HttpContext.Current.Cache["columns"] = columns;
        }
    }
}