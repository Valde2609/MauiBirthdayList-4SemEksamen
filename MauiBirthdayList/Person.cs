using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBirthdayList;
public class Person
{
	public int Id { get; set; }
	public string UserId { get; set; }
	public string Name { get; set; }
	public int BirthYear { get; set; }
	public int BirthMonth { get; set; }
	public int BirthDayOfMonth { get; set; }
	public string? Remarks { get; set; }
	public string? PictureUrl { get; set; }
	public int Age { get; set; }
}
