using ECommerce_API.Data;
using ECommerce_API.Migrations;
using ECommerce_API.Models;
using ECommerce_API.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Metadata;

namespace ECommerce_API.Controllers
{
    [Route("api/MenuItem")]
    [ApiController]
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ApiResponse _response;
        private readonly IWebHostEnvironment _env;
        public MenuItemController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _response = new ApiResponse();
            _env = env;
        }
        [HttpGet]
        public IActionResult GetMenuItems()
        {
            var menuItems = _db.MenuItems.ToList();
            var orderDetailsWithRatings = _db.OrderDetails.Where(u => u.Rating != null).ToList();
            foreach(var menuItem in menuItems)
            {
                var ratings = orderDetailsWithRatings.Where(u => u.MenuItemId == menuItem.Id).Select(u => u.Rating!.Value);
                var avgRating = ratings.Any() ? ratings.Average() : 0;
                menuItem.Ratings = avgRating;
            }
            _response.Result = menuItems;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);

        }

        [HttpGet("{id:int}", Name="GetMenuItem")]
        public IActionResult GetMenuItem(int id)
        {
            if(id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            var menuItem = _db.MenuItems.FirstOrDefault(u => u.Id == id);

            var orderDetailsWithRatings = _db.OrderDetails.Where(u => u.Rating != null && u.MenuItemId == menuItem!.Id).ToList();
 
                var ratings = orderDetailsWithRatings.Select(u => u.Rating!.Value);
                var avgRating = ratings.Any() ? ratings.Average() : 0;
                menuItem!.Ratings = avgRating;

            _response.Result = menuItem;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);

        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateMenuItem([FromForm] MenuItemCreateDTO menuItemCreateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (menuItemCreateDTO.File == null || menuItemCreateDTO.File.Length == 0)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages = ["File is required"];
                        return BadRequest(_response);
                    }

                    var imagePath = Path.Combine(_env.WebRootPath, "images");
                    if (!Directory.Exists(imagePath)) { }
                    {
                        Directory.CreateDirectory(imagePath);
                    }
                    var filePath = Path.Combine(imagePath, menuItemCreateDTO.File.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await menuItemCreateDTO.File.CopyToAsync(stream);
                    }
                    MenuItem menuItem = new MenuItem()
                    {
                        Name = menuItemCreateDTO.Name,
                        Description = menuItemCreateDTO.Description,
                        Category = menuItemCreateDTO.Category,
                        SpecialTag = menuItemCreateDTO.SpecialTag,
                        Price = menuItemCreateDTO.Price,
                        Image = @"images/" + menuItemCreateDTO.File.FileName
                    };
                    _db.MenuItems.Add(menuItem);
                    await _db.SaveChangesAsync();

                    _response.Result = menuItemCreateDTO;
                    _response.StatusCode = HttpStatusCode.Created;
                    return CreatedAtRoute("GetMenuItem", new { id = menuItem.Id }, _response);
                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = [ex.ToString()];
            }
            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateMenuItem(int id, [FromForm] MenuItemUpdateDTO menuItemUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (menuItemUpdateDTO == null || menuItemUpdateDTO.Id != id)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest(_response);
                    }
                    var menuItemFromDb = await _db.MenuItems.FirstOrDefaultAsync(u => u.Id == id);
                    
                    if(menuItemFromDb == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        return NotFound(_response);
                    }

                    menuItemFromDb.Name = menuItemUpdateDTO.Name;
                    menuItemFromDb.Description = menuItemUpdateDTO.Description;
                    menuItemFromDb.Price = menuItemUpdateDTO.Price;
                    menuItemFromDb.Category = menuItemUpdateDTO.Category;
                    menuItemFromDb.SpecialTag = menuItemUpdateDTO.SpecialTag;

                    if(menuItemUpdateDTO.File != null && menuItemUpdateDTO.File.Length > 0)
                    {
                        var imagePath = Path.Combine(_env.WebRootPath, "images");
                        if (!Directory.Exists(imagePath)) { }
                        {
                            Directory.CreateDirectory(imagePath);
                        }
                        var filePath = Path.Combine(imagePath, menuItemUpdateDTO.File.FileName);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        var filePath_OldFile = Path.Combine(_env.WebRootPath, menuItemFromDb.Image);
                        if (System.IO.File.Exists(filePath_OldFile))
                        {
                            System.IO.File.Delete(filePath_OldFile);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await menuItemUpdateDTO.File.CopyToAsync(stream);
                        }

                        menuItemFromDb.Image = "images/" + menuItemUpdateDTO.File.FileName;
                    }

                    _db.MenuItems.Update(menuItemFromDb);
                    await _db.SaveChangesAsync();

                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = [ex.ToString()];
            }
            return BadRequest(_response);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteMenuItem(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (id == 0)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        return BadRequest(_response);
                    }
                    var menuItemFromDb = await _db.MenuItems.FirstOrDefaultAsync(u => u.Id == id);

                    if (menuItemFromDb == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        return NotFound(_response);
                    }
                        var filePath_OldFile = Path.Combine(_env.WebRootPath, menuItemFromDb.Image);
                        if (System.IO.File.Exists(filePath_OldFile))
                        {
                            System.IO.File.Delete(filePath_OldFile);
                        }

                    _db.MenuItems.Remove(menuItemFromDb);
                    await _db.SaveChangesAsync();

                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = [ex.ToString()];
            }
            return BadRequest(_response);
        }
    }
}
