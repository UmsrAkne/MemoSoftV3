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
                        new () { ParentCommentId = 4, Text = "subComment1", IsCheckable = true, },
                        new () { ParentCommentId = 4, Text = "subComment2", IsCheckable = false, },
                        new () { ParentCommentId = 4, Text = "subComment3", IsCheckable = true, },
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
            };
        }
    }
}