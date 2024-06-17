using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private ResponseDto responseDto;

        public ProductAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            responseDto = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto get()
        {
            try
            {
                IEnumerable<Product> products = _db.Products.ToList();

                responseDto.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                responseDto.Message = ex.Message;
                responseDto.IsSuccess = false;
            }


            return responseDto;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto get(int id)
        {
            try
            {
                var product = _db.Products.FirstOrDefault(p => p.ProductId == id);
                if (product == null)
                {
                    responseDto.IsSuccess = false;
                }
                responseDto.Result = _mapper.Map<ProductDto>(product);

            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ResponseDto post(ProductDto productDto) 
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                _db.Products.Add(product);
                _db.SaveChanges();

                if(productDto.Image != null)
                {
                    var fileName = product.ProductId + Path.GetExtension(productDto.Image.FileName);
                    
                    var filePath = @"wwwroot\ProductImages\" + fileName;

                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                    using(var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDto.Image.CopyTo(fileStream);
                    }

                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                    product.ImageLocalPath = filePath;
                }
                else
                {
                    productDto.ImageUrl = "https://placehold.co/600x400";
                }
				_db.Products.Update(product);
				_db.SaveChanges();

				responseDto.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }

        [HttpPut]
        [Authorize(Roles = "admin")]

        public ResponseDto put([FromBody] ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                _db.Products.Update(product);
                _db.SaveChanges();
                responseDto.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "admin")]

        public ResponseDto delete(int id)
        {
            try
            {
                var product = _db.Products.First(p => p.ProductId == id);
                
                _db.Products.Remove(product);
                _db.SaveChanges();
                
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }
    }
}
