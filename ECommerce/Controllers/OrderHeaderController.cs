using ECommerce_API.Data;
using ECommerce_API.Models;
using ECommerce_API.Models.Dto;
using ECommerce_API.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ECommerce_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderHeaderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ApiResponse _response;
        public OrderHeaderController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();
        }
        [HttpGet]
        public ActionResult<ApiResponse> GetOrders(string userId = "")
        {
            IEnumerable<OrderHeader> orderHeaderList = _db.OrderHeaders.Include(u => u.OrderDetails).ThenInclude(u => u.MenuItem).OrderByDescending(u => u.OrderHeaderId);

            if (!string.IsNullOrEmpty(userId))
            {
                orderHeaderList = orderHeaderList.Where(u => u.ApplicationUserid == userId);
            }
            _response.Result = orderHeaderList;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{orderId:int}")]
        public ActionResult<ApiResponse> GetOrder(int orderId)
        {
            if (orderId == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }


            OrderHeader? orderHeader = _db.OrderHeaders.Include(u => u.OrderDetails).ThenInclude(u => u.MenuItem).FirstOrDefault(u => u.OrderHeaderId == orderId);

            if (orderHeader == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.ErrorMessages.Add("Invalid order Id");
                return NotFound(_response);
            }

            _response.Result = orderHeader;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        public ActionResult<ApiResponse> CreateOrder([FromBody] OrderHeaderCreateDTO orderHeaderDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    OrderHeader orderheader = new()
                    {
                        PickUpName = orderHeaderDTO.PickUpName,
                        PickUpPhoneNumber = orderHeaderDTO.PickUpPhoneNumber,
                        PickUpEmail = orderHeaderDTO.PickUpEmail,
                        OrderDate = DateTime.Now,
                        ApplicationUserid = orderHeaderDTO.ApplicationUserid,
                        OrderTotal = orderHeaderDTO.OrderTotal,
                        Status = SD.status_confirmed,
                        TotalItem = orderHeaderDTO.TotalItem
                    };

                    _db.OrderHeaders.Add(orderheader);
                    _db.SaveChanges();

                    foreach (var orderDetails in orderHeaderDTO.OrderDetails)
                    {
                        OrderDetail orderDetail = new()
                        {
                            OrderHeaderId = orderheader.OrderHeaderId,
                            MenuItemId = orderDetails.MenuItemId,
                            Quantity = orderDetails.Quantity,
                            ItemName = orderDetails.ItemName,
                            Price = orderDetails.Price
                        };
                        _db.OrderDetails.Add(orderDetail);
                    }
                    _db.SaveChanges();
                    _response.Result = orderheader;
                    _response.StatusCode = HttpStatusCode.Created;
                    orderheader.OrderDetails = [];
                    return CreatedAtAction(nameof(GetOrder), new { orderId = orderheader.OrderHeaderId }, _response);

                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = ModelState.Values.SelectMany(u => u.Errors).Select(u => u.ErrorMessage).ToList();
                    return BadRequest(_response);
                }


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpPut("{orderId:int}")]
        public ActionResult<ApiResponse> UpdateeOrder(int orderId, [FromBody] OrderHeaderUpdateDTO orderHeaderDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (orderId != orderHeaderDTO.OrderHeaderId)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.ErrorMessages.Add("Invalid Id");
                        return BadRequest(_response);
                    }

                    OrderHeader? orderHeaderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.OrderHeaderId == orderId);

                    if (orderHeaderFromDb == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.ErrorMessages.Add("Order not found");
                        return NotFound(_response);
                    }

                    if (!string.IsNullOrEmpty(orderHeaderDTO.PickUpName))
                    {
                        orderHeaderFromDb.PickUpName = orderHeaderDTO.PickUpName;
                    }
                    if (!string.IsNullOrEmpty(orderHeaderDTO.PickUpPhoneNumber))
                    {
                        orderHeaderFromDb.PickUpPhoneNumber = orderHeaderDTO.PickUpPhoneNumber;
                    }

                    if (!string.IsNullOrEmpty(orderHeaderDTO.PickUpEmail))
                    {
                        orderHeaderFromDb.PickUpEmail = orderHeaderDTO.PickUpEmail;
                    }

                    if (!string.IsNullOrEmpty(orderHeaderDTO.Status))
                    {
                        if (orderHeaderFromDb.Status.Equals(SD.status_confirmed,StringComparison.InvariantCultureIgnoreCase)
                            && orderHeaderDTO.Status.Equals(SD.status_readyForPickup, StringComparison.InvariantCultureIgnoreCase))
                        {
                            orderHeaderFromDb.Status = SD.status_readyForPickup;
                        }

                        if (orderHeaderFromDb.Status.Equals(SD.status_readyForPickup, StringComparison.InvariantCultureIgnoreCase)
                            && orderHeaderDTO.Status.Equals(SD.status_completed, StringComparison.InvariantCultureIgnoreCase))
                        {
                            orderHeaderFromDb.Status = SD.status_completed;
                        }

                        if (orderHeaderDTO.Status.Equals(SD.status_cancelled, StringComparison.InvariantCultureIgnoreCase))
                        {
                            orderHeaderFromDb.Status = SD.status_cancelled;
                        }


                    }

                    _db.SaveChanges();
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);

                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = ModelState.Values.SelectMany(u => u.Errors).Select(u => u.ErrorMessage).ToList();
                    return BadRequest(_response);
                }


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }
    }
}

    

