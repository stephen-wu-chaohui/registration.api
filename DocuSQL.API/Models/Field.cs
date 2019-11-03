using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocuSQL.API.Models
{
    public class Field
    {
        [Key]
        public Int32 Id { get; set; }

        [Required]
        public Int32 DocumentId { get; set; }

        [ForeignKey("DocumentId")]
        public Document Document { get; set; }

        [Required]
        public string FieldId { get; set; }

        public string FieldValue { get; set; }
    }
}
