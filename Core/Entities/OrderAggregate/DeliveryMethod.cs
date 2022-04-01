using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    public class DeliveryMethod : BaseEntity
    {
        [MaxLength(100)]
        public string ShortName { get; set; }

        [MaxLength(20)]
        public string DeliveryTime { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }
    }
}
