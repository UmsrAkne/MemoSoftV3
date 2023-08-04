using System;
using System.Collections.Generic;

namespace MemoSoftV3.Models
{
    public class SampleCommentProvider
    {
        public static List<Comment> GetComments()
        {
            return new List<Comment>()
            {
                new ()
                {
                    Id = 1,
                    Text = "test Text1",
                    IsFavorite = false,
                    IsCheckable = false,
                    Checked = false,
                    DateTime = DateTime.Now,
                    Tags = new List<Tag>(),
                    SubComments = new List<SubComment>(),
                },

                new ()
                {
                    Id = 2,
                    Text = "test Text2",
                    IsFavorite = true,
                    IsCheckable = true,
                    Checked = false,
                    DateTime = DateTime.Now,
                    Tags = new List<Tag>(),
                    SubComments = new List<SubComment>(),
                },

                new ()
                {
                    Id = 3,
                    Text = "test Text3",
                    IsFavorite = true,
                    IsCheckable = true,
                    Checked = false,
                    DateTime = DateTime.Now,
                    Tags = new List<Tag>()
                    {
                        new () { Name = "Tag1", },
                        new () { Name = "Tag2", },
                        new () { Name = "Tag3", },
                    },
                    SubComments = new List<SubComment>(),
                },

                new ()
                {
                    Id = 4,
                    Text = "test Text3",
                    IsFavorite = true,
                    IsCheckable = true,
                    Checked = false,
                    DateTime = DateTime.Now,
                    Tags = new List<Tag>()
                    {
                        new () { Name = "Tag1", },
                        new () { Name = "Tag2", },
                        new () { Name = "Tag3", },
                    },

                    SubComments = new List<SubComment>()
                    {
                        new ()
                        {
                            ParentCommentId = 4, Text = "subComment1", IsCheckable = true,
                            TimeTracking = true,
                        },
                        new () { ParentCommentId = 4, Text = "subComment2", IsCheckable = false, },
                        new ()
                        {
                            ParentCommentId = 4, Text = "subComment3", IsCheckable = true,
                            TimeTracking = true,
                            WorkingTimeSpan = TimeSpan.FromMinutes(10),
                        },
                    },
                },

                new ()
                {
                    Id = 5,
                    Text = "test Text3",
                    IsFavorite = true,
                    IsCheckable = true,
                    Checked = false,
                    DateTime = DateTime.Now,

                    SubComments = new List<SubComment>()
                    {
                        new () { ParentCommentId = 4, Text = "subComment1", IsCheckable = true, },
                        new () { ParentCommentId = 4, Text = "subComment2", IsCheckable = false, },
                        new () { ParentCommentId = 4, Text = "subComment3", IsCheckable = true, },
                    },
                },

                new ()
                {
                    Id = 6,
                    GroupId = 1,
                    Text = "test Text6",
                    IsFavorite = false,
                    IsCheckable = false,
                    Checked = false,
                    DateTime = DateTime.Now,
                    Tags = new List<Tag>(),
                    SubComments = new List<SubComment>(),
                },

                new ()
                {
                    Id = 7,
                    GroupId = 1,
                    Text = "test Text7",
                    IsFavorite = false,
                    IsCheckable = false,
                    Checked = false,
                    DateTime = DateTime.Now,
                    Tags = new List<Tag>(),
                    SubComments = new List<SubComment>(),
                },

                new ()
                {
                    Id = 8,
                    GroupId = 2,
                    Text = "test Text8",
                    IsFavorite = false,
                    IsCheckable = false,
                    Checked = false,
                    DateTime = DateTime.Now,
                    Tags = new List<Tag>(),
                    SubComments = new List<SubComment>(),
                },
            };
        }

        public static List<Group> GetSampleGroups()
        {
            return new List<Group>()
            {
                new () { Id = 1, Name = "Sample Group 1", },
                new () { Id = 2, Name = "Sample Group 2", },
                new () { Id = 3, Name = "Sample Group 3", },
            };
        }
    }
}