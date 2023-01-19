# VoRtex-School
VoRtex School is a virtual worlds platform for education. Developed in Unity game engine using UNet networking, IBM Watson, Forge Networking, OpenCV, MySQL and Ethereum.

Educator needs are represented through the following system requirements:

•	Implement an access system for user authorization and authentication (security)

•	Enable tools for creating a virtual classroom without additional programming skills (VW design)

•	Enable the deployment of the virtual classroom to end-users on cloud servers or local (institutions) machines (scalability)

•	Enable virtual assists during the session that support teachers for predeined courses (content generation)

•	Design immersive environment that can be used via different user interfaces (Communication using PC desktop or VR devices)



The VoRtex prototype adapts the MicroLessons concept. MicroLessons are sessions prepared and created by the teacher using a web platform or the client application on a desktop PC. Also, the MicroLessons structure includes challenges like problem solving puzzles for different use cases like learning a new programming algorithm or exploring space to find required objects. MicroLesson practical examples are:

•	Build 3D interactive simulation (introduction to game development)

•	Bank resource management and critical thinking (introduction to finance and decision making)

•	Real-world physics (a lesson about laws of physics)

•	Scientific method (chemistry and biology)

•	Flora and Fauna exploration (vegetation and animal)

•	Earth exploration and time travel (geography and history)


![alt text](https://github.com/Aca1990/VoRtex-School/blob/master/Pictures/VoRtex%20high-level%20layer-based%20architecture.png?raw=true)

Figure 1. VoRtex high-level layer-based architecture.


![alt text](https://github.com/Aca1990/VoRtex-School/blob/master/Pictures/Web%20platform%20component%20software%20design.png?raw=true)

Figure 2. Web platform component software design.


![alt text](https://github.com/Aca1990/VoRtex-School/blob/master/Pictures/VoRtex%20high-level%20component-based%20architecture.png?raw=true)

Figure 3. VoRtex high-level component-based architecture.

![alt text](https://github.com/Aca1990/VoRtex-School/blob/master/Pictures/Intelligent%20agents%20software%20design.png?raw=true)

Figure 4. Intelligent agents software design.


![alt text](https://github.com/Aca1990/VoRtex-School/blob/master/Pictures/VoRtex%20virtual%20environment%20(Windows%20desktop%2C%20first-person%20perspective).png?raw=true)

Figure 5. VoRtex virtual environment (Windows desktop, first-person perspective).



VoRtex prototype video (click below)
[![VoRtex prototype video](https://github.com/Aca1990/VoRtex-School/blob/master/Pictures/VoRtex%20virtual%20environment%20(Windows%20desktop%2C%20third-person%20perspective).png)](https://www.youtube.com/watch?v=xmUY6tadgkA&ab_channel=VoRtexteam "VoRtex prototype video")

VoRtex prototype build: https://mega.nz/folder/kQJkTL6L#tfWQAO-l6k7RXPdrubmYsg
VoRtex prototype Blockchain build: https://mega.nz/folder/cFJXmKyZ#LYC8-vZX6qvoOPbdwbytuw

Install Requirement:

•	Unity 2019.3.6f1 (64-bit)

•	MAMP 4.1.0

•	MySQL Workbench (8.0.22 or later)

Set up the database

•	Import useraccess.sql and social.sql from VortexPrototype\SQLSetup

•	Copy data from VortexPrototype\MAMPSetup into C:\MAMP\htdocs

IBM Watson setup

•	Text to speech:Enable Prof. VoRtex voice, check ExampleAssistantV2 and setup Watson service

•	Speech to text: Add Assets/Prefabs/ExampleStreaming.prefab inside scene

•	More info: https://github.com/watson-developer-cloud/unity-sdk

Blockchain

•	Enable Assets/Scripts/WalletManager.cs script inside [ManagerComponents]

•	More info: http://casopisi.junis.ni.ac.rs/index.php/FUAutContRob/article/view/4660

•	Demo: https://www.youtube.com/watch?v=GFJWu-_wlcQ&t=18s&ab_channel=VoRtexteam

Biometric check:

•	Uncomment FaceRecognitionManager.Star inside source code.

Unity VR support

•	To enable VR for your game builds and the editor, open the Player Settings (menu: Edit > Project Settings > Player). Select Other Settings and check the Virtual Reality Supported checkbox.

•	Change to First person mode (VR view) using "C" button on keyboard.

Contact author for support.

Reference: https://www.mdpi.com/2079-9292/11/3/317
