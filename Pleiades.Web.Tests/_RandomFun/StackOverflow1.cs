using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Web.Tests._RandomFun
{
    /*
    class Result
    {
        public string Data { get; set; }
    }

    interface IRepository
    {
        Result[] Search(string data);
    }

    class DiskResult : Result
    {
        public int FileSize { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    class DiskRepository : IRepository
    {
        public Result[] Search(string data)
        {
            // ...
            DiskResult[] results = GetDataFromSomewhere();
            return results;
        }
    }
    */


    interface IRepository
    {
    }

    interface IRepository<T> where T : Result
    {
        T[] Search(string data);
    }

    interface IResultDisplayService<T> where T : Result
    {
        void Display(T result);
    }

    class Result
    {
        public string Data { get; set; }
    }

    class DiskResult : Result
    {
        public int FileSize { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }

    class DiskRepository : IRepository<DiskResult>
    {
        public DiskResult[] Search(string data)
        {
            var results = new DiskResult[10];

            // .... do stuff with the Disk 

            return results;
        }
    }

}
