using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Pleiades.Framework.Data.EF;
using Pleiades.Framework.Utilities;


namespace Pleiades.Framework.IntegrationTests.DataEF
{
    [TestFixture]
    public class TestFixture
    {
        [SetUp]        
        public void BuildDatabase()
        {
            var context = new MyContext();
            if (context.Database.Exists())
            {
                context.Database.Delete();
            }

            context.Database.Create();
        }

        [Test]
        public void Test_Add_And_FindFirstOrDefault_GetAll_And_Delete_And_Count()
        {
            // Arrange
            var context = new MyContext();
            var repository = new MyEntityRepository(context);

            // Act
            var entity1 = new MyEntity { Name = "George", Description = "It's like this and ah!", Amount = 900 };
            var entity2 = new MyEntity { Name = "David", Description = "Testing awaaaay", Amount = 6600 };
            
            repository.Add(entity1);
            repository.Add(entity2);
            repository.SaveChanges();

            var entity1Fromdb = repository.FindFirstOrDefault(x => x.Id == entity1.Id);
            var entity2Fromdb = repository.FindFirstOrDefault(x => x.Id == entity2.Id);

            // Assert
            Assert.IsTrue(entity1.StrungOutCompare(entity1Fromdb));
            Assert.IsTrue(entity2.StrungOutCompare(entity2Fromdb));

            Assert.IsFalse(entity1.StrungOutCompare(entity2Fromdb));
            Assert.IsFalse(entity2.StrungOutCompare(entity1Fromdb));

            // Clean-up
            EmptyRepository();
        }

        [Test]
        public void Test_FindBy_And_Count_Predicate()
        {
            // First, initialize
            var context = new MyContext();
            var repository = new MyEntityRepository(context);            
            
            // Arrange
            var entity1 = new MyEntity { Name = "Aleks", Description = "Pimping awaaaay", Amount = 222 };
            var entity2 = new MyEntity { Name = "Ayende", Description = "Representing the true commerce gangstas", Amount = 333 };
            var entity3 = new MyEntity { Name = "Martha", Description = "Home-Ec gangsta", Amount = 333 };

            repository.Add(entity1);
            repository.Add(entity2);
            repository.Add(entity3);
            repository.SaveChanges();

            // Act & Assert
            var entity1FromDb = repository.FindBy(x => x.Name == "Aleks").FirstOrDefault();
            Assert.IsTrue(entity1.StrungOutCompare(entity1FromDb));

            var howManyGangstas = repository.Count(x => x.Description.Contains("gangsta"));
            Assert.AreEqual(2, howManyGangstas);
        }

        [Test]
        public void Test_Transaction_Edit()
        {
            // First, initialize
            this.EmptyRepository();
            var context = new MyContext();
            var repository = new MyEntityRepository(context);

            // Arrange
            var entity1 = new MyEntity { Name = "Olga", Description = "Hi everybody, I'm like librarian", Amount = 777 };
            repository.Add(entity1);
            repository.SaveChanges();

            // Assert
            var entity1Db = repository.FindFirstOrDefault(x => x.Name == "Olga");
            Assert.IsTrue(entity1Db.StrungOutCompare(entity1));

            // 1st Transaction - Fails!
            try
            {
                var unitOfWork = new EFUnitOfWork(context);
                unitOfWork.Execute(() => 
                    {
                        entity1Db.Amount = 800;
                        repository.Edit(entity1Db);
                        throw new Exception("If I can't have data, nobody can!!!");
                    });
            }
            catch
            {
                // Should kill the Transaction
            }

            var entity1Db2 = repository.FindFirstOrDefault(x => x.Name == "Olga");
            Assert.AreEqual(777, entity1Db2.Amount);

            // 2nd Transaction - Succeeds!
            try
            {
                var unitOfWork = new EFUnitOfWork(context);
                unitOfWork.Execute(() =>
                {
                    entity1Db2.Amount = 800;
                    repository.Edit(entity1Db2);
                });
            }
            catch
            {
                // Should kill the Transaction
            }

            var entity1Db3 = repository.FindFirstOrDefault(x => x.Name == "Olga");
            Assert.AreEqual(800, entity1Db2.Amount);
        }

        public void EmptyRepository()
        {
            var context = new MyContext();
            var repository = new MyEntityRepository(context); 
            
            foreach (var entity in repository.GetAll())
            {
                repository.Delete(entity);
            }
            repository.SaveChanges();

            // More Assert
            Assert.AreEqual(0, repository.Count());
        }
    }
}
