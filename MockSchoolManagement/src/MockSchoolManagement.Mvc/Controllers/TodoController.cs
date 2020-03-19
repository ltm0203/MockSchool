using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockSchoolManagement.Infrastructure.Repositories;
using MockSchoolManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockSchoolManagement.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        //注入仓储服务，因TodoItem的主键id为long类型，仓储服务参数也需要对应一致
        private readonly IRepository<TodoItem, long> _todoItemRepository;

        public TodoController(IRepository<TodoItem, long> todoRepository)
        {
            this._todoItemRepository = todoRepository;
        }

        /// <summary>
        /// 获取所有待办事项
        /// </summary>
        /// <returns> </returns>
        [HttpGet]
        public async Task<ActionResult<List<TodoItem>>> GetAll()
        {       //获取所有的代码事项列表
            var models = await _todoItemRepository.GetAllListAsync();
            return models;
        }

        #region 根据Id获取待办事项

        /// <summary>
        /// 通过Id获取待办事项
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetById(long id)
        {
            var todoItem = await _todoItemRepository.FirstOrDefaultAsync(a => a.Id == id);

            if (todoItem == null)
            {   //返回为404 状态码
                return NotFound();
            }

            return todoItem;
        }

        #endregion 根据Id获取待办事项

        #region 更新待办事项

        /// <summary>
        /// 更新待办事项
        /// </summary>
        /// <param name="id"> </param>
        /// <param name="todoItem"> </param>
        /// <returns> </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            await _todoItemRepository.UpdateAsync(todoItem);

            //返回状态码204
            return NoContent();
        }

        #endregion 更新待办事项

        #region 添加待办实现

        /// <summary>
        /// 添加待办实现
        /// </summary>
        /// <param name="todoItem"> </param>
        /// <returns> </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoItem>> Create(TodoItem todoItem)
        {
            await _todoItemRepository.InsertAsync(todoItem);

            //创建一个reatedAtActionResult对象，它生成一个状态码为Status201 Created的响应。
            return CreatedAtAction(nameof(GetAll), new { id = todoItem.Id }, todoItem);
        }

        #endregion 添加待办实现

        #region 删除指定id的待办事项

        /// <summary>
        /// 删除指定id的待办事项
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> Delete(long id)
        {
            var todoItem = await _todoItemRepository.FirstOrDefaultAsync(a => a.Id == id);
            if (todoItem == null)
            {
                return NotFound();
            }
            await _todoItemRepository.DeleteAsync(todoItem);
            return todoItem;
        }

        #endregion 删除指定id的待办事项
    }
}