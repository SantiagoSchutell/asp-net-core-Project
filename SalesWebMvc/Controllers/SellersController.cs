using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using SalesWebMvc.Models.ViewModels;
using System.Collections.Generic;

namespace SalesWebMvc.Controllers
{
	public class SellersController : Controller
	{

		private readonly SellerService _sellerService;
		private readonly DepartmentService _departmentService;

		public SellersController(SellerService sellerService, DepartmentService departmentService)
		{
			_sellerService = sellerService;
			_departmentService = departmentService;
		}


		public IActionResult Index()
		{

			var list = _sellerService.FindAll();
			return View(list);
		}

		public IActionResult Create()
		{
			var departaments = _departmentService.FindAll();
			var ViewModel = new SellerFormViewModel { Departaments = departaments };

			return View(ViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Seller seller)
		{
			_sellerService.Insert(seller);
			return RedirectToAction(nameof(Index));
		}
	}
}
