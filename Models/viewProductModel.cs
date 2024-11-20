
    namespace Eproject.Models
    {
        public class viewProductModel
        {
            internal string existingImagePath;

            public int Id { get; set; }

            public string ItemName { get; set; }

          public string Des { get; set; }
            public int Price { get; set; }
   
            public IFormFile photo { get; set; }
            // Add CategoryId property to match your form
            public int CategoryId { get; set; }

        }
    }
