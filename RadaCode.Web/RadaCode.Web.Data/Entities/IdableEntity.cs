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

    public abstract class ActionStampableIdableEntity : ActionStampable
    {
        [Key]
        public Guid Id { get; set; }

        protected ActionStampableIdableEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
