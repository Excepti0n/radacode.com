using System;
using System.ComponentModel.DataAnnotations;

namespace RadaCode.Web.Data.Entities
{
    public abstract class IdableEntity
    {
        [Key]
        public Guid Id { get; set; }

        protected IdableEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
