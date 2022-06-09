namespace Hastnama.Solico.Domain.Models.UserManagement
{
    public class UserAddress
    {
        
        public long Id { get; set; }
        public long UserId { get; set; }
    
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string PostalCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public bool IsDeleted { get; set; }
        
        public virtual User User { get; set; }
    }
}