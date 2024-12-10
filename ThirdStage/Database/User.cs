using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdStage.Database;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string HashPassword { get; set; } 
}
