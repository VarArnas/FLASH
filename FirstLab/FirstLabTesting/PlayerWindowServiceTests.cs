using FirstLab.Migrations;
using FirstLab.src.exceptions;
using FirstLab.src.interfaces;
using FirstLab.src.models;
using FirstLab.src.services;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Xunit.Sdk;

namespace FirstLabTesting;

public class PlayerWindowServiceTests : IClassFixture<PlayWindowServiceTestFixture>
{
    private readonly PlayWindowServiceTestFixture _fixture;



    public PlayerWindowServiceTests(PlayWindowServiceTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.ResetMocks();
    }

    public static IEnumerable<object[]> CheckIfPreviousOrNextData()
    {
        var flashcardSet = new FlashcardSet
        {
            FlashcardSetName = "Sample Set",
            FlashcardSetDifficulty = "Medium",
            Flashcards = new ObservableCollection<Flashcard>
            {
                new Flashcard { FlashcardName = "Card1", FlashcardQuestion = "SampleQ", FlashcardAnswer = "SampleA", FlashcardColor = "IndianRed", FlashcardTimer = "3" },
                new Flashcard { FlashcardName = "Card2", FlashcardQuestion = "SampleQ", FlashcardAnswer = "SampleA", FlashcardColor = "RoyalBlue", FlashcardTimer = "3" },
                new Flashcard { FlashcardName = "Card3", FlashcardQuestion = "SampleQ", FlashcardAnswer = "SampleA", FlashcardColor = "Yellow", FlashcardTimer = "3" },
                new Flashcard { FlashcardName = "Card4", FlashcardQuestion = "SampleQ", FlashcardAnswer = "SampleA", FlashcardColor = "Green", FlashcardTimer = "3" },
                new Flashcard { FlashcardName = "Card5", FlashcardQuestion = "SampleQ", FlashcardAnswer = "SampleA", FlashcardColor = "Orange", FlashcardTimer = "3" }
            }
        };

        yield return new object[] { true, 1, flashcardSet, true, 0 };
        yield return new object[] { true, 0, flashcardSet, true, 0 };
        yield return new object[] { true, 3, flashcardSet, true, 2 };

        yield return new object[] { false, 6, flashcardSet, true, 6 };
        yield return new object[] { false, 2, flashcardSet, false, 3 };
        yield return new object[] { false, 4, flashcardSet, false, 4 };


    }

    [Theory]
    [MemberData(nameof(CheckIfPreviousOrNextData))]
    public void CheckIfPreviousOrNext_CheckingAllDifferentOutcomesWithBools_ReturnsExpectedIndex(bool isPrevious, int currentIndex, FlashcardSet flashcardSet, bool isStart, int expectedIndex)
    {
        // Arrange
        var service = _fixture.PlayWindowService;

        // Act
        int result = service.CheckIfPreviousOrNext(isPrevious, currentIndex, flashcardSet, isStart);

        // Assert
        Assert.Equal(expectedIndex, result);
    }

    [Fact]
    public void SetAnimation_CreatesExpectedAnimation()
    {
        //Arrange

        //Act
        var animation = _fixture.PlayWindowService.SetAnimation();

        // Assert
        Assert.Equal(1.0, animation.From);
        Assert.Equal(0.1, animation.To);
        Assert.Equal(TimeSpan.FromSeconds(2), animation.Duration);
        Assert.True(animation.AutoReverse);
        Assert.Equal(RepeatBehavior.Forever, animation.RepeatBehavior);
    }

