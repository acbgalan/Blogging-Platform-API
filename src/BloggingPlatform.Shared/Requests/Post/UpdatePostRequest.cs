﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloggingPlatform.Shared.Requests.Post
{
    public class UpdatePostRequest : CreatePostRequest
    {
        public int Id { get; set; }
    }
}