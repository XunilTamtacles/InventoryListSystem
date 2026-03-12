using System;
using System.Collections.Generic;

namespace Inventory_List_System.Models.Database;

public partial class TableUser
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
}
