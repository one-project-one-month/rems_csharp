namespace REMS.Models.Property
{
    public class PropertyStatusChangeRequestModel
    {
        public int PropertyId { get; set; }
        public string PropertyStatus { get; set; }
        public string ApprovedBy { get; set; }
    }
}