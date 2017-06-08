using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;


namespace VowelsService
{
    public interface SubmitOrder
        : CorrelatedBy<Guid>
    {
        DateTime SubmitDate { get; }
        string CustomerNumber { get; }
        string OrderNumber { get; }
    }
}
