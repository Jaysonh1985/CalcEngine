﻿// Copyright (c) 2016 Project AIM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalculationCSharp.Areas.Project.Models
{
    public class Comments
    {
        public string CommentName { get; set; }
        public string CommentType { get; set; }
        public DateTime CommentDateTime { get; set; }
        public string CommentUser { get; set; }
    }
}