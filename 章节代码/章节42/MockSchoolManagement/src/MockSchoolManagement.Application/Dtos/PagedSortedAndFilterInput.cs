using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockSchoolManagement.Application.Dtos
{
    /// <summary>
    /// 分页排序查询参数
    /// </summary>
    public class PagedSortedAndFilterInput
    {
        public PagedSortedAndFilterInput()
        {
            CurrentPage = 1;
            MaxResultCount = 10;
        }


        #region 分页
        /// <summary>
        /// 每页分页条数
        /// </summary>
        [Range(0, 1000)]
        public int MaxResultCount { get; set; } 
        /// <summary>
        /// 当前页
        /// </summary>
        [Range(0, 1000)]
        public int CurrentPage { get; set; }

     

        #endregion


        #region 搜索和排序

        /// <summary>
        /// 排序字段Id
        /// </summary>
        public string Sorting { get; set; }

        /// <summary>
        /// 搜索文本名称
        /// </summary>
        public string FilterText { get; set; }
        #endregion






    }
}
