using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/Stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Stocks = await _context.Stocks.ToListAsync();
            var stockDto= Stocks.Select(s => s.ToStockDto());
            return Ok(Stocks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var Stock = await  _context.Stocks.FindAsync(id);
            if(Stock == null)
            {
                return NotFound();
            }
            return Ok(Stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());
        }

         [HttpPut]
         [Route("{id}")]
         public async Task<IActionResult> Update([FromRoute] int id,[FromBody] UpdateStockRequestDto updateDto)
         {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(X => X.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }
            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.Purchase;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            await _context.SaveChangesAsync();
            return Ok(stockModel.ToStockDto());
         }

         [HttpDelete]
         [Route("{id}")]
         public async Task<IActionResult> Delete ([FromRoute] int id)
         {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(X => X.Id == id);
            if (stockModel == null)
            {
                return NotFound();
            }
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return NoContent();
         }
    }
}