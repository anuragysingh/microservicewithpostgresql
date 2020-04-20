namespace Customer.API.Core.Model
{
    /// <summary>
    /// User model.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets user id details.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets Name detials.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Email detials.
        /// </summary>
        public string Email { get; set; }
    }
}
