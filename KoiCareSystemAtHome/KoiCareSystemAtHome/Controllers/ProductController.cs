﻿using KoiCareSystemAtHome.Entities;
using KoiCareSystemAtHome.Models;
using KoiCareSystemAtHome.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

namespace KoiCareSystemAtHome.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController (IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetAllProductsAsync();
                return Ok(new { success = true, Product = products });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
          
        }

        [HttpGet("get-all-shopId{shopId}")]
        public async Task<IActionResult> GetProductsByShopId(int shopId)
        {
            try
            {
                var products = await _productRepository.GetAllByShopIdAsync(shopId);
                return Ok(new { success = true, Product = products });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("get-byId{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            try
            {
                var product = await _productRepository.GetByProductIdAsync(productId);
                if (product == null) return NotFound();
                return Ok(new { success = true, Product = product });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("get-byName{name}")]
        public async Task<IActionResult> GetProductByName(string name)
        {
            try
            {
                var products = await _productRepository.SearchByNameAsync(name);
                if (products == null) return NotFound();
                return Ok(new { success = true, product = products });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("create")]
        public async Task<IActionResult> AddProduct(ProductDTO product)
        {
            try
            {
                if (product == null) return BadRequest();
                var newProduct = new ProductsTbl
                {
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Quantity,
                    Category = product.Category,
                    Image = product.Image,
                    ProductInfo = product.Description,
                    Status = product.Status,
                    ShopId = product.ShopId
                };
                await _productRepository.AddAsync(newProduct);
                return CreatedAtAction(nameof(GetProductById), new { productId = newProduct.ProductId }, newProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("update{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, ProductDTO product)
        {
            try
            {
                var updateProduct = await _productRepository.GetByIdAsync(productId);
                if (updateProduct == null) return BadRequest();
                updateProduct.Name = product.Name;
                updateProduct.Price = product.Price;
                updateProduct.Stock = product.Quantity;
                updateProduct.Category = product.Category;
                updateProduct.Image = product.Image;
                updateProduct.ProductInfo = product.Description;
                updateProduct.Status = product.Status;
                updateProduct.ShopId = product.ShopId;
                await _productRepository.UpdateAsync(updateProduct);
                return Ok(new { success = true, Product = updateProduct });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //Set the status of product. Delete product means status = false.
        [HttpDelete("edit-status{productId}")]
        public async Task<IActionResult> EditStatusProduct(int productId, bool status)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                string message = "";
                if (product == null) return NotFound();
                product.Status = status;
                if (status) message = "Status is set to true";
                else message = "Status is set to false";
                await _productRepository.UpdateAsync(product);
                return Ok(new { success = true, message = message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
