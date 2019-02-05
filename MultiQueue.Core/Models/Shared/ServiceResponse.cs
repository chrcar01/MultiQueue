using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiQueue.Core.Models.Shared
{
    public class ServiceResponse<TData>
    {
        public ServiceResponse()
        {
        }
        
        public ServiceResponse(TData data)
        {
            Data = data;
        }

        private bool? _hasErrors;
        public bool HasErrors
        {
            get => _hasErrors ?? Exception != null || ErrorMessages.Any();
            set => _hasErrors = value;
        }
        public TData Data { get; set; }
        private Exception _exception;
        [JsonIgnore]
        public Exception Exception
        {
            get
            {
                return _exception;
            }
            set
            {
                _exception = value;
                ErrorMessages.Add(_exception?.ToString());
            }
        }
        
        public ICollection<string> ErrorMessages { get; set; } = new List<string>();
    }
}
