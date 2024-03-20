using System.Collections.Generic;

namespace SalesWebMvc.Models.ViewModels
{
	public class SellerFormViewModel
	{

        public Seller Seller { get; set; }
        public ICollection<Department> Departaments { get; set; }

    }
}
