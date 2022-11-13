using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTest
{
    public interface IEntity
    {
        /// <summary>
        /// This entity's primary key identifier.
        /// </summary>
        int Id { get; }
    }
    public interface INamedEntity : IEntity
    {
        /// <summary>
        /// This entity's name.
        /// </summary>
        string Name { get; }
    }

    public class MyClass : INamedEntity, IEquatable<MyClass>//for Invoke() AsExpandable() to work
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public int Id => throw new NotImplementedException();

        public MyClass() 
        {
        }

        public bool Equals(MyClass other) =>
            other != null && this.Name == other.Name;

    }
}
