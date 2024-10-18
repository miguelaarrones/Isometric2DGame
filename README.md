# Isometric2DGame
A demo game made as a techincal test for [RunicDices](https://runicdices.io)

This game was made in 5 days using Unity version 2022.3.44f1 with the 2D URP Template.
This is a technical demo test, so it is not a 100% polished.

## Thoughts on this project
I took me a total of 13:20 hours to complete this project, a lot less than the 34 hours that I expected it would take. To be honest, I underestimated how fast some things would be to implement and also, the expected time is counting some things that I though I could do, but I did not.
The main tasks (Project Setup, Player Controller, Camera Setup and the Enemy) where really fast to do, if I did them correctly.
I'm not really used to 2D games but I think I did a great job, even doing some UI work that I'm not used to.

My idea for this task, was to first get everything working, the first few hours I did not care too much about if it was really well written or if it was the perfect way of doing it, I just wanted it to work.
Once I got everything working, I started to polish it, code refactoring, files organization and scene organization. I implemented some post processing, sounds and a main menu, as I felt it needed it.
This is the best way I though for a project like this, with a few tasks and limited time.

## Workflow Diagram
For this test, it was required to create a workflow diagram showing the tasks that are requested in the PDF. The diagram can be found [here](https://drive.google.com/file/d/16adVBFA2TAIyd_BKKFT4JV9KwQICydrs/view?usp=sharing).

## General tasks time estimation
- Unity Project and GitHub setup: **0:30h**
    - Did I completed the task as estimated? Yes. **Total time: 0:30**
    - Why? It was a very simple task, just creating a GitHub repository and creating the Unity project.
    - Comments: ...

- Isometric Player movement and Input System: **3:30h**
    - Did I completed the task as estimated? It took less time than expected. **Total time: 1:30h**
    - Why? The project is a protoype so I'm not polishing it a lot. And also, setting up the isometric view in 2D was easier than I expected if I did it correctly.
    - Comments: I had a hard time with the camera and the movement. In 3D this isometric view is easier and it had me thinking if I was doing it right.

- AI State Machine for Enemy Behavior: **6:00h**
    - Did I completed the task as estimated? I took me less then expected to get the state machine working with one of the bonus tasks done. I will come back to the left bonus tasks and see how much will it take for me to complete them. **Total time: 2:30h**
    - Why? I think I'm underestimating my knowledge, It's been a few weeks without me touching anything related to Unity and I feel that I know more than I thought.
    - Comments: I tried to implement A* pathfinding but I ended giving up, as this 2D isometric view confuses me and the only time I did A* was in a 3D environment.

- Basic Combat System: **7:00h**
    - Did I completed the task as estimated? Yes, again, it took me less than expected. **Total time: 3:30**
    - Why? If I'm being honest, I'm actually fast prototyping everything, I think it will all add up in time once I start polishing and debugging.
    - Comments: If this project had a more open task, like "The player might have more attacks, or abilities", I probably would have done things different, but for the scope of this project I think it is good enough.

- Simple Inventory System: **5:00h**
    - Did I completed the task as estimated? I completed the mandatory tasks and one bonus task in half the time I expected. **Total time: 2:30**
    - Why? The UI is something that I'm not really good at, I had some trouble thinking on how to make it work and the user being able to interact with it. I thought I could do the other Bonus tasks, but being realistic, after my work in this section, I'm not sure if I'm able to do it.
    - Comments: Had a lot of trouble with the UI and the New Unity Input System. I'm not used to working in the UI of my games and this was the first time I really tried to work with this New Input System.

- Dialogue System with a custom Editor Tool: **11:00h**
    - Did I completed the task as estimated? I never did this.
    - Why? It escapes from the knowledge I actually have on Unity.
    - Comments: I really thought I could do this, but I don't know where to start. Also, I never tried to create a custom Editor tool, but I will try in the future for sure.

## Polishing
- First iteration time: **0:45h**
    - Did some code refactor and file organization to make everything more clear.
- Second iteration time: **0:50h**
    - I refactorized some code and made small changes.
- Third iteration time: **1:13**
    - Added Sounds, Post Processing and a Main Menu. Had to include the old input system to the project for managing the two simple buttons from the Main Menu.

## Assets used
- Sound effects made with [jsfxr](https://sfxr.me).
- Main theme **Retro Platforming** by David Fesliyan from [Fesliyan Studios](https://www.fesliyanstudios.com).
