using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace VacationManagementApp.AssistanceClasses
{
    public class ServiceResult<T>
    {
        private bool _succeed = true;
        public bool Succeed
        {
            get { return Errors.Count == 0; }
            private set { _succeed = value; }
        }
        public Dictionary<string, string> Errors { get; set; }
        public T? Data { get; set; }

        public ServiceResult()
        {
            Errors = new Dictionary<string, string>();
        }

        public void UpdateModelError(ModelStateDictionary modelState)
        {
            foreach (var error in Errors)
            {
                modelState.AddModelError(error.Key, error.Value);
            }
        }

        public void AppendError(string key, string value)
        {
            Errors.Add(key, value);
            Succeed = false; // This ensures that Succeed is updated when an error is added
        }
    }
}
