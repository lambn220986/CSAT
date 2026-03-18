using System;

namespace CSAT.Models
{
    public class CustomerSatisfaction
    {
        public int Id { get; set; }
        public int VoteValue { get; set; }
        public int? UserId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DeviceName { get; set; }
        public string IPAddress { get; set; }
        public string Note { get; set; }
    }
}
