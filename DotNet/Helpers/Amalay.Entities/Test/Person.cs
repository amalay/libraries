using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public class Person : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public string Sex { get; set; }

        public Address HomeAddress { get; set; }

        public Address OfficeAddress { get; set; }

        public static Person Create(IDataRecord dataRecord)
        {
            return new Person()
            {
                Id = Convert.ToInt32(dataRecord["Id"]),
                FirstName = Convert.ToString(dataRecord["FirstName"]),
                LastName = Convert.ToString(dataRecord["LastName"]),
                Age = Convert.ToInt32(dataRecord["Age"]),
                Sex = Convert.ToString(dataRecord["Sex"]),
                HomeAddress = new Address(),
                OfficeAddress = new Address()
            };
        }
    }
}
