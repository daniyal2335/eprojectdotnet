namespace Eproject.Models
{
    public class SelectCatererViewModel
    {
        public string CatererId { get; set; } // Caterer ka unique ID (Admin ke liye integer, Registered ke liye GUID ho sakta hai)
        public string Name { get; set; } // Caterer ka naam
        public string Source { get; set; } // Caterer ka source (Admin ya Registered)
    }
}
