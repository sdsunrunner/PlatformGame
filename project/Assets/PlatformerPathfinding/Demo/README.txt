README Platformer Pathfinding

//***********************************************************************************************//

Demo Setup:

Step One)

Create the following tags and layers for your unity project.
(case sensitive)

 ground
 ladder
 portal
 oneway

Step Two)

Inside the PlatformerPathfinding->Demo->Prefabs folder, assign the tags and layers to the appropriate prefabs.
(Do not change child layers, it is not necessary)

Also, inside resources folder, do the same for the GroundTile. 
(this will allow creating and deleting blocks to work during runtime)


//**********************************************************************************************//

Using Platformer Pathfinding API:

To request a path, simply call the function inside the PathfindingAgent script, make sure to pass in
a parameter of a Vector3 or a GameObject. If a path is possible to, the character will receive instructions
and if the character's variable: isAiControlled is set to true, the character will attempt to follow the 
instructions.

e.g: 
	//myVector3 is a Vector3
	myCharacter.GetComponent<PathfindingAgent>().RequestPath(myVector3); 

	//or

	//chaseThisGameObject is a GameObject
	myCharacter.GetComponent<PathfindingAgent>().RequestPath(chaseThisGameObject);

You can use AiController to create custom AI sequences that utilize the Pathfinding.
There are 3 main examples for AI:

 -Pathfinding, which navigates the AI towards a GameObject or Vector3.
 -GroundPatrol which is the traditional patrol AI, but when the "player" is near, it attempts to flee.
 -PathfindingChase, follows pathfinding unless the "player" is near, it attempts to chase.


