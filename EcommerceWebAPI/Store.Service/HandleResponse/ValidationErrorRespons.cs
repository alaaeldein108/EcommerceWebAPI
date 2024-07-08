using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.HandleResponse
{
    public class ValidationErrorRespons : CustomException
    {
        public ValidationErrorRespons() 
            : base(400)
        {

        }
        public IEnumerable<string> Errors { get; set; }
    }
}
