﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DataAccess
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<Post> Posts { get; set; }
    }

}
