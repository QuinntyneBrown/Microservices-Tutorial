#region Licence
/* The MIT License (MIT)
Copyright © 2014 Ian Cooper <ian_hammond_cooper@yahoo.co.uk>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the “Software”), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

#endregion

using System;
using paramore.brighter.commandprocessor;

namespace Orders_Core.Ports.Commands
{
    public class EditOrderCommand : Command, ICanBeValidated
    {
        public int OrderId { get; private set; }
        public string OrderDescription { get; private set; }
        public DateTime? OrderDueDate { get; private set; }
        public string OrderName { get; private set; }

        public EditOrderCommand(int orderId, string orderName, string orderDecription, DateTime? dueDate = null)
            : base(Guid.NewGuid())
        {
            OrderId = orderId;
            OrderName = orderName;
            OrderDescription = orderDecription;
            OrderDueDate = dueDate;
        }

        public bool IsValid()
        {
            if ((OrderId >= 0) || (OrderDescription == null) || (OrderName == null))
            {
                return false;
            }

            return true;
        }
    }
}