using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Exceptions
{
    public class Result : Exception
    {
        public Result(string Message) : base(Message) {}
    }
}