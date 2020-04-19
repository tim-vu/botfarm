using System;
using System.Collections.Generic;
using System.Text;

namespace FORFarm.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }

        public NotFoundException(string name, string propertyName, string value) : base($"Entity \"{name}\" with {propertyName}:{value} was not found")
        {
        }
    }
}
