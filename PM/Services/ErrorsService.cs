using PM.Models;
using PM.Models.Base;
using System.Collections.Generic;

namespace PM.Services
{
    public class ErrorsService : ObservableObject
    {
        private List<Error> errors;
        public List<Error> Errors
        {
            get => errors;
            set
            {
                errors = value;
                OnPropertyChanged(nameof(Errors));
            }
        }

        public ErrorsService()
        {
            Errors = new List<Error>();
        }

    }
}
