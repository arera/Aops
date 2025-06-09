namespace AOps.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;

        // Add domain behavior if needed, e.g.:
        // public void ChangeEmail(string newEmail) { ... }
    }
}
