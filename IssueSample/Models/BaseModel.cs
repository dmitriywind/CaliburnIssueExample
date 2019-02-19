using System;

namespace IssueSample.Models
{
    public class BaseModel
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public string Comment { get; set; }
    }
}
