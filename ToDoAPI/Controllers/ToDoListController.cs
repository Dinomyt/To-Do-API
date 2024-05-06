using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using ToDoAPI.Data;
using ToDoAPI.Models;



namespace ToDoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoListController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ToDoListController(AppDbContext context){
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<ToDoList>> getLists(){
            var list = await _context.ToDoLists.AsNoTracking().ToListAsync();
            return list;
        }
        [HttpPost]
        public async Task<IActionResult> Create(ToDoList list){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            await _context.AddAsync(list);

            var result = await _context.SaveChangesAsync();

            if (result > 0) {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id){
            var list = await _context.ToDoLists.FindAsync(id);
            if (list == null){
                return NotFound();
            }
            _context.Remove(list);
            var result = await _context.SaveChangesAsync();
            if (result > 0) {
                return Ok("List was deleted successfully");
            }
            return BadRequest();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ToDoList list){
            
            var thisList = await _context.ToDoLists.FindAsync(id);

            if (thisList == null){
                return NotFound();
            }
            thisList.List = list.List;
 
            var result = await _context.SaveChangesAsync();
            if (result > 0) {
                return Ok("List was updated successfully");
            }
            return BadRequest();
        }

    }
}