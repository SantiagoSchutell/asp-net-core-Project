using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using SalesWebMvc.Models.ViewModels;
using System.Collections.Generic;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;
using System.Data.Odbc;

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
			if (!ModelState.IsValid)
			{
				var departaments = _departmentService.FindAll();
				var viewModel = new SellerFormViewModel { Seller = seller, Departaments = departaments };

				return View(viewModel);
			}

			_sellerService.Insert(seller);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();

			}

			var obj = _sellerService.FindById(id.Value);
			if (obj == null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id not provided" });
			}

			return View(obj);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(int id)
		{
			_sellerService.Remove(id);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return  RedirectToAction(nameof(Error), new { message = "Id not Found" });

            }

			var obj = _sellerService.FindById(id.Value);
			if (obj == null)
			{
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

			return View(obj);
		}

		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

			var obj = _sellerService.FindById(id.Value);
			if (obj == null)
			{
                return RedirectToAction(nameof(Error), new { message = "Id not Found" });
            }

			List<Department> departments = _departmentService.FindAll();
			SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departaments = departments };
			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, Seller seller)
		{
			if (!ModelState.IsValid)
			{
				var departaments = _departmentService.FindAll();
				var viewModel = new SellerFormViewModel { Seller = seller, Departaments = departaments };

				return View(viewModel);
			}

			if (id != seller.Id)
			{
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }
			try
			{
				_sellerService.Update(seller);
				return RedirectToAction(nameof(Index));
			}
			catch (NotFoundException e)
			{
				return RedirectToAction(nameof(Error), new { message = e.Message });
			}
			catch (DbConcurrencyException e)
			{
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

		public IActionResult Error(string message)
		{
			var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
		 return View(viewModel);
		}
		
	}
}