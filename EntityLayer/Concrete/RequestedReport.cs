using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EntityLayer.Concrete
{
	[Microsoft.EntityFrameworkCore.Index("UserId")]
	public class RequestedReport
	{
		[Key]
		public int Id { get; set; }
		[ForeignKey("AspNetUsers")]
		public string? UserId { get; set; }
		public required string ReportType { get; set; }
		public required string DateRange { get; set; }
		public string? Status { get; set; }
		public string? ReportUrl { get; set; }
	}
}
