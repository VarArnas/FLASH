# Flash: A Dynamic Learning Application

**Flash** is an innovative application designed to revolutionize the learning process for students preparing for exams and tests. Developed in collaboration with @bestoxl and @VarArnas, **Flash** creates a fun and playful digital environment, making studying more efficient, intuitive, and enjoyable. Built using C# and .NET with WPF for GUI, and utilizing an SQL local database to store data, **Flash** offers a range of features to enhance the learning experience.

## Key Features:
* **Flashcard Management:** Users can create, delete, or edit flashcard sets within the intuitive flashcard options window. Additionally, a search feature allows for easy filtering of flashcards.
![Screenshot](examples/pic1.png) </br>
* **Customization:** Customize individual flashcards to suit specific learning needs within the flashcard customization window, accessed through editing the flashcard set.
![Screenshot](examples/pic2.png) </br>
* **Interactive Learning:** Test your knowledge with randomized flashcards by pressing the play button, providing a challenging yet effective learning experience.
![Screenshot](examples/pic3.png) </br>
* **ChatGPT Integration:** Enter the ChatGPT play window mode for additional learning assistance. ChatGPT evaluates answers' likelihood, offering valuable feedback to enhance understanding.
![Screenshot](examples/pic4.png) </br>
* **Statistics Tracking:** Monitor progress and performance through the statistics window, providing insights into learning patterns and areas for improvement.
![Screenshot](examples/pic5.png) </br>
* **Program Shortcuts:** Utilize program shortcuts for easier navigation, enhancing user accessibility and efficiency.
![Screenshot](examples/pic6.png) </br>
* **Timer Functionality:** Set timers for each flashcard to manage study sessions effectively and maintain focus.
![Screenshot](examples/pic7.png)
![Screenshot](examples/pic8.png) </br>
* **GPT mode** evaluating answers likelyhood.
![Screenshot](examples/pic10.png) </br>
![Screenshot](examples/pic11.png) </br>
![Screenshot](examples/pic12.png) </br>
![Screenshot](examples/pic13.png) </br>
* Ability to **alter the text** itself once playing the flashcards.
![Screenshot](examples/pic15.png) </br>
* **Difficulty Settings:** Determine flashcard set difficulty based on customizable color options during flashcard customization.
![Screenshot](examples/pic16.png) </br>
	
## How to run:
To run **Flash:**
1. Ensure dependencies are installed.
2. Set the local database full path inside DataContext.cs connection string for example : @"Data Source = C:\Users\arnas\Source\Repos\FLASH\FirstLab\FirstLab\src\data\myDatabase.db".
3. Update migrations in the console using ```dotnet ef database update```.
4. Build the application using ```dotnet build```.
5. Optionally, integrate ChatGPT's API key within OpenAIController.cs for enhanced functionality, either create your own with an OpenAI API account or use the one provided.

**Note:** Due to limited funding, a general ChatGPT API key is not provided. Users can add their own API key for ChatGPT integration
	

