namespace OakbrookFinanceTechTest.Test
{
    using Moq;
    using Newtonsoft.Json;
    using NUnit.Framework;
    using OakbrookFinanceTechTest.Model;
    using OakbrookFinanceTechTest.Repository.Interfaces;
    using OakbrookFinanceTechTest.Service;
    using OakbrookFinanceTechTest.Service.Interfaces;

    [TestFixture]
    public class DetailsServiceTests
    {
        private IDetailsService sut;
        private Mock<ILogger> mockLogger;
        private Mock<IDetailsRepository> mockDetailsRepo;
        private Mock<IDecisionApiRepository> mockDecisionRepo;
        private DetailsModel detailsModel;

        [SetUp]
        public void SetUp()
        {
            this.mockDetailsRepo = new Mock<IDetailsRepository>();
            this.mockLogger = new Mock<ILogger>();
            this.mockDecisionRepo = new Mock<IDecisionApiRepository>();
            this.detailsModel = new DetailsModel { FirstName = "Test", LastName = "McTesterson", DateOfBirth = "2002-02-02" };
            this.sut = new DetailsService(this.detailsModel, this.mockDetailsRepo.Object, this.mockDecisionRepo.Object, this.mockLogger.Object);
        }

        [Test]
        public void ProcessInput_ShouldReturn_EmptyDetailsModel_WhenGiven_NoDetailsModel()
        {           
            Assert.AreEqual(JsonConvert.SerializeObject(new DetailsModel()), JsonConvert.SerializeObject(this.sut.ProcessInput(null).Result));
        }

        [Test]
        public void ProcessInput_ShouldReturn_DetailsModelWithNonNullResult_WhenGiven_DetailsModel()
        {
            this.mockDecisionRepo.Setup(x => x.PostDetails(this.detailsModel)).ReturnsAsync(new DetailsModel { FirstName = "Test", LastName = "McTesterson", DateOfBirth = "2002-02-02", Result = "Accepted" });

            Assert.IsNotNull(this.sut.ProcessInput(this.detailsModel).Result);
        }

        [Test]
        public void ProcessInput_ShouldReturn_DetailsModel_WithNonNullResult_WhenGiven_DetailsModel_WithId()
        {
            this.mockDecisionRepo.Setup(x => x.PostDetails(It.IsAny<DetailsModel>())).ReturnsAsync(new DetailsModel { FirstName = "Test", LastName = "McTesterson", DateOfBirth = "2002-02-02", Result = "Accepted", Id = 22});
            this.detailsModel.Id = 22;

            Assert.IsNotNull(this.sut.ProcessInput(this.detailsModel).Result);
        }

        [Test]
        public void ProcessInput_ShouldCall_StoreDetails_Once_WhenGiven_DetailsModel_WithNoId()
        {
            this.mockDecisionRepo.Setup(x => x.PostDetails(It.IsAny<DetailsModel>())).ReturnsAsync(new DetailsModel { FirstName = "Test", LastName = "McTesterson", DateOfBirth = "2002-02-02", Result = "Accepted", Id = 22 });

            this.sut.ProcessInput(this.detailsModel);

            mockDetailsRepo.Verify(x => x.StoreDetails(It.IsAny<DetailsModel>()), Times.Once);
        }

        [Test]
        public void ProcessInput_ShouldNotCall_StoreDetails_WhenGiven_DetailsModel_WithId()
        {
            this.mockDecisionRepo.Setup(x => x.PostDetails(It.IsAny<DetailsModel>())).ReturnsAsync(new DetailsModel { FirstName = "Test", LastName = "McTesterson", DateOfBirth = "2002-02-02", Result = "Accepted" });
            this.detailsModel.Id = 22;
            
            this.sut.ProcessInput(this.detailsModel);

            mockDetailsRepo.Verify(x => x.StoreDetails(It.IsAny<DetailsModel>()), Times.Never);
        }

        [Test]
        public void ProcessInput_ShouldCall_StoreResult_Once_WhenGiven_DetailsModel_WithId()
        {
            this.mockDecisionRepo.Setup(x => x.PostDetails(It.IsAny<DetailsModel>())).ReturnsAsync(new DetailsModel { FirstName = "Test", LastName = "McTesterson", DateOfBirth = "2002-02-02", Result = "Accepted", Id = 22 });
            this.detailsModel.Id = 22;
            
            this.sut.ProcessInput(this.detailsModel);

            mockDetailsRepo.Verify(x => x.StoreResult(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

    }
}