    [Theory]
    [InlineData(true, false, true)]
    [InlineData(false, true, false)]
    [InlineData(false, false, false)]
    public void DetermineQuestionOrAnswer_GivenFlags_ReturnsExpectedResult(bool question, bool answer, bool expectedResult)
    {
        // Arrange

        // Act
        var result = _fixture.PlayWindowService.DetermineQuestionOrAnswer(question, answer);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void GetFlashcardIndex_WithValidFlashcardSet_ReturnsCorrectIndex()
    {
        // Arrange
        var flashcardSet = new FlashcardSet
        {
            FlashcardSetName = "Sample Set",
            FlashcardSetDifficulty = "Medium",
            Flashcards = new ObservableCollection<Flashcard> 
            {
                new Flashcard { FlashcardName = "Card1" },
                new Flashcard { FlashcardName = "Card2" }

            }
        };
        var flashcard = flashcardSet!.Flashcards![1];

        // Act
        var index = _fixture.PlayWindowService.GetFlashcardIndex(flashcard, flashcardSet);

        // Assert
        Assert.Equal(1, index);
    }

    [Fact]
    public void CreateTextAndBorderPropertiesPlayWindow_InvokesFactoryContainerWithCorrectParameters_ReturnsExpectedObject()
    {
        // Arrange
        var mockFactoryContainer = Mock.Get(_fixture.FactoryContainer);
        var expectedObject = new TextAndBorderPropertiesPlayWindow("1/5", "Sample Text", Brushes.IndianRed, Visibility.Visible, Visibility.Collapsed);

        mockFactoryContainer.Setup(f => f.CreateTextAndBorderPropertiesPlayWindow(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SolidColorBrush>(), It.IsAny<Visibility>(), It.IsAny<Visibility>()))
                            .Returns(expectedObject);

        // Act
        var result = _fixture.PlayWindowService.CreateTextAndBorderProperties("1/5", "Sample Text", Brushes.IndianRed, Visibility.Visible, Visibility.Collapsed);

        // Assert
        Assert.Equal(expectedObject, result);
        mockFactoryContainer.Verify(f => f.CreateTextAndBorderPropertiesPlayWindow(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SolidColorBrush>(), It.IsAny<Visibility>(), It.IsAny<Visibility>()), Times.Once);
    }

    [Fact]
    public void ThrowCustomException_InvokesCreateExceptionWithValidMessage_ThrowsCustomNullException()
    {
        // Arrange
        var mockFactoryContainer = Mock.Get(_fixture.FactoryContainer);
        var customException = new CustomNullException("Test Exception");
        mockFactoryContainer.Setup(f => f.CreateException(It.IsAny<string>())).Returns(customException);

        // Act & Assert
        var exception = Assert.Throws<CustomNullException>(() =>
            _fixture.PlayWindowService.ThrowCustomException("Test Exception", new Exception()));

        Assert.Equal("Exception: Test Exception", exception.Message);
        mockFactoryContainer.Verify(f => f.CreateException(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void SetQuestionOrAnswerProperties_NoColor_ThrowsNullException()
    {
        // Arrange
        var mockFlashcard = new Flashcard { FlashcardName = "Card1",  FlashcardQuestion = "Q1", FlashcardAnswer = "A1", FlashcardColor = "", FlashcardTimer = "3" };
        var flashcardSet = new FlashcardSet
        {
            FlashcardSetName = "Sample Set",
            FlashcardSetDifficulty = "Medium",
            Flashcards = new ObservableCollection<Flashcard> { mockFlashcard }
        };

        // Act & Assert
        Assert.Throws<NullReferenceException>(() =>
            _fixture.PlayWindowService.SetQuestionOrAnswerProperties(true, false, mockFlashcard, flashcardSet));
    }

    [Fact]
    public void SetQuestionOrAnswerProperties_SomeColor_DoesntThrowNullException()
    {
        // Arrange
        var mockFlashcard = new Flashcard { FlashcardName = "Card1", FlashcardQuestion = "Q1", FlashcardAnswer = "A1", FlashcardColor = "IndianRed", FlashcardTimer = "3" };
        var flashcardSet = new FlashcardSet
        {
            FlashcardSetName = "Sample Set",
            FlashcardSetDifficulty = "Medium",
            Flashcards = new ObservableCollection<Flashcard> { mockFlashcard }
        };

        //Act
        var exception = Record.Exception(() => _fixture.PlayWindowService.SetQuestionOrAnswerProperties(true, false, mockFlashcard, flashcardSet));

        //Assert
        Assert.Null(exception);
    }


    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void SetQuestionOrAnswerProperties_ValidInputsWithEitherQuestionOrAnswer_ReturnsCorrectPropertiesWithVisibilitiesAndText(bool question, bool answer)
    {
        // Arrange
        var mockFlashcard = new Flashcard { FlashcardName = "Card1", FlashcardQuestion = "Q1", FlashcardAnswer = "A1", FlashcardColor = "IndianRed", FlashcardTimer = "3" };
        var flashcardSet = new FlashcardSet
        {
            FlashcardSetName = "Sample Set",
            FlashcardSetDifficulty = "Medium",
            Flashcards = new ObservableCollection<Flashcard> { mockFlashcard }
        };
        string expectedText = question ? mockFlashcard.FlashcardQuestion : mockFlashcard.FlashcardAnswer;
        Visibility expectedQuestionVisibilty = question ? Visibility.Visible : Visibility.Collapsed;
        Visibility expectedAnswerVisibility = answer ? Visibility.Visible: Visibility.Collapsed;


        var mockFactoryContainer = Mock.Get(_fixture.FactoryContainer);

        // Act
        var result = _fixture.PlayWindowService.SetQuestionOrAnswerProperties(question, answer, mockFlashcard, flashcardSet);

        // Assert
        mockFactoryContainer.Verify(f => f.CreateTextAndBorderPropertiesPlayWindow(
            It.Is<string>(s => s == "1/1"),
            It.Is<string>(s => s == expectedText),
            It.IsAny<SolidColorBrush>(),
            It.Is<Visibility>(v => v == expectedQuestionVisibilty),
            It.Is<Visibility>(v => v == expectedAnswerVisibility)), Times.Once);
    }

    [Theory]
    [InlineData("5 seconds", 5)]
    [InlineData("5", 5)]
    [InlineData("", 0)]
    [InlineData("seconds      5", 5)]
    public void ParseCounter_DifferentSelectedTimeInputs_ReturnsCorrectNumberFromString(string selectedTime, int actualTime)
    {
        //Arrange

        //Act
        var result = _fixture.PlayWindowService.ParseCounter(selectedTime);

        //Assert
        Assert.Equal(actualTime, result);
    }

    [Fact]
    public void FindCounter_NoTimeGiven_ThrowsNullException()
    {
        //Arrange
        var mockFlashcard = new Flashcard { FlashcardName = "Card1", FlashcardTimer = null };

        //Act & Assert
        Assert.Throws<NullReferenceException>(() =>
            _fixture.PlayWindowService.FindCounter(mockFlashcard));
    }

    [Theory]
    [InlineData("5 seconds")]
    [InlineData("5")]
    [InlineData("seconds 5")]
    public void FindCounter_ValidTimer_DoesNotThrowException(string timer)
    {
        // Arrange
        var mockFlashcard = new Flashcard { FlashcardName = "Card1", FlashcardTimer = timer };

        // Act
        var exception = Record.Exception(() => _fixture.PlayWindowService.FindCounter(mockFlashcard));

        //Assert
        Assert.Null(exception);
    }

    [Fact]
    public void CreateCounter_NullTimer_ThrowsAnException()
    {
        // Arrange
        var mockFlashcard = new Flashcard { FlashcardName = "Card1", FlashcardTimer = null };
        int counter = 5;

        // Act
        Exception exception = Record.Exception(() => _fixture.PlayWindowService.CreateCounter(ref counter, mockFlashcard));

        // Assert
        Assert.IsType<NullReferenceException>(exception);
    }

    [Fact]
    public void CreateCounter_ValidTimer_SetsCounterCorrectly()
    {
        // Arrange
        var mockFlashcard = new Flashcard { FlashcardName = "Card1", FlashcardQuestion = "Q1", FlashcardAnswer = "A1", FlashcardColor = "IndianRed", FlashcardTimer = "5 seconds" };
        int counter = 50;

        // Act
        _fixture.PlayWindowService.CreateCounter(ref counter, mockFlashcard);

        // Assert
        Assert.Equal(5, counter);
    }

    [Fact]
    public void ShuffleFlashcards_ValidFlashcardSet_ReturnsAFlashcardSetWithTheSameElementsInDifferentOrder()
    {
        // Arrange
        var flashcards = new ObservableCollection<Flashcard>
        {
            new Flashcard { FlashcardName = "Card1" },
            new Flashcard { FlashcardName = "Card2" },
            new Flashcard { FlashcardName = "Card3" },
            new Flashcard { FlashcardName = "Card4" },
            new Flashcard { FlashcardName = "Card5" },
            new Flashcard { FlashcardName = "Card6" },
            new Flashcard { FlashcardName = "Card7" },
            new Flashcard { FlashcardName = "Card8" },
            new Flashcard { FlashcardName = "Card9" },
            new Flashcard { FlashcardName = "Card10" }
        };

        var originalOrder = flashcards.ToList();

        // Act
        _fixture.PlayWindowService.ShuffleFlashcards(flashcards);

        // Assert
        Assert.Equal(10, flashcards.Count); 
        Assert.All(originalOrder, card => Assert.Contains(card, flashcards));
        Assert.NotEqual(originalOrder, flashcards); // SOMETIMES MIGHT FAIL DUE TO SHUFFLE BEING ABLE TO SHUFFLE THE SAME SO JUST RETRY THE TESTS
    }


    [Fact]
    public void GetQuestionAnswerProperties_InvalidColor_ThrowsException()
    {
        // Arrange
        var mockFlashcard = new Flashcard { FlashcardName = "Card1", FlashcardQuestion = "Q1", FlashcardAnswer = "A1", FlashcardColor = "", FlashcardTimer = "3" };
        var flashcardSet = new FlashcardSet
        {
            FlashcardSetName = "Sample Set",
            FlashcardSetDifficulty = "Medium",
            Flashcards = new ObservableCollection<Flashcard> { mockFlashcard }
        };

        // Act & Assert
        Assert.Throws<NullReferenceException>(() =>
            _fixture.PlayWindowService.GetQuestionAnswerProperties(true, false, mockFlashcard, flashcardSet));
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void GetQuestionAnswerProperties_ValidInputs_ReturnsExpectedProperties(bool question, bool answer)
    {
        // Arrange
        var mockFlashcard = new Flashcard { FlashcardName = "Card1", FlashcardQuestion = "Q1", FlashcardAnswer = "A1", FlashcardColor = "IndianRed", FlashcardTimer = "3" };
        var flashcardSet = new FlashcardSet
        {
            FlashcardSetName = "SampleSet",
            FlashcardSetDifficulty = "Medium",
            Flashcards = new ObservableCollection<Flashcard>
            {
                new Flashcard { FlashcardName = "Card1", FlashcardQuestion = "Q1", FlashcardAnswer = "A1", FlashcardColor = "IndianRed", FlashcardTimer = "3"}
            }
        };

        var mockFactoryContainer = Mock.Get(_fixture.FactoryContainer);
        string expectedText = question ? mockFlashcard.FlashcardQuestion : mockFlashcard.FlashcardAnswer;
        Visibility expectedQuestionVisibilty = question ? Visibility.Visible : Visibility.Collapsed;
        Visibility expectedAnswerVisibility = answer ? Visibility.Visible : Visibility.Collapsed;

        var expectedProperties = new TextAndBorderPropertiesPlayWindow("1/1", expectedText, Brushes.IndianRed, expectedQuestionVisibilty, expectedAnswerVisibility);

        mockFactoryContainer.Setup(f => f.CreateTextAndBorderPropertiesPlayWindow(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SolidColorBrush>(), It.IsAny<Visibility>(), It.IsAny<Visibility>()))
            .Returns(expectedProperties);

        // Act
        var result = _fixture.PlayWindowService.GetQuestionAnswerProperties(question, answer, mockFlashcard, flashcardSet);

        // Assert

        Assert.Equal(expectedText, result.QuestionAnswerText);
        Assert.Equal(expectedQuestionVisibilty, result.QuestionBorderVisibility);
        Assert.Equal(expectedAnswerVisibility, result.AnswerBorderVisibility);
    }

    [Fact]
    public void CloneFlashcardSet_GivenAValidFlashcardSet_ReturnsACopyOfThatFlashcardSet()
    {
        // Arrange
        var originalSet = new FlashcardSet
        {
            FlashcardSetName = "Sample Set",
            FlashcardSetDifficulty = "Medium",
            Flashcards = new ObservableCollection<Flashcard>
            {
                new Flashcard { FlashcardName = "Card1", FlashcardQuestion = "SampleQ", FlashcardAnswer = "SampleA", FlashcardColor = "IndianRed", FlashcardTimer = "3" },
                new Flashcard { FlashcardName = "Card2", FlashcardQuestion = "SampleQ", FlashcardAnswer = "SampleA", FlashcardColor = "RoyalBlue", FlashcardTimer = "3" },
                new Flashcard { FlashcardName = "Card3", FlashcardQuestion = "SampleQ", FlashcardAnswer = "SampleA", FlashcardColor = "Yellow", FlashcardTimer = "3" },
                new Flashcard { FlashcardName = "Card4", FlashcardQuestion = "SampleQ", FlashcardAnswer = "SampleA", FlashcardColor = "Green", FlashcardTimer = "3" },
                new Flashcard { FlashcardName = "Card5", FlashcardQuestion = "SampleQ", FlashcardAnswer = "SampleA", FlashcardColor = "Orange", FlashcardTimer = "3" }
            }
        };

        var mockFactoryContainer = Mock.Get(_fixture.FactoryContainer);
        mockFactoryContainer.Setup(f => f.CreateObject<FlashcardSet>())
                        .Returns(() => new FlashcardSet { Flashcards = new ObservableCollection<Flashcard>() });

        // Act
        var clonedSet = _fixture.PlayWindowService.CloneFlashcardSet(originalSet);

        // Assert
        Assert.Equal(originalSet.FlashcardSetName, clonedSet.FlashcardSetName);
        Assert.Equal(originalSet.Flashcards.Count, clonedSet.Flashcards.Count);
        for (int i = 0; i < originalSet.Flashcards.Count; i++)
        {
            Assert.Equal(originalSet.Flashcards[i].FlashcardName, clonedSet.Flashcards[i].FlashcardName);
            Assert.Equal(originalSet.Flashcards[i].FlashcardQuestion, clonedSet.Flashcards[i].FlashcardQuestion);
            Assert.Equal(originalSet.Flashcards[i].FlashcardAnswer, clonedSet.Flashcards[i].FlashcardAnswer);
            Assert.Equal(originalSet.Flashcards[i].FlashcardColor, clonedSet.Flashcards[i].FlashcardColor);
            Assert.Equal(originalSet.Flashcards[i].FlashcardTimer, clonedSet.Flashcards[i].FlashcardTimer);
        }
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void SetTextProperties_GivenFlags_ReturnsExpectedProperties(bool isHighlighted, bool isItalic)
    {
        // Arrange
        var mockFactoryContainer = Mock.Get(_fixture.FactoryContainer);
        var expectedFontWeight = isHighlighted ? FontWeights.Bold : FontWeights.Normal;
        var expectedFontStyle = isItalic ? FontStyles.Italic : FontStyles.Normal;

        // Act
        var result = _fixture.PlayWindowService.SetTextProperties(isHighlighted, isItalic);

        // Assert
        mockFactoryContainer.Verify(f => f.CreateTextModificationProperties(
            It.Is<bool>(b => b == isHighlighted), 
            It.Is<bool>(b => b == isItalic), 
            It.Is<FontWeight>(fw => fw == expectedFontWeight), 
            It.Is<FontStyle>(fs => fs == expectedFontStyle)), Times.Once);
    }


    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void SetQuestionOrAnswerProperties_ValidInputsWithEitherQuestionOrAnswer_ReturnsCorrecdatPropertiesWithVisibilitiesAndText(bool question, bool answer)
    {
        // Arrange
        var mockFlashcard = new Flashcard { FlashcardName = "Card1", FlashcardQuestion = "Q1", FlashcardAnswer = "A1", FlashcardColor = "IndianRed", FlashcardTimer = "3" };
        var flashcardSet = new FlashcardSet
        {
            FlashcardSetName = "Sample Set",
            FlashcardSetDifficulty = "Medium",
            Flashcards = new ObservableCollection<Flashcard> { mockFlashcard }
        };
        string expectedText = question ? mockFlashcard.FlashcardQuestion : mockFlashcard.FlashcardAnswer;
        Visibility expectedQuestionVisibilty = question ? Visibility.Visible : Visibility.Collapsed;
        Visibility expectedAnswerVisibility = answer ? Visibility.Visible : Visibility.Collapsed;


        var mockFactoryContainer = Mock.Get(_fixture.FactoryContainer);

        // Act
        var result = _fixture.PlayWindowService.SetQuestionOrAnswerProperties(question, answer, mockFlashcard, flashcardSet);

        // Assert
        mockFactoryContainer.Verify(f => f.CreateTextAndBorderPropertiesPlayWindow(
            It.Is<string>(s => s == "1/1"),
            It.Is<string>(s => s == expectedText),
            It.IsAny<SolidColorBrush>(),
            It.Is<Visibility>(v => v == expectedQuestionVisibilty),
            It.Is<Visibility>(v => v == expectedAnswerVisibility)), Times.Once);
    }

    [Fact]
    public void HandleNullColor_GivenAFlashcard_SetsDefaultColor()
    {
        // Arrange
        var mockFlashcard = new Flashcard { FlashcardQuestion = "Card1", FlashcardColor = "Purple"};
        var exception = new CustomNullException("Test Exception");

        // Act
        _fixture.PlayWindowService.HandleNullColor(exception, mockFlashcard);

        // Assert
        Assert.Equal(exception.defaultColor, mockFlashcard.FlashcardColor);
    }

    [Theory]
    [InlineData(true, 5, 10, 15)]
    [InlineData(false, 3, 10, 7)]
    public void FindNewTextSize_GivenParameters_ReturnsExpectedSize(bool increaseSize, int sizeChange, double presentFontSize, double expectedFontSize)
    {
        // Arrange
        var flashcardDesign = new FlashcardDesign
        {
            IncreaseTextSize = sizeChange,
            DecreaseTextSize = sizeChange
        };

        // Act
        var newSize = _fixture.PlayWindowService.FindNewTextSize(increaseSize, flashcardDesign, presentFontSize);

        // Assert
        Assert.Equal(expectedFontSize, newSize);
    }

    [Fact]
    public void SetSlidePanelAnimation_TogglesVisibility_ReturnsCorrectAnimation()
    {
        // Arrange

        // Act & Assert
        Assert.Equal("SlideOutAnimation", _fixture.PlayWindowService.SetSlidePanelAnimation());
        Assert.Equal("SlideInAnimation", _fixture.PlayWindowService.SetSlidePanelAnimation());
    }


    public static IEnumerable<object[]> IsLastIndexData()
    { 
        var flashcardSet2 = new FlashcardSet
        {
            FlashcardSetName = "Sample Set",
            FlashcardSetDifficulty = "Medium",
            Flashcards = new ObservableCollection<Flashcard>
            {
                new Flashcard { FlashcardName = "Card1", FlashcardQuestion = "SampleQ", FlashcardAnswer = "SampleA", FlashcardColor = "IndianRed", FlashcardTimer = "3" },
                new Flashcard { FlashcardName = "Card2", FlashcardQuestion = "SampleQ", FlashcardAnswer = "SampleA", FlashcardColor = "RoyalBlue", FlashcardTimer = "3" },
                new Flashcard { FlashcardName = "Card2", FlashcardQuestion = "SampleQ", FlashcardAnswer = "SampleA", FlashcardColor = "RoyalBlue", FlashcardTimer = "3" }
            }
        };

        yield return new object[] { 0, flashcardSet2, false };
        yield return new object[] { 2, flashcardSet2, true };
    }

    [Theory]
    [MemberData(nameof(IsLastIndexData))]
    public void IsLastIndex_GivenIndexAndAValidFlashcardSet_ReturnsExpectedResult(int index, FlashcardSet flashcardSet, bool expectedResult)
    {
        // Arrange

        // Act
        var result = _fixture.PlayWindowService.IsLastIndex(index, flashcardSet);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, false)]
    public void IsFirstOrZeroIndex_GivenIndex_ReturnsExpectedResult(int index, bool expectedResult)
    {
        // Arrange & Act
        var result = _fixture.PlayWindowService.IsFirstOrZeroIndex(index);

        // Assert
        Assert.Equal(expectedResult, result);
    }







































}
