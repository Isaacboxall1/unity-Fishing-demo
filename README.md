# Fishing Prototype

A small 2D fishing prototype built in Unity, inspired by fishing minigames in games such as Stardew Valley. The focus of this project was to create a complete gameplay loop while exploring Unity's core workflows and systems within a limited development period. Created over the course of 2 days.

![Gameplay](Docs/fishing-prototype-gameplay.gif)

## Features

* Cast, bite and reaction-based fishing loop
* Stardew Valley-inspired fishing minigame
* Fish-specific movement and difficulty settings using ScriptableObjects
* Catch inventory with dynamically generated UI
* Animated player states and contextual UI prompts
* Dynamic fishing line and scrolling layered backgrounds
* WebGL build playable in the browser

## Controls

* **E** - Cast / hook fish / dismiss catch popup
* **Space** - Control the catch bar during the fishing minigame
* **Q** - Open / close inventory

## Development & Deployment

A GitHub Actions workflow automatically builds the Unity WebGL project and deploys the latest version to itch.io whenever changes are pushed to the `main` branch.

## Play

The latest build is available on itch.io:

**[Play on itch.io](https://isaacboxall.itch.io/fishing-prototype)**
