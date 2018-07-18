#OMDb Web API client realized in .NET Framework with WPF and MVVM pattern.

#Getting Started

- You can remove favorited movies by choosing "Remove" from the context menu in the favorites tab
- You can download poster in the watched tab by choosing "Download poster" from the context menu
- You can select the imports folder in the watched tab by pressing the '...' button or by inserting the path in the input control
- Export favorites button is removed as favorites are asynchronusly updated when the user adds or removes items from favorites
- Favorites are saved in Favoreties.txt as JSON, last path which is used in watched tab is saved in Lastpath.txt, last search is save in Lastsearch.txt 

#Note
***Movies that can not be found on OMDbAPI will not be imported but only folder names which do not match the name pattern will be displayed in the notification

#Installing

Run OMDbAPIClient.exe, you will need .NET Framework to run the application

#Built With Visual Studio 2017 in .NET Framework with WPF

#Versioning I used git for versioning with VSTS an than moved the project to GitHub

#Authors Smail Galijasevic | smailgalijasevic@gmail.com

#License This project is licensed under the MIT License - see the LICENSE.md file for details