namespace ibillcraft.Models
{
    public class UnitConfigurationModel
    {
        public Guid? UserId { get; set; }
        public Guid? u_id { get; set; }
        public string? u_unit { get; set; }
        public string? u_height { get; set; }
        public string? u_amount { get; set; }
        public string? u_size { get; set; }
        public string? u_width { get; set; }
        public DateTime? u_updateddate { get; set; }
        public DateTime? u_createddate { get; set; }
        public string? u_isactive { get; set; }
   
    }
    public class Unit
    {
        public UnitConfigurationModel? data { get; set; }
    }
}
