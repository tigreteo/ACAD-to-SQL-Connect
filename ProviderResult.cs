using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACADtoSQL
{
    public class ProviderResult
    {
        public ProviderResult()
        {
            this.Error = null;
            this.Result = null;
            this.RowsAffected = 0;
            this.IsSuccess = false;
        }

        public Exception Error { get; set; }
        public Object Result { get; set; }
        public int RowsAffected { get; set; }
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Use tage object to return any other object with result or error
        /// </summary>
        public object Tag { get; set; }    
    }
}
