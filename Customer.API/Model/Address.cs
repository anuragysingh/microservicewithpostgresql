namespace Customer.API.Core.Model
{
    public class Address
    {
        //Foreign key relation with Address
        public int UserId { get; set; }
        public User User { get; set; }

        public int AddressID { get; set; }

        public int Address1 { get; set; }

        public int Address2 { get; set; }
    }
}
