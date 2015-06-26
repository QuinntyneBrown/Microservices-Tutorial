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
using FakeItEasy;
using Machine.Specifications;
using Orders_Core.Adapters.DataAccess;
using Orders_Core.Model;
using Orders_Core.Ports.Commands;
using Orders_Core.Ports.Handlers;
using paramore.brighter.commandprocessor.Logging;

namespace Orders_Core.Adapters.Tests
{
    [Subject(typeof(AddOrderCommandHandler))]
    public class When_adding_a_new_order_to_the_list
    {
        private static AddOrderCommandHandler s_handler;
        private static AddOrderCommand s_cmd;
        private static IOrdersDAO s_ordersDAO;
        private static Order s_orderToBeAdded;
        private const string CUSTOMER_NAME = "Test customer";
        private const string ORDER_DESCRIPTION = "Test that we store a description";
        private static readonly DateTime s_NOW = DateTime.Now;

        private Establish _context = () =>
        {
            var logger = A.Fake<ILog>();
            s_ordersDAO = new OrdersDAO();
            s_ordersDAO.Clear();

            s_cmd = new AddOrderCommand(CUSTOMER_NAME, ORDER_DESCRIPTION, s_NOW);

            s_handler = new AddOrderCommandHandler(s_ordersDAO, logger);
        };

        private Because _of = () =>
        {
            s_handler.Handle(s_cmd);
            s_orderToBeAdded = s_ordersDAO.FindById(s_cmd.OrderId);
        };

        private It _should_have_the_matching_customer_name = () => s_orderToBeAdded.CustomerName.ShouldEqual(CUSTOMER_NAME);
        private It _should_have_the_matching_customer_description = () => s_orderToBeAdded.OrderDescription.ShouldEqual(ORDER_DESCRIPTION);
        private It _sould_have_the_matching_customer_name = () => s_orderToBeAdded.DueDate.Value.ToShortDateString().ShouldEqual(s_NOW.ToShortDateString());
    }

    [Subject(typeof(EditOrderCommandHandler))]
    public class When_editing_an_existing_order
    {
        private static EditOrderCommandHandler s_handler;
        private static EditOrderCommand s_cmd;
        private static IOrdersDAO s_ordersDAO;
        private static Order s_orderToBeEdited;
        private const string NEW_CUSTOMER_NAME = "New Customer";
        private const string NEW_ORDER_DESCRIPTION = "New Test that we store an order";
        private static readonly DateTime s_NEW_TIME = DateTime.Now.AddDays(1);

        private Establish _context = () =>
        {
            var logger = A.Fake<ILog>();
            s_orderToBeEdited = new Order("Existing Customer", "My Order Description", DateTime.Now);
            s_ordersDAO = new OrdersDAO();
            s_ordersDAO.Clear();
            s_orderToBeEdited = s_ordersDAO.Add(s_orderToBeEdited);

            s_cmd = new EditOrderCommand(s_orderToBeEdited.Id, NEW_CUSTOMER_NAME , NEW_ORDER_DESCRIPTION , s_NEW_TIME);

            s_handler = new EditOrderCommandHandler(s_ordersDAO, logger);
        };

        private Because _of = () =>
        {
            s_handler.Handle(s_cmd);
            s_orderToBeEdited = s_ordersDAO.FindById(s_cmd.OrderId);
        };

        private It _should_update_the_order_with_the_new_order_name = () => s_orderToBeEdited.CustomerName.ShouldEqual(NEW_CUSTOMER_NAME);
        private It _should_update_the_task_with_the_new_order_description = () => s_orderToBeEdited.OrderDescription.ShouldEqual(NEW_ORDER_DESCRIPTION);
        private It _should_update_the_task_with_the_new_order_time = () => s_orderToBeEdited.DueDate.Value.ToShortDateString().ShouldEqual(s_NEW_TIME.ToShortDateString());
    }

    [Subject(typeof(CompleteOrderCommandHandler))]
    public class When_completing_an_existing_order
    {
        private static CompleteOrderCommandHandler s_handler;
        private static CompleteOrderCommand s_cmd;
        private static IOrdersDAO s_ordersDAO;
        private static Order s_orderToBeCompleted;
        private static readonly DateTime s_COMPLETION_DATE = DateTime.Now.AddDays(-1);

        private Establish _context = () =>
        {
            var logger = A.Fake<ILog>();
            s_orderToBeCompleted = new Order("IamA Customer", "My Order Description", DateTime.Now);
            s_ordersDAO = new OrdersDAO();
            s_ordersDAO.Clear();
            s_orderToBeCompleted = s_ordersDAO.Add(s_orderToBeCompleted);

            s_cmd = new CompleteOrderCommand(s_orderToBeCompleted.Id, s_COMPLETION_DATE);

            s_handler = new CompleteOrderCommandHandler(s_ordersDAO, logger);
        };

        private Because _of = () =>
        {
            s_handler.Handle(s_cmd);
            s_orderToBeCompleted = s_ordersDAO.FindById(s_cmd.OrderId);
        };

        private It _should_update_the_orders_completed_date = () => s_orderToBeCompleted.CompletionDate.Value.ToShortDateString().ShouldEqual(s_COMPLETION_DATE.ToShortDateString());
    }

    [Subject(typeof(CompleteOrderCommandHandler))]
    public class When_completing_a_missing_order
    {
        private static CompleteOrderCommandHandler s_handler;
        private static CompleteOrderCommand s_cmd;
        private static IOrdersDAO s_ordersDAO;
        private const int ORDER_ID = 1;
        private static readonly DateTime s_COMPLETION_DATE = DateTime.Now.AddDays(-1);
        private static Exception s_exception;

        private Establish _context = () =>
        {
            var logger = A.Fake<ILog>();
            s_ordersDAO = new OrdersDAO();
            s_ordersDAO.Clear();
            s_cmd = new CompleteOrderCommand(ORDER_ID, s_COMPLETION_DATE);

            s_handler = new CompleteOrderCommandHandler(s_ordersDAO, logger);
        };

        private Because _of = () => s_exception = Catch.Exception(() => s_handler.Handle(s_cmd));

        private It _should_fail = () => s_exception.ShouldBeAssignableTo<ArgumentOutOfRangeException>();
    }
}