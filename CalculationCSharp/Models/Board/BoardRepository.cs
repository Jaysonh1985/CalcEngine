using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Models
{
    public class BoardRepository
    {
        public List<Column> GetColumns()
        {
            if (HttpContext.Current.Cache["columns"] == null)
            {
                var columns = new List<Column>();
                var tasks = new List<Tasks>();
                for (int i = 1; i < 6; i++)
                {
                    tasks.Add(new Tasks { ColumnId = 1, Id = i, Name = "Task " + i, Description = "Task " + i + " Description" });
                }
                columns.Add(new Column { Description = "to do column", Id = 1, Name = "Backlog", Tasks = tasks });
                columns.Add(new Column { Description = "in progress column", Id = 2, Name = "In Progress", Tasks = new List<Tasks>() });
                columns.Add(new Column { Description = "test column", Id = 3, Name = "Test", Tasks = new List<Tasks>() });
                columns.Add(new Column { Description = "done column", Id = 4, Name = "Release", Tasks = new List<Tasks>() });
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

        public Tasks GetTask(int taskId)
        {
            var columns = this.GetColumns();            
            foreach (var c in columns)
            {                
                foreach (var task in c.Tasks)
                {
                    if (task.Id == taskId)
                        return task;
                }
            }

            return null;
        }

        public void MoveTask(int taskId, int targetColId)
        {
            var columns = this.GetColumns();
            var targetColumn = this.GetColumn(targetColId);
            
            // Add task to the target column
            var task = this.GetTask(taskId);
            var sourceColId = task.ColumnId;
            task.ColumnId = targetColId;
            targetColumn.Tasks.Add(task);
            
            // Remove task from source column
            var sourceCol = this.GetColumn(sourceColId);
            sourceCol.Tasks.RemoveAll(t => t.Id == taskId);

            // Update column collection
            columns.RemoveAll(c => c.Id == sourceColId || c.Id == targetColId);
            columns.Add(targetColumn);
            columns.Add(sourceCol);

            this.UpdateColumns(columns.OrderBy(c => c.Id).ToList());
        }

        public void AddTask(int targetColId)
        {
            var columns = this.GetColumns();
            var targetColumn = this.GetColumn(targetColId);
            var taskId = targetColumn.Tasks.Count;
            // Add task to the target column
            var task = new Tasks();
            task.Description = "Task Description " + taskId;
            task.Id = taskId;
            task.Name = "Task " + taskId;
                         
            var sourceColId = task.ColumnId;
            task.ColumnId = targetColId;
            targetColumn.Tasks.Add(task);

            this.UpdateColumns(columns.OrderBy(c => c.Id).ToList());
        }

        public void DeleteTask(int taskId, int targetColId)
        {
            var columns = this.GetColumns();

            // Add task to the target column
            var task = this.GetTask(taskId);
            var sourceColId = task.ColumnId;

            // Remove task from source column
            var sourceCol = this.GetColumn(sourceColId);
            sourceCol.Tasks.RemoveAll(t => t.Id == taskId);

            this.UpdateColumns(columns.OrderBy(c => c.Id).ToList());
        }

        public void EditTask(int taskId, int targetColId, JObject Data)
        {
            dynamic json = Data;
            var columns = this.GetColumns();
            var targetColumn = this.GetColumn(targetColId);

            // Add task to the target column
            var task = this.GetTask(taskId);
            var sourceColId = task.ColumnId;
            task.Name = json.Name;
            task.Description = json.Description;

            this.UpdateColumns(columns.OrderBy(c => c.Id).ToList());
        }

        private void UpdateColumns(List<Column> columns)
        {
            HttpContext.Current.Cache["columns"] = columns;
        }
    }
}