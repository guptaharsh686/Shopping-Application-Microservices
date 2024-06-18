using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto> products = new List<ProductDto>();
            var response = await _productService.GetAllProducts();

            if(response != null && response.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(products);
        }

        public async Task<IActionResult> ProductCreate()
        {
            var product = new ProductDto();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateProduct(productDto);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "New Product added sucessfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }

            return View(productDto);
        }

        
        public async Task<IActionResult> ProductDelete(int ProductId)
        {  
            var response = await _productService.GetProductById(ProductId);
            if (response != null && response.IsSuccess) 
            {
                var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(product);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
           
            var response = await _productService.DeleteProduct(productDto.ProductId);
            if(response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted sucessfully!";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(productDto);
        }

        public async Task<IActionResult> ProductEdit(int ProductId)
        {
            var response = await _productService.GetProductById(ProductId);
            if(response != null && response.IsSuccess )
            {
                var product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(product);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return NotFound();
        }

        [HttpPost]
		public async Task<IActionResult> ProductEdit(ProductDto productDto)
		{
            if(ModelState.IsValid)
            {
                var response = await _productService.UpdateProduct(productDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product details updated sucessfully!";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
			
            return View(productDto);
		}

	}
}
