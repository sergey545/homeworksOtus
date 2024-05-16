using System.Collections;
using System.Collections.Generic;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class NonFakeDataFactory
    {
        protected List<Employee> data =  new(FakeDataFactory.Employees);
        public ICollection<Employee> Employee
        {
            get
            {
                return data;
            }
            set
            {
                data.AddRange(value);
            }
        }

    }



}
