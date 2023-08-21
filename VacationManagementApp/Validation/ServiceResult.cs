using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VacationManagementApp.Validators
{
    public class ServiceResult
    {
        private bool succeed;
        private Dictionary<string, string> dataWithErrors;

        public ServiceResult()
        {
            dataWithErrors = new Dictionary<string, string>();
            succeed = true;
        }

        public void AddError(string issuedData, string message)
        {
            succeed = false;
            dataWithErrors.Add(issuedData, message);
        }

        public void ManageModelState(ModelStateDictionary modelState)
        {
            if (succeed) return;

            foreach (var dataMessagePair in dataWithErrors)
            {
                modelState.AddModelError(dataMessagePair.Key, dataMessagePair.Value);
            }
        }


        public bool Succeed
        {
            get
            {
                return this.succeed;
            }
        }







    }
}
