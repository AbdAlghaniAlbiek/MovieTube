# MovieTube
<p align="center">
  <img src="https://github.com/AbdAlghaniAlbiek/MovieTube/blob/master/MovieTube/Assets/MovieTubeIcon.png"> 
</p>

![Twitter Follow](https://img.shields.io/twitter/follow/AbdAlbiek?style=social) ![GitHub](https://img.shields.io/github/license/AbdAlghaniAlbiek/SQLiteDBProject)


# Table of content
* [General Info](#general-information)
* [Tecnologies Used](technologies-used)
* [Features](#features)
* [Screenshots](#screenshots)
* [Setup](#setup)
* [Versions](#versions)
* [Project Status](#project-status)

## General Information
* This UWP application is created to show how to stream a video content from the server to client like `YouTube` does today.
* The user can make an account then he can choose from the categories list the movies-type category that he like and inside this category there is list of movies and when user clicks on a movie the app navigates to the movie details and in this page can user rating, like/dislike, make comment and seeing the movie details (story author - director - actors ...) and play the actual movie or watch the trailer. 
* The server is Node.js backend project that contains The Resfull APIs and movies/trailers videos. you can see it [here.](https://github.com/AbdAlghaniAlbiek/movietube_backend)

## Technologies Used
* Serialize/Deserialize data.
* Consuming Restfull APIs from the server. 
* High Security level.
* Animations.
* Streaming videos.

## Features
* This app can serialize/deserialize the sended/received data from server using `Newtonsoft.Json`.
* Connecting to the server using `Refit` and fetch json data from it using Restfull APIs.
* It achieves the high Security level by Implementation this principles:
1. Encryption/Decryption data that sended/received between server and client using `AES-128-cbc` alghorithm.
2. Verify the requests that are from signed account not from any user and I achieved this using `JWT` tech.
3. To verify the token is sended from the right server, I decode token to have `sercret keyword` and check this sercret keyword if it's equal to the `stored secret keyword` in my UWP application or not.
* Using some of animations like scaling anim and ReorderAnimation for GridViews
* Streaming movies and trailers from the server is simple with using built-in control in UWP, it's called `MediaPlayerElement`.
* the user can interact with the movies like (like, dislike, comment, favourite or rating it).

## Screenshots
> To see all screenshots you can go [there.](https://github.com/AbdAlghaniAlbiek/MovieTube/tree/master/MovieTube/Assets/Screenshots)
<p align="center">
  <img src="https://github.com/AbdAlghaniAlbiek/MovieTube/blob/master/MovieTube/Assets/Screenshots/SignUp.jpg"> 
</p>
<p align="center">
  <img src="https://github.com/AbdAlghaniAlbiek/MovieTube/blob/master/MovieTube/Assets/Screenshots/Movie_Details1.jpg"> 
</p>
<p align="center">
  <img src="https://github.com/AbdAlghaniAlbiek/MovieTube/blob/master/MovieTube/Assets/Screenshots/Play_Movie.jpg"> 
</p>

## Setup
* Visual Studio 2019 at least.
* Windows 10 OS, Version: 1809 update, Build:(10.0, 17763).
* Windows 10, version 1809 SDK.
### Dependencies
There few dependencies you should to install them:
* MovieTube.Functions
1. Refit (5.1.54)
* MovieTube.UI
1. Microsoft.Toolkit.UWP.Controls (6.1.1).
2. Microsoft.Toolkit.UWP.Animation (6.1.1).
3. Microsoft.Toolkit.UWP.Connectivity (6.1.1).

## Versions
**[version 1.0.0]:** Contains all the features that descriped above.

## Project status
This project `no longer being worked on` but the contributions are still welcome.
