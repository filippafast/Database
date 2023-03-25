using System.ComponentModel.DataAnnotations;

namespace Management_System.Models.Entities
{
    internal class CaseEntity
    {
        public CaseEntity() 
        {
            Id = Guid.NewGuid();
            var _datetime = DateTime.Now;
            Created = _datetime;
            Modified = _datetime;
        }
        public Guid Id { get; set; } 

        [StringLength(50)]
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; } 

        public int StatusTypeId { get; set; }  
        public Guid CustomerId { get; set; }
       

        public CustomerEntity Customer { get; set; } = null!;
        public StatusTypeEntity StatusType { get; set; } = null!;

        public ICollection<CommentEntity> Comments { get; set; } = new HashSet<CommentEntity>();
    }




}
