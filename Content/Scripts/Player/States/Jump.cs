using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Jump : IState
{
    public string StateName { get; } = "Jump";

    public void Update(ref Player host, float delta)
    {
    }
}

