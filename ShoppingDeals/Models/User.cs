using MongoDB.Bson;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable NonReadonlyMemberInGetHashCode

namespace ShoppingDeals.Models
{
    /// <summary>
    /// A user that uses the Deals DB
    /// </summary>
    public class User
    {
        /// <summary>
        /// The username
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The salted and hashed password
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// The MongoDB unique identifier
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Constructs a <see cref="User"/> with the given username and password hash
        /// </summary>
        /// <param name="name">The username</param>
        /// <param name="passwordHash">The password hash</param>
        public User(string name, string passwordHash)
        {
            Name = name;
            PasswordHash = passwordHash;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((User) obj);
        }

        private bool Equals(User other)
        {
            return
                string.Equals(Name, other.Name) &&
                string.Equals(PasswordHash, other.PasswordHash);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name?.GetHashCode() ?? 0)*397) ^ (PasswordHash?.GetHashCode() ?? 0);
            }
        }
    }
}