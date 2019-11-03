using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocuSQL.API.Models
{
    public class Document
    {
        [Key]
        public Int32 Id { get; set; }

        [StringLength(256)]
        public string SubmissionId { get; internal set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime SubmissionTime { get; set; }

        public List<Field> Fields { get; set; }
    }
}
