using FooModule;
using FooModule.Helpers;
using FooModule.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestLogicImplementer
{
    public class FooLogicImplementerTests
    {
        private readonly Mock<IFooCollection> _fooCollection;

        private Mock<IFooWork> _work1;
        private Mock<IFooWork> _work2;

        public FooLogicImplementerTests()
        {
            _fooCollection = new Mock<IFooCollection>();
        }

        [Fact]
        public async void If_All_Good_Return_Good()
        {
            // mock dependencies
            // setup work 1 to be good result
            _work1 = new Mock<IFooWork>();
            _work1.Setup(s => s.CheckFooAsync("logic_reg_1_1", It.IsAny<string>(), It.IsAny<FooContext>(), It.IsAny<FooToken>()))
                .Returns(Task.FromResult(new FooResult("logic_reg_1_1", It.IsAny<string>(), FooResultVals.GoodResult)));

            // setup work 2 to be good result
            _work2 = new Mock<IFooWork>();
            _work2.Setup(s => s.CheckFooAsync("logic_reg_1_2", It.IsAny<string>(), It.IsAny<FooContext>(), It.IsAny<FooToken>()))
                .Returns(Task.FromResult(new FooResult("logic_reg_1_2", It.IsAny<string>(), FooResultVals.GoodResult)));

            // Add some FooRegistrations
            var data = new List<FooRegistration>()
            {
                new FooRegistration("logic_reg_1_1", _work1.Object, new FooToken(), new List<string> {"tag1", "tag2"}),
                new FooRegistration("logic_reg_1_2", _work2.Object, new FooToken(), new List<string> {"tag1", "tag2"})
            };
            _fooCollection.Setup(s => s.GetAllRegistrations()).Returns(data);

            // act
            var testImplementer = new FooLogicImplementer(new List<IFooCollection> { _fooCollection.Object });

            // assert
            var res = await testImplementer.ExecuteLogic();

            Xunit.Assert.Equal("Good", res.FinalRes);
        }

        [Fact]
        public async void If_Some_Good_Return_Bad()
        {
            // mock dependencies
            // setup work 1 to be good result
            _work1 = new Mock<IFooWork>();
            _work1.Setup(s => s.CheckFooAsync("logic_reg_1_1", It.IsAny<string>(), It.IsAny<FooContext>(), It.IsAny<FooToken>()))
                .Returns(Task.FromResult(new FooResult("logic_reg_1_1", It.IsAny<string>(), FooResultVals.GoodResult)));

            // setup work 2 to be bad result
            _work2 = new Mock<IFooWork>();
            _work2.Setup(s => s.CheckFooAsync("logic_reg_1_2", It.IsAny<string>(), It.IsAny<FooContext>(), It.IsAny<FooToken>()))
                .Returns(Task.FromResult(new FooResult("logic_reg_1_2", It.IsAny<string>(), FooResultVals.BadResult)));

            // Add some FooRegistrations
            var data = new List<FooRegistration>()
            {
                new FooRegistration("logic_reg_1_1", _work1.Object, new FooToken(), new List<string> {"tag1", "tag2"}),
                new FooRegistration("logic_reg_1_2", _work2.Object, new FooToken(), new List<string> {"tag1", "tag2"})
            };
            _fooCollection.Setup(s => s.GetAllRegistrations()).Returns(data);

            // act
            var testImplementer = new FooLogicImplementer(new List<IFooCollection> { _fooCollection.Object });

            // assert
            var res = await testImplementer.ExecuteLogic();

            Xunit.Assert.Equal("Bad", res.FinalRes);
        }

        [Fact]
        public async void If_All_Bad_Return_Bad()
        {
            // mock dependencies
            // setup work 1 to be good result
            _work1 = new Mock<IFooWork>();
            _work1.Setup(s => s.CheckFooAsync("logic_reg_1_1", It.IsAny<string>(), It.IsAny<FooContext>(), It.IsAny<FooToken>()))
                .Returns(Task.FromResult(new FooResult("logic_reg_1_1", It.IsAny<string>(), FooResultVals.BadResult)));

            // setup work 2 to be bad result
            _work2 = new Mock<IFooWork>();
            _work2.Setup(s => s.CheckFooAsync("logic_reg_1_2", It.IsAny<string>(), It.IsAny<FooContext>(), It.IsAny<FooToken>()))
                .Returns(Task.FromResult(new FooResult("logic_reg_1_2", It.IsAny<string>(), FooResultVals.BadResult)));

            // Add some FooRegistrations
            var data = new List<FooRegistration>()
            {
                new FooRegistration("logic_reg_1_1", _work1.Object, new FooToken(), new List<string> {"tag1", "tag2"}),
                new FooRegistration("logic_reg_1_2", _work2.Object, new FooToken(), new List<string> {"tag1", "tag2"})
            };
            _fooCollection.Setup(s => s.GetAllRegistrations()).Returns(data);

            // act
            var testImplementer = new FooLogicImplementer(new List<IFooCollection> { _fooCollection.Object });

            // assert
            var res = await testImplementer.ExecuteLogic();

            Xunit.Assert.Equal("Bad", res.FinalRes);
        }

        [Fact]
        public async void If_1_Registered_Executes_1()
        {
            // mock dependencies
            // setup work 1 to be good result
            _work1 = new Mock<IFooWork>();
            _work1.Setup(s => s.CheckFooAsync("logic_reg_1_1", It.IsAny<string>(), It.IsAny<FooContext>(), It.IsAny<FooToken>()))
                .Returns(Task.FromResult(new FooResult("logic_reg_1_1", It.IsAny<string>(), FooResultVals.GoodResult)));

            // Add some FooRegistrations
            var data = new List<FooRegistration>()
            {
                new FooRegistration("logic_reg_1_1", _work1.Object, new FooToken(), new List<string> {"tag1", "tag2"})
            };
            _fooCollection.Setup(s => s.GetAllRegistrations()).Returns(data);

            // act
            var testImplementer = new FooLogicImplementer(new List<IFooCollection> { _fooCollection.Object });

            // assert
            var res = await testImplementer.ExecuteLogic();

            Xunit.Assert.Single(res.Data);
        }

        [Fact]
        public async void If_2_Registered_Executes_2()
        {
            // mock dependencies
            // setup work 1 to be good result
            _work1 = new Mock<IFooWork>();
            _work1.Setup(s => s.CheckFooAsync("logic_reg_1_1", It.IsAny<string>(), It.IsAny<FooContext>(), It.IsAny<FooToken>()))
                .Returns(Task.FromResult(new FooResult("logic_reg_1_1", It.IsAny<string>(), FooResultVals.GoodResult)));

            // setup work 2 to be good result
            _work2 = new Mock<IFooWork>();
            _work2.Setup(s => s.CheckFooAsync("logic_reg_1_2", It.IsAny<string>(), It.IsAny<FooContext>(), It.IsAny<FooToken>()))
                .Returns(Task.FromResult(new FooResult("logic_reg_1_2", It.IsAny<string>(), FooResultVals.BadResult)));

            // Add some FooRegistrations
            var data = new List<FooRegistration>()
            {
                new FooRegistration("logic_reg_1_1", _work1.Object, new FooToken(), new List<string> {"tag1", "tag2"}),
                new FooRegistration("logic_reg_1_2", _work2.Object, new FooToken(), new List<string> {"tag1", "tag2"})
            };
            _fooCollection.Setup(s => s.GetAllRegistrations()).Returns(data);

            // act
            var testImplementer = new FooLogicImplementer(new List<IFooCollection> { _fooCollection.Object });

            // assert
            var res = await testImplementer.ExecuteLogic();

            Xunit.Assert.Equal(2, res.Data.Count);
        }
    }
}
