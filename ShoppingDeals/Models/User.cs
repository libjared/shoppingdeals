namespace ShoppingDeals.Models
{
    public class User
    {
        public string Name { get; }
        public string PasswordHash { get; }

        public User(string name, string passwordHash)
        {
            Name = name;
            PasswordHash = passwordHash;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((User)obj);
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
                return ((Name?.GetHashCode() ?? 0) * 397) ^ (PasswordHash?.GetHashCode() ?? 0);
            }
        }
    }
}