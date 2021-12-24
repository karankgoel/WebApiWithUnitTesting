using eBroker.DAL;
using eBroker.DAL.Models;
using eBroker.DAL.Repositories;
using eBroker.Shared.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace eBroker.Tests.Repositories
{
    public class RepositoryTest
    {
        static DbContextOptions options { get; }

        static RepositoryTest()
        {
            options = new DbContextOptionsBuilder<EBrokerContext>().UseInMemoryDatabase(databaseName: "EBrokerDatabase").Options;

            using (var context = new EBrokerContext(options))
            {
                context.Equities.Add(new EquityDB()
                {
                    EquityId = 1,
                    Name = "Binbosys",
                    Price = 100
                });
                context.Equities.Add(new EquityDB()
                {
                    EquityId = 2,
                    Name = "BCS",
                    Price = 200
                });
                context.Equities.Add(new EquityDB()
                {
                    EquityId = 3,
                    Name = "Defiance",
                    Price = 450
                });


                context.EquityHoldings.Add(new EquityHoldingDB()
                {
                    EquityHoldingId = 1,
                    UserId = 1,
                    EquityId = 1,
                    Quantity = 10
                });
                context.EquityHoldings.Add(new EquityHoldingDB()
                {
                    EquityHoldingId = 2,
                    UserId = 1,
                    EquityId = 2,
                    Quantity = 40
                });
                context.EquityHoldings.Add(new EquityHoldingDB()
                {
                    EquityHoldingId = 3,
                    UserId = 1,
                    EquityId = 3,
                    Quantity = 100
                });
                context.EquityHoldings.Add(new EquityHoldingDB()
                {
                    EquityHoldingId = 4,
                    UserId = 2,
                    EquityId = 1,
                    Quantity = 10
                });
                context.EquityHoldings.Add(new EquityHoldingDB()
                {
                    EquityHoldingId = 5,
                    UserId = 2,
                    EquityId = 3,
                    Quantity = 10
                });

                context.Wallet.Add(new WalletDB()
                {
                    WalletId = 1,
                    UserId = 1,
                    Amount = 100
                });
                context.Wallet.Add(new WalletDB()
                {
                    WalletId = 2,
                    UserId = 2,
                    Amount = 600
                });

                context.TransactionOrders.Add(new TransactionOrderDB()
                {
                    OrderId = 1,
                    UserId = 1,
                    EquityId = 1,
                    Quantity = 10,
                    UnitPrice = 100,
                    Type = OrderType.Buy
                });
                context.TransactionOrders.Add(new TransactionOrderDB()
                {
                    OrderId = 2,
                    UserId = 1,
                    EquityId = 1,
                    Quantity = 5,
                    UnitPrice = 120,
                    Type = OrderType.Sell
                });
                context.TransactionOrders.Add(new TransactionOrderDB()
                {
                    OrderId = 3,
                    UserId = 2,
                    EquityId = 1,
                    Quantity = 10,
                    UnitPrice = 100,
                    Type = OrderType.Buy
                });
                context.SaveChanges();
            }
        }


        #region EquityRepository
        [Fact]
        public void EquityRepository_GetById_Successful()
        {
            using(var context = new EBrokerContext(options))
            {
                EquityRepository repository = new EquityRepository(context);

                var response = repository.GetById(1);

                Assert.True(response.EquityId == 1);
                Assert.True(response.Name == "Binbosys");
                Assert.True(response.Price == 100);
            }
        }

        [Fact]
        public void EquityRepository_GetById_ElementDoesNotExist()
        {
            using (var context = new EBrokerContext(options))
            {
                EquityRepository repository = new EquityRepository(context);

                var response = repository.GetById(5);

                Assert.Null(response);
            }
        }

        [Fact]
        public void EquityRepository_Insert_Successful()
        {
            var inputObject = new EquityDB()
            {
                EquityId = 4,
                Name = "LP",
                Price = 250
            };

            using (var context = new EBrokerContext(options))
            {
                EquityRepository repository = new EquityRepository(context);
                var oldEquity = context.Equities.FirstOrDefault(x => x.EquityId == 4);
                Assert.Null(oldEquity);

                repository.Insert(inputObject);
                context.SaveChanges();

                var newEquity = context.Equities.FirstOrDefault(x => x.EquityId == 4);
                Assert.NotNull(newEquity);
            }
        }

        [Fact]
        public void EquityRepository_Insert_ItemWithSameKeyExists()
        {
            var inputObject = new EquityDB()
            {
                EquityId = 3,
                Name = "LP",
                Price = 250
            };

            using (var context = new EBrokerContext(options))
            {
                EquityRepository repository = new EquityRepository(context); 
                var ex = Assert.Throws<ArgumentException>(() => {
                    repository.Insert(inputObject);
                    context.SaveChanges();
                });
                Assert.Equal("An item with the same key has already been added. Key: 3", ex.Message);
            }
        }

        [Fact]
        public void EquityRepository_Update_Successful()
        {
            var inputObject = new EquityDB()
            {
                EquityId = 2,
                Name = "BCS",
                Price = 250
            };

            using (var context = new EBrokerContext(options))
            {
                EquityRepository repository = new EquityRepository(context);

                repository.Update(inputObject);
                context.SaveChanges();

                var newEquity = context.Equities.FirstOrDefault(x => x.EquityId == 2);
                Assert.True(newEquity.Price == 250);
                Assert.True(newEquity.EquityId == 2);
            }
        }

        [Fact]
        public void EquityRepository_Get_WithoutFilter()
        {
            using (var context = new EBrokerContext(options))
            {
                EquityRepository repository = new EquityRepository(context);

                var response = repository.Get();

                Assert.NotNull(response);
            }
        }

        [Fact]
        public void EquityRepository_Get_WithFilter()
        {
            using (var context = new EBrokerContext(options))
            {
                EquityRepository repository = new EquityRepository(context);

                var response = repository.Get(x=> x.Price == 200);

                Assert.NotNull(response);
                Assert.True(response.Count() == 1);
                Assert.True(response.FirstOrDefault().Name == "BCS");
                Assert.True(response.FirstOrDefault().EquityId == 2);
            }
        }

        [Fact]
        public void EquityRepository_Get_WithFilter_RecordDoesNotExist()
        {
            using (var context = new EBrokerContext(options))
            {
                EquityRepository repository = new EquityRepository(context);

                var response = repository.Get(x => x.Name == "SBI");

                Assert.NotNull(response);
                Assert.Empty(response);
            }
        }
        #endregion

        #region EquityHoldingRepository
        [Fact]
        public void EquityHoldingRepository_GetById_Successful()
        {
            using (var context = new EBrokerContext(options))
            {
                EquityHoldingRepository repository = new EquityHoldingRepository(context);

                var response = repository.GetById(1);

                Assert.True(response.EquityHoldingId == 1);
                Assert.True(response.EquityId == 1);
            }
        }

        [Fact]
        public void EquityHoldingRepository_GetById_ElementDoesNotExist()
        {
            using (var context = new EBrokerContext(options))
            {
                EquityHoldingRepository repository = new EquityHoldingRepository(context);

                var response = repository.GetById(6);

                Assert.Null(response);
            }
        }

        [Fact]
        public void EquityHoldingRepository_Insert_Successful()
        {
            var inputObject = new EquityHoldingDB()
            {
                EquityHoldingId = 6,
                EquityId = 4,
                Quantity = 100,
                UserId = 1
            };

            using (var context = new EBrokerContext(options))
            {
                EquityHoldingRepository repository = new EquityHoldingRepository(context);
                var oldEquityHolding = context.EquityHoldings.FirstOrDefault(x => x.EquityHoldingId == 6);
                Assert.Null(oldEquityHolding);

                repository.Insert(inputObject);
                context.SaveChanges();

                var newEquityHolding = context.EquityHoldings.FirstOrDefault(x => x.EquityHoldingId == 5);
                Assert.NotNull(newEquityHolding);
            }
        }

        [Fact]
        public void EquityHoldingRepository_Insert_ItemWithSameKeyExists()
        {
            var inputObject = new EquityHoldingDB()
            {
                EquityHoldingId = 5,
                EquityId = 4,
                Quantity = 100,
                UserId = 1
            };

            using (var context = new EBrokerContext(options))
            {
                EquityHoldingRepository repository = new EquityHoldingRepository(context);

                var ex = Assert.Throws<ArgumentException>(() => { 
                    repository.Insert(inputObject); 
                    context.SaveChanges(); 
                });
                Assert.Equal("An item with the same key has already been added. Key: 5", ex.Message);
            }
        }

        [Fact]
        public void EquityHoldingRepository_Update_Successful()
        {
            var inputObject = new EquityHoldingDB()
            {
                EquityHoldingId = 1,
                EquityId = 1,
                UserId = 1,
                Quantity = 50
            };

            using (var context = new EBrokerContext(options))
            {
                EquityHoldingRepository repository = new EquityHoldingRepository(context);

                repository.Update(inputObject);
                context.SaveChanges();

                var newEquityHolding = context.EquityHoldings.FirstOrDefault(x => x.EquityHoldingId == 1);
                Assert.True(newEquityHolding.Quantity == 50);
                Assert.True(newEquityHolding.EquityId == 1);
            }
        }

        [Fact]
        public void EquityHoldingRepository_Get_WithoutFilter()
        {
            using (var context = new EBrokerContext(options))
            {
                EquityHoldingRepository repository = new EquityHoldingRepository(context);

                var response = repository.Get();

                Assert.NotNull(response);
            }
        }

        [Fact]
        public void EquityHoldingRepository_Get_WithFilter()
        {
            using (var context = new EBrokerContext(options))
            {
                EquityHoldingRepository repository = new EquityHoldingRepository(context);

                var response = repository.Get(x => x.Quantity == 10);

                Assert.NotNull(response);
                Assert.True(response.Count() == 3);
                //Assert.True(response.FirstOrDefault().Name == "BCS");
                //Assert.True(response.FirstOrDefault().EquityId == 2);
            }
        }

        [Fact]
        public void EquityHoldingRepository_Get_WithFilter_RecordDoesNotExist()
        {
            using (var context = new EBrokerContext(options))
            {
                EquityHoldingRepository repository = new EquityHoldingRepository(context);

                var response = repository.Get(x => x.Quantity == 75);

                Assert.NotNull(response);
                Assert.Empty(response);
            }
        }
        #endregion

        #region WalletRepository
        [Fact]
        public void WalletRepository_GetById_Successful()
        {
            using (var context = new EBrokerContext(options))
            {
                WalletRepository repository = new WalletRepository(context);

                var response = repository.GetById(1);

                Assert.True(response.WalletId == 1);
                Assert.True(response.UserId == 1);
                Assert.True(response.Amount == 100);
            }
        }

        [Fact]
        public void WalletRepository_GetById_ElementDoesNotExist()
        {
            using (var context = new EBrokerContext(options))
            {
                WalletRepository repository = new WalletRepository(context);

                var response = repository.GetById(5);

                Assert.Null(response);
            }
        }

        [Fact]
        public void WalletRepository_Insert_Successful()
        {
            var inputObject = new WalletDB()
            {
                WalletId = 4,
                UserId = 3,
                Amount = 250
            };

            using (var context = new EBrokerContext(options))
            {
                WalletRepository repository = new WalletRepository(context);
                var oldWallet = context.Wallet.FirstOrDefault(x => x.WalletId == 4);
                Assert.Null(oldWallet);

                repository.Insert(inputObject);
                context.SaveChanges();

                var newWallet = context.Wallet.FirstOrDefault(x => x.WalletId == 4);
                Assert.NotNull(newWallet);
            }
        }

        [Fact]
        public void WalletRepository_Insert_ItemWithSameKeyExists()
        {
            var inputObject = new WalletDB()
            {
                WalletId = 2,
                UserId = 3,
                Amount = 250
            };

            using (var context = new EBrokerContext(options))
            {
                WalletRepository repository = new WalletRepository(context);
                var ex = Assert.Throws<ArgumentException>(() => {
                    repository.Insert(inputObject);
                    context.SaveChanges();
                });
                Assert.Equal("An item with the same key has already been added. Key: 2", ex.Message);
            }
        }

        [Fact]
        public void WalletRepository_Update_Successful()
        {
            var inputObject = new WalletDB()
            {
                WalletId = 2,
                UserId = 2,
                Amount = 5000
            };

            using (var context = new EBrokerContext(options))
            {
                WalletRepository repository = new WalletRepository(context);

                repository.Update(inputObject);
                context.SaveChanges();

                var newWallet = context.Wallet.FirstOrDefault(x => x.WalletId == 2);
                Assert.True(newWallet.Amount == 5000);
            }
        }

        [Fact]
        public void WalletRepository_Get_WithoutFilter()
        {
            using (var context = new EBrokerContext(options))
            {
                WalletRepository repository = new WalletRepository(context);

                var response = repository.Get();

                Assert.NotNull(response);
            }
        }

        [Fact]
        public void WalletRepository_Get_WithFilter()
        {
            using (var context = new EBrokerContext(options))
            {
                WalletRepository repository = new WalletRepository(context);

                var response = repository.Get(x => x.UserId == 1);

                Assert.NotNull(response);
                Assert.True(response.Count() == 1);
                Assert.True(response.FirstOrDefault().UserId == 1);
                Assert.True(response.FirstOrDefault().WalletId == 1);
            }
        }


        [Fact]
        public void WalletRepository_Get_WithIncludeProperties()
        {
            using (var context = new EBrokerContext(options))
            {
                WalletRepository repository = new WalletRepository(context);

                var response = repository.Get(x => x.UserId == 1, "EquityHoldings");

                Assert.NotNull(response);
                Assert.True(response.Count() == 1);
                Assert.True(response.FirstOrDefault().UserId == 1);
                Assert.True(response.FirstOrDefault().WalletId == 1);
                Assert.NotNull(response.FirstOrDefault().EquityHoldings);
            }
        }

        [Fact]
        public void WalletRepository_Get_WithFilter_RecordDoesNotExist()
        {
            using (var context = new EBrokerContext(options))
            {
                WalletRepository repository = new WalletRepository(context);

                var response = repository.Get(x => x.UserId == 10);

                Assert.NotNull(response);
                Assert.Empty(response);
            }
        }
        #endregion

        #region TransactionOrderRepository
        [Fact]
        public void TransactionOrderRepository_GetById_Successful()
        {
            using (var context = new EBrokerContext(options))
            {
                TransactionOrderRepository repository = new TransactionOrderRepository(context);

                var response = repository.GetById(1);

                Assert.True(response.OrderId == 1);
                Assert.True(response.UserId == 1);
                Assert.True(response.Quantity == 10);
            }
        }

        [Fact]
        public void TransactionOrderRepository_GetById_ElementDoesNotExist()
        {
            using (var context = new EBrokerContext(options))
            {
                TransactionOrderRepository repository = new TransactionOrderRepository(context);

                var response = repository.GetById(5);

                Assert.Null(response);
            }
        }

        [Fact]
        public void TransactionOrderRepository_Insert_Successful()
        {
            var inputObject = new TransactionOrderDB()
            {
                OrderId = 4,
                UserId = 3,
                Quantity = 250,
                UnitPrice = 100
            };

            using (var context = new EBrokerContext(options))
            {
                TransactionOrderRepository repository = new TransactionOrderRepository(context);
                var oldOrder = context.TransactionOrders.FirstOrDefault(x => x.OrderId == 4);
                Assert.Null(oldOrder);

                repository.Insert(inputObject);
                context.SaveChanges();

                var newOrder = context.TransactionOrders.FirstOrDefault(x => x.OrderId == 4);
                Assert.NotNull(newOrder);
            }
        }

        [Fact]
        public void TransactionOrderRepository_Insert_ItemWithSameKeyExists()
        {
            var inputObject = new TransactionOrderDB()
            {
                OrderId = 2,
                UserId = 3,
                Quantity = 250,
                UnitPrice = 100
            };

            using (var context = new EBrokerContext(options))
            {
                TransactionOrderRepository repository = new TransactionOrderRepository(context);
                var ex = Assert.Throws<ArgumentException>(() => {
                    repository.Insert(inputObject);
                    context.SaveChanges();
                });
                Assert.Equal("An item with the same key has already been added. Key: 2", ex.Message);
            }
        }

        [Fact]
        public void TransactionOrderRepository_Update_Successful()
        {
            var inputObject = new TransactionOrderDB()
            {
                OrderId = 2,
                UserId = 3,
                Quantity = 250,
                UnitPrice = 100,
                EquityId = 2,
                Type = OrderType.Buy
            };

            using (var context = new EBrokerContext(options))
            {
                TransactionOrderRepository repository = new TransactionOrderRepository(context);

                repository.Update(inputObject);
                context.SaveChanges();

                var newOrder = context.TransactionOrders.FirstOrDefault(x => x.OrderId == 2);
                Assert.True(newOrder.Quantity == 250);
            }
        }

        [Fact]
        public void TransactionOrderRepository_Get_WithoutFilter()
        {
            using (var context = new EBrokerContext(options))
            {
                TransactionOrderRepository repository = new TransactionOrderRepository(context);

                var response = repository.Get();

                Assert.NotNull(response);
            }
        }

        [Fact]
        public void TransactionOrderRepository_Get_WithFilter()
        {
            using (var context = new EBrokerContext(options))
            {
                TransactionOrderRepository repository = new TransactionOrderRepository(context);

                var response = repository.Get(x => x.UserId == 1);

                Assert.NotNull(response);
                Assert.True(response.Count() == 2);
                Assert.True(response.FirstOrDefault().UserId == 1);
            }
        }

        [Fact]
        public void TransactionOrderRepository_Get_WithFilter_RecordDoesNotExist()
        {
            using (var context = new EBrokerContext(options))
            {
                TransactionOrderRepository repository = new TransactionOrderRepository(context);

                var response = repository.Get(x => x.UserId == 10);

                Assert.NotNull(response);
                Assert.Empty(response);
            }
        }
        #endregion
    }
}
