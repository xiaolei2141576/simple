using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Models.Common
{
    /// <summary>
    /// 基础实体（被所有业务实体继承）
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }
    }
}
