using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniFoodNew.DataB
{
    public class ServerAnswer<T>
    {
        public List<string> Errors { get;} = [];
        public bool IsSuccess => Errors.Count == 0;
        public T ServerResponse { get; set; }
        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }
}
