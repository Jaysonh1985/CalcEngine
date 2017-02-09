// Copyright (c) 2016 Project AIM
using CalculationCSharp.Areas.Project.Models.ViewModels;
using CalculationCSharp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CalculationCSharp.Areas.Project.Models
{
    public class BoardRepository
    {
        CalculationDBContext db = new CalculationDBContext();
        ProjectBoards ProjectBoard = new ProjectBoards();
        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        public List<ProjectBoards> GetBoards()
        {
            var ProjectBoardList = db.ProjectBoards.ToList();
            var ColumnsReturns = ProjectBoardList
            .Select(o => new ProjectBoards
            {
                  Name = o.Name,
                Client= o.Client,
                UpdateDate = o.UpdateDate,
                User = o.User,
                BoardId = o.BoardId

            }).ToList();
            return ProjectBoardList;
        }

        public ProjectBoards GetBoard(dynamic json)
        {
            if(json.boardId == null)
            {
                return null;
            }
            else
            {
                int ID = Convert.ToInt32(json.boardId);
                var Test = db.ProjectBoards.Include(t => t.ProjectColumns).Where(t => t.BoardId == ID).FirstOrDefault();
                ProjectBoards Board = db.ProjectBoards.Find(ID);

                return Board;
            }                      
        }

        public void AddBoard(dynamic json)
        {
            ProjectBoard.Name = json.boardName;
            ProjectBoard.Configuration = Convert.ToString(json.data);
            ProjectBoard.User = HttpContext.Current.User.Identity.Name.ToString();
            ProjectBoard.UpdateDate = DateTime.Now;
            db.ProjectBoards.Add(ProjectBoard);
            db.SaveChanges();
        }

        public ProjectBoards UpdateBoard(dynamic json)
        {
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            ProjectBoards ProjectBoard = db.ProjectBoards.Find(Convert.ToInt32(json.boardId));
            ProjectBoard.Configuration = Convert.ToString(json.data);
            ProjectBoard.User = HttpContext.Current.User.Identity.Name.ToString();
            ProjectBoard.UpdateDate = DateTime.Now;
            List<ProjectColumns> Columns = (List<ProjectColumns>)javaScriptSerializ­er.Deserialize(Convert.ToString(json.data), typeof(List<ProjectColumns>));

            foreach(var col in Columns)
            {
                col.ProjectBoard = ProjectBoard;
                if(col.ProjectStories != null)
                {
                    foreach (var story in col.ProjectStories)
                    {
                        story.ProjectColumns = col;
                    }
                }
            }
            // Load original parent including the child item collection
            var originalBoard = db.ProjectBoards
                .Where(p => p.BoardId == ProjectBoard.BoardId)
                .Include(p => p.ProjectColumns)
                .SingleOrDefault();
            if(originalBoard != null)
            {
                //update parent
                var parentEntry = db.Entry(originalBoard);
                parentEntry.CurrentValues.SetValues(ProjectBoard);

                foreach (var originalColumnItem in
                    originalBoard.ProjectColumns.ToList())
                {
                    if (!ProjectBoard.ProjectColumns.Any(c => c.ColumnId == originalColumnItem.ColumnId))
                        // Yes -> It's a deleted child item -> Delete
                        db.ProjectColumns.Remove(originalColumnItem);
                }
                /////COLUMNS
                foreach (var columnItem in Columns)
                {
                    var originalColumnItem = db.ProjectColumns
                        .Where(c => c.ColumnId == columnItem.ColumnId)
                        .Include(p=> p.ProjectStories)
                        .SingleOrDefault();

                    // Is original child item with same ID in DB?
                    if (originalColumnItem != null)
                    {               
                        // Yes -> Update scalar properties of child item
                        var columnEntry = db.Entry(originalColumnItem);
                        var originalColumnValues = db.Entry(originalColumnItem).OriginalValues;
                        // Set the Name property to a new value 
                        columnEntry.Property(u => u.ColumnId).CurrentValue = columnItem.ColumnId;
                        columnEntry.Property(u => u.Description).CurrentValue = columnItem.Description;
                        columnEntry.Property(u => u.Name).CurrentValue = columnItem.Name;
                        columnEntry.Property(u => u.UpdateDate).CurrentValue = columnItem.UpdateDate;

                        // Load original parent including the child item collection
                        var originalColumn = db.ProjectColumns
                            .Where(p => p.ColumnId == columnItem.ColumnId)
                            .Include(p => p.ProjectStories)
                            .SingleOrDefault();

                        ///STORIES
                        foreach (var originalStoryItem in
                        originalColumn.ProjectStories.ToList())
                            {
                                if (!columnItem.ProjectStories.Any(c => c.StoryId == originalStoryItem.StoryId))
                                    // Yes -> It's a deleted child item -> Delete
                                    db.ProjectStories.Remove(originalStoryItem);
                            }
                        foreach (var storyItem in columnItem.ProjectStories)
                        {
                            var originalStoryItem = originalColumnItem.ProjectStories
                            .Where(c => c.StoryId == storyItem.StoryId)
                            .SingleOrDefault();

                            // Is original child item with same ID in DB?
                            if (originalStoryItem != null)
                            {
                                // Yes -> Update scalar properties of child item
                                var storyEntry = db.Entry(originalStoryItem);
                                var originalStoryValues = db.Entry(originalStoryItem).OriginalValues;
                                // Set the Name property to a new value 
                                storyEntry.Property(u => u.Name).CurrentValue = storyItem.Name;
                                storyEntry.Property(u => u.StoryId).CurrentValue = storyItem.StoryId;
                                storyEntry.Property(u => u.Requested).CurrentValue = storyItem.Requested;
                                storyEntry.Property(u => u.Moscow).CurrentValue = storyItem.Moscow;
                                storyEntry.Property(u => u.Timebox).CurrentValue = storyItem.Timebox;
                                storyEntry.Property(u => u.AcceptanceCriteria).CurrentValue = storyItem.AcceptanceCriteria;
                                storyEntry.Property(u => u.StartDate).CurrentValue = storyItem.StartDate;
                                storyEntry.Property(u => u.RAG).CurrentValue = storyItem.RAG;
                                storyEntry.Property(u => u.RequestedDate).CurrentValue = storyItem.RequestedDate;
                                storyEntry.Property(u => u.SLADays).CurrentValue = storyItem.SLADays;
                                storyEntry.Property(u => u.Complexity).CurrentValue = storyItem.Complexity;
                                storyEntry.Property(u => u.ElapsedTime).CurrentValue = storyItem.ElapsedTime;
                                storyEntry.Property(u => u.DueDate).CurrentValue = storyItem.DueDate;
                                storyEntry.Property(u => u.User).CurrentValue = storyItem.User;
                                storyEntry.Property(u => u.Description).CurrentValue = storyItem.Description;
                                storyEntry.Property(u => u.Name).CurrentValue = storyItem.Name;
                                storyEntry.Property(u => u.UpdateDate).CurrentValue = storyItem.UpdateDate;
                                storyEntry.Property(u => u.Effort).CurrentValue = storyItem.Effort;
                                storyEntry.State = EntityState.Modified;

                                // Load original parent including the child item collection
                                var originalStory = db.ProjectStories
                                    .Where(p => p.StoryId == storyItem.StoryId)
                                    .Include(p => p.ProjectComments)
                                    .Include(t => t.ProjectTasks)
                                    .Include(t => t.ProjectUpdates)
                                    .SingleOrDefault();
                                //COMMENTS
                                foreach (var originalCommentItem in
                                originalStory.ProjectComments.ToList())
                                {
                                    if (!storyItem.ProjectComments.Any(c => c.CommentId == originalCommentItem.CommentId))
                                        // Yes -> It's a deleted child item -> Delete
                                        db.ProjectComments.Remove(originalCommentItem);
                                }
                                foreach (var commentItem in storyItem.ProjectComments)
                                {
                                    var originalCommentItem = originalStoryItem.ProjectComments
                                    .Where(c => c.CommentId == commentItem.CommentId)
                                    .SingleOrDefault();

                                    // Is original child item with same ID in DB?
                                    if (originalCommentItem != null)
                                    {
                                        //// Yes -> Update scalar properties of child item
                                        //var commentEntry = db.Entry(originalCommentItem);
                                        //var originalCommentValues = db.Entry(originalCommentItem).OriginalValues;
                                        //// Set the Name property to a new value 
                                        //commentEntry.Property(u => u.CommentId).CurrentValue = commentItem.CommentId;
                                        //commentEntry.Property(u => u.CommentName).CurrentValue = commentItem.CommentName;
                                        //commentEntry.Property(u => u.CommentType).CurrentValue = commentItem.CommentType;
                                        //commentEntry.Property(u => u.CommentDateTime).CurrentValue = commentItem.CommentDateTime;
                                        //commentEntry.Property(u => u.UpdateDate).CurrentValue = commentItem.UpdateDate;
                                        //commentEntry.Property(u => u.CommentUser).CurrentValue = commentItem.CommentUser;
                                        //commentEntry.State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        commentItem.ProjectStories = originalStory;
                                        commentItem.UpdateDate = DateTime.Now;
                                        originalStoryItem.ProjectComments.Add(commentItem);
                                    }
                                }
                                //Tasks
                                foreach (var originalTaskItem in
                                originalStory.ProjectTasks.ToList())
                                {
                                    if (!storyItem.ProjectTasks.Any(c => c.TaskId == originalTaskItem.TaskId))
                                        // Yes -> It's a deleted child item -> Delete
                                        db.ProjectTasks.Remove(originalTaskItem);
                                }
                                foreach (var taskItem in storyItem.ProjectTasks)
                                {
                                    var originalTaskItem = originalStoryItem.ProjectTasks
                                    .Where(c => c.TaskId == taskItem.TaskId)
                                    .SingleOrDefault();

                                    // Is original child item with same ID in DB?
                                    if (originalTaskItem != null)
                                    {
                                        // Yes -> Update scalar properties of child item
                                        var taskEntry = db.Entry(originalTaskItem);
                                        var originalTaskValues = db.Entry(originalTaskItem).OriginalValues;
                                        // Set the Name property to a new value 
                                        taskEntry.Property(u => u.TaskName).CurrentValue = taskItem.TaskName;
                                        taskEntry.Property(u => u.TaskUser).CurrentValue = taskItem.TaskUser;
                                        taskEntry.Property(u => u.UpdateDate).CurrentValue = taskItem.UpdateDate;
                                        taskEntry.Property(u => u.Status).CurrentValue = taskItem.Status;
                                        taskEntry.Property(u => u.RemainingTime).CurrentValue = taskItem.RemainingTime;
                                        taskEntry.Property(u => u.TaskId).CurrentValue = taskItem.TaskId;
                                        taskEntry.State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        taskItem.ProjectStories = originalStory;
                                        taskItem.UpdateDate = DateTime.Now;
                                        originalStoryItem.ProjectTasks.Add(taskItem);
                                    }
                                }

                                //Tasks
                                foreach (var originalUpdateItem in
                                originalStory.ProjectUpdates.ToList())
                                {
                                    if (!storyItem.ProjectUpdates.Any(c => c.UpdateId == originalUpdateItem.UpdateId))
                                        // Yes -> It's a deleted child item -> Delete
                                        db.ProjectUpdates.Remove(originalUpdateItem);
                                }
                                foreach (var updateItem in storyItem.ProjectUpdates)
                                {
                                    var originalUpdateItem = originalStoryItem.ProjectUpdates
                                    .Where(c => c.UpdateId == updateItem.UpdateId)
                                    .SingleOrDefault();

                                    // Is original child item with same ID in DB?
                                    if (originalUpdateItem != null)
                                    {
                                        //// Yes -> Update scalar properties of child item
                                        //var updateEntry = db.Entry(originalUpdateItem);
                                        //var originalUpdateValues = db.Entry(originalUpdateItem).OriginalValues;
                                        //// Set the Name property to a new value 
                                        //updateEntry.Property(u => u.UpdateDate).CurrentValue = updateItem.UpdateDate;
                                        //updateEntry.Property(u => u.UpdateDateTime).CurrentValue = updateItem.UpdateDateTime;
                                        //updateEntry.Property(u => u.UpdateField).CurrentValue = updateItem.UpdateField;
                                        //updateEntry.Property(u => u.UpdateId).CurrentValue = updateItem.UpdateId;
                                        //updateEntry.Property(u => u.UpdateUser).CurrentValue = updateItem.UpdateUser;
                                        //updateEntry.Property(u => u.UpdateValue).CurrentValue = updateItem.UpdateValue;
                                        //updateEntry.State = EntityState.Modified;
                                    }
                                    else
                                    {
                                        updateItem.ProjectStories = originalStory;
                                        updateItem.UpdateDate = DateTime.Now;
                                        originalStoryItem.ProjectUpdates.Add(updateItem);
                                    }
                                }

                            }
                            else
                            {
                                storyItem.ProjectColumns = originalColumn;
                                storyItem.UpdateDate = DateTime.Now;
                                originalColumn.ProjectStories.Add(storyItem);

                            }
                        }
                    }
                    else
                    {
                        originalBoard.ProjectColumns.Add(columnItem);
                    }
                }
            }
            

            db.SaveChanges();

            return ProjectBoard;
        }

        public void DeleteBoard(dynamic json)
        {
            var db = new CalculationDBContext();
            ProjectBoards ProjectBoard = db.ProjectBoards.Find(Convert.ToInt32(json.boardId));
            db.ProjectBoards.Remove(ProjectBoard);
            db.SaveChanges();
        }
    }
}