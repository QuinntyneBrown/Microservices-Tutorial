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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FakeItEasy;
using Grean.AtomEventStore;
using Machine.Specifications;
using paramore.brighter.commandprocessor;
using paramore.brighter.commandprocessor.Logging;
using Products_Core.Adapters.Atom;
using Products_Core.Adapters.DataAccess;
using Products_Core.Model;
using Products_Core.Ports.Commands;
using Products_Core.Ports.Events;
using Products_Core.Ports.Handlers;

namespace Orders_Core.Adapters.Tests
{
    [Subject(typeof(AddProductCommandHandler))]
    public class When_adding_a_new_product_to_the_list
    {
        private static AddProductCommandHandler s_handler;
        private static AddProductCommand s_cmd;
        private static IProductsDAO s_productsDAO;
        private static IAmACommandProcessor s_commandProcessor; 
        private static Product s_productToBeAdded;
        private const string PRODUCT_NAME = "Red Velvet Cake";
        private const string PRODUCT_DESCRIPTION = "Red chocolatey goodness";
        private static readonly double PRODUCT_PRICE = 4.50; 

        private Establish _context = () =>
        {
            var logger = A.Fake<ILog>();
            s_commandProcessor = A.Fake<IAmACommandProcessor>();
            s_productsDAO = new ProductsDAO();
            s_productsDAO.Clear();

            s_cmd = new AddProductCommand(PRODUCT_NAME, PRODUCT_DESCRIPTION, PRODUCT_PRICE);

            s_handler = new AddProductCommandHandler(s_productsDAO, s_commandProcessor, logger);
        };

        private Because _of = () =>
        {
            s_handler.Handle(s_cmd);
            s_productToBeAdded = s_productsDAO.FindById(s_cmd.ProductId);
        };

        private It _should_have_the_matching_product_name = () => s_productToBeAdded.ProductName.ShouldEqual(PRODUCT_NAME);
        private It _should_have_the_matching_product_description = () => s_productToBeAdded.ProductDescription.ShouldEqual(PRODUCT_DESCRIPTION);
        private It _should_have_the_matching_product_price = () => s_productToBeAdded.ProductPrice.ShouldEqual(PRODUCT_PRICE);
        private It _should_have_raise_a_product_added_event = () => A.CallTo(() => s_commandProcessor.Publish(A<ProductAddedEvent>.Ignored)).MustHaveHappened();
    }


    [Subject(typeof(ChangeProductCommandHandler))]
    public class When_editing_an_existing_product
    {
        private static ChangeProductCommandHandler s_handler;
        private static ChangeProductCommand s_cmd;
        private static IProductsDAO s_productssDAO;
        private static IAmACommandProcessor s_commandProcessor; 
        private static Product s_productToBeEdited;
        private const string NEW_PRODUCT_NAME  = "Chocolate Cake";
        private const string NEW_PRODUCT_DESCRIPTION = "The best chocolate ever";
        private const double NEW_PRICE = 5.50;

        private Establish _context = () =>
        {
            var logger = A.Fake<ILog>();
            s_commandProcessor = A.Fake<IAmACommandProcessor>();
            s_productToBeEdited = new Product("Coffee Cake", "Coffee Cake with a real jolt", 3.50);
            s_productssDAO = new ProductsDAO();
            s_productssDAO.Clear();
            s_productToBeEdited = s_productssDAO.Add(s_productToBeEdited);

            s_cmd = new ChangeProductCommand(s_productToBeEdited.Id, NEW_PRODUCT_NAME , NEW_PRODUCT_DESCRIPTION, NEW_PRICE);

            s_handler = new ChangeProductCommandHandler(s_productssDAO, s_commandProcessor, logger);
        };

        private Because _of = () =>
        {
            s_handler.Handle(s_cmd);
            s_productToBeEdited = s_productssDAO.FindById(s_cmd.ProductId);
        };

        private It _should_update_the_order_with_the_new_product_name = () => s_productToBeEdited.ProductName.ShouldEqual(NEW_PRODUCT_NAME);
        private It _should_update_the_task_with_the_new_product_description = () => s_productToBeEdited.ProductDescription.ShouldEqual(NEW_PRODUCT_DESCRIPTION);
        private It _should_update_the_task_with_the_new_product_time = () => s_productToBeEdited.ProductPrice.ShouldEqual(NEW_PRICE);
        private It _should_raise_a_product_updated_event = () => A.CallTo(() => s_commandProcessor.Publish(A<ProductChangedEvent>.Ignored)).MustHaveHappened();
    }

    [Subject(typeof(RemoveProductCommandHandler))]
    public class When_removing_an_existing_product
    {
        private static RemoveProductCommandHandler s_handler;
        private static RemoveProductCommand s_cmd;
        private static IProductsDAO s_productssDAO;
        private static IAmACommandProcessor s_commandProcessor; 
        private static Product s_productToBeRemoved;

        private Establish _context = () =>
        {
            var logger = A.Fake<ILog>();
            s_commandProcessor = A.Fake<IAmACommandProcessor>();
            s_productToBeRemoved = new Product("Coffee Cake", "Coffee Cake with a real jolt", 3.50);
            s_productssDAO = new ProductsDAO();
            s_productssDAO.Clear();
            s_productToBeRemoved = s_productssDAO.Add(s_productToBeRemoved);

            s_cmd = new RemoveProductCommand(s_productToBeRemoved.Id);

            s_handler = new RemoveProductCommandHandler(s_productssDAO, s_commandProcessor, logger);
        };

        private Because _of = () =>
        {
            s_handler.Handle(s_cmd);
            s_productToBeRemoved = s_productssDAO.FindById(s_cmd.ProductId);
        };

        private It _should_update_the_order_with_the_new_product_name = () => s_productToBeRemoved.ShouldBeNull();
        private It _should_raise_a_product_updated_event = () => A.CallTo(() => s_commandProcessor.Publish(A<ProductRemovedEvent>.Ignored)).MustHaveHappened();
    }
}