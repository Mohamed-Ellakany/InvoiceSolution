using Invoice.BLL.Dtos;
using Invoice.BLL.Hubs;
using Invoice.BLL.Services;
using Invoice.DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using InvoiceEntity = Invoice.DAL.Entities.Invoice;

namespace Invoice.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController(IInvoiceServices invoiceServices) : ControllerBase
    {
        private readonly IInvoiceServices _invoiceServices = invoiceServices;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var invoices = await _invoiceServices.GetAllInvoicesAsync();

            if (invoices == null || !invoices.Any())
                return NotFound("No invoices found.");

            return Ok(invoices);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
          var invoice = await _invoiceServices.GetInvoiceByIdAsync(id);
           
            return invoice == null ? NotFound("No invoice with this id") : Ok(invoice);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateInvoiceDto invoice)
        {
            var result = await _invoiceServices.CreateInvoiceAsync(invoice);
            
            if (result is null )
                return BadRequest("Invoice with this serial already exists");

            return Created();
        }



        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, CreateInvoiceDto updated)
        {
           
            var result = await _invoiceServices.UpdateInvoiceAsync(id, updated);

            if (result is null)
                return BadRequest("No invoice with this id or serial already exists");

            return Created();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
           var result = await _invoiceServices.DeleteInvoiceAsync(id);

            if (!result)
                return NotFound("No invoice with this id");

            return NoContent();
        }

    }
}
