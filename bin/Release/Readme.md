StudioCCS - CCS Model Viewer

This is an early alpha release. Many of the planned features are currently unimplemented.
Also, this readme is a train wreck.

A note on CCS Generations:
	Generation 1: IMOQ/F
	Generation 2: GU/Link
	Generation 3: GU:LR (New or updated files)

There are two viewport modes, Preview, and Scene.

Preview mode lists all the CCS Files that have been loaded, and categorizes the interesting stuff.
Here's a breif rundown of the interesting things, just to get a sense of how the CCS format is laid out.
Source code and/or a full write up on the format will be released eventually.

Clumps:
	A "Clump" is a list of "Object" nodes, which more or less forms a skeletal structure.
	Starting in Generation 2 CCS Files, they also have a list of Position, Rotaiton and Scale vectors for the bind pose
	of each Object. Each Object has a slot for a Model and a Shadow Model. These slots are optional. For Generation 2 
	CCS Files, additional parameters appear to have been added, but at this time, their purpose is unknown.
	Currently, Rudimentary support for visualizing Objects has been implemented. It can look kinda buggy at times.
	
	Each model may have 0 or more Sub Models.
	There are 4 main types of Model, spead out between several different model type codes.
	Rigid:
		Rigid models don't have vertices that are deformable.
		
	Deformable:
		Deformable models are usually a list of sub models with single weighted vertices, followed by a sub model
		containing all of the multi-weighted vertices, or "bendy parts".
	
	Shadow:
		Shadow models are for projecting (or whatever black magic that is) shadows onto. 
		We don't bother rendering these right now.
		
	Morph Target:
		Morph Target models are for holding vertex positions for a moprh target. 
		As far as I can tell, Morph Target animation is only supported on Rigid models.
		Interestingly enough, they usually have a material or texture assignment, but no texture coordinates. 
		Some in Generation 2 files even have vertex colors.

	Clicking on a Clump in Preview Mode Tree will render every Model(and Sub Model) for every Object listed in the 
	Clump. Clicking on an Object or Model will render the Model(and Sub Models) for that Object or Model.
	You can click on each Sub Model to view only that sub model if you so wish.	
	
	You may notice when trying to view Character, Monster, or anything that uses Deformable models that most of the 
	Object nodes have empty Models excecpt for one near the end, which usually has "body" somewhere in it's name.
	I believe this is because they decided to just group all of those sub models together to make sending them and the
	matrices through to the VU easier. In the future I may fix it so that clicking on the empty model renders the 
	correct sub model.
	
Materials:
	"Materials" contain a texture assignment, an alpha value, and a texture coordinate offset. For Generation 2 CCS 
	Files, additional parameters appear to have been added, but at this time, their purpose is unknown.
	
	Currently, clicking on a Material in the Preview Mode Tree currently does nothing.
	
Textures:
	There's 2 known types of tetxures used in Generation 1 files:
		4bit Indexed Color
		8bit Indexed Color
		
	An additional format has been spotted in Generation two:
		32bit RGBA Color
	
	More texture types are apparently supported for Gen 1 & 2 games, however, I've yet to see them. If I or anyone
	else runs across them I will implement them, otherwise, at this time, support is low priority.
	
	Generation 3 adds two more texture types:
		DXT1
		DXT5
	
	Textures may also contain mipmaps, however they are currently discarded.
	
	.hack//link added support for non-power of two textures, however these are usually swizzled, and are currently 
	unsupported. I've only ever really seen them in the comic cutscene files anyways.

	Currently, clicking on a Texture will show that texture mapped to a plane in the Preview Viewport. 

HitMeshes:
	HitMeshes contain collision meshes. Each hit mesh might have a color assigned with it, presumably for coloring 
	a model that collides with it to make for a cheap way to do shading with the baked lighting.
	
	Currently clicking on a HitMesh or SubMesh in the Preview Mode Tree does nothing.
	
Bounding Boxes:
	Contain a Min and Max value, not much else.
	
	Currently, clicking on a Bounding Box in the Preview Mode Tree does nothing, and they are not visualized in
	Scene mode.
	
Dummies:
	Dummies contain a position, and sometimes a rotational value. They're useful for well, being dummies to hold 
	the  position and rotation of things.
	
	Currently, clicking on a Dummy in the Preview Mode Tree does nothing, however, they are visualized as little 
	green wireframe boxes in Scene mode.
	
Animations:
	All Camera parameters, and all paramters but the type for Lights are stored in animations.
	Animations are a pretty complex topic, and support for them is currently in progress.
	
	Currently, clicking on an Animation in the Preview Mode Tree does nothing.
	Right clicking on an Animation will give you the option "Set Pose".
	This will attempt to set everything referenced in it's first frame. This will look buggy until animation support 
	is implemented properly.
	
There are many other types of objects contained in CCS files, but the ones listed above currently have higher 
priority for support.

Currently, Scene mode will just attempt to render everything in all of the loaded CCS Files it can, 
so it may or may not show you something coherent.

Controls:
	Right Click and Drag in the Preview and Scene Viewports to rotate the arcball camera around.
		Each Viewport has an independant camera, so changing the view in one mode will not affect the other.
		There may be an option to disable this in the future if it ends up becoming annoying.
	Use the Mouse Wheel to Zoom in and Out.
	The - and + keys on the number line can also be used to Zoom in and out (See note above about the bug)
	
	Use W and S to move the camera along the Z Axis
	Use A and D to move the camera along the X Axis
	Use X and Z to move the camera along the Y Axis

	Currently these keys cannot be remapped, support for that may be added in the future.
	Support for moving relative to current camera direction will probably be added in the future as well.

Usage:
Use File -> Load to load a CCS File. The Log Window will spit out any information it feels necessary, along with 
wether or not the file was loaded successfully.

If the file was loaded successfully, the CCS File will be added to the Preview Mode Tree with it's internal name, 
not the file name.

To Unload a CCS File, right click on it in the Preview Mode Tree, and select "Unload"

Selecting "View Info Report" when right clicking on a CCS file in the Preview Mode Tree will open a window with a 
report of some information that may or may not be useful to you. I use it for debugging. Some information may be 
missing or inaccurate. more will be added as I feel like it.

Use Scene - > Dump to .OBJ to dump all Models out to a .OBJ file(with included .MTL file), and dump all textures to 
png. Note, however, that DXT1 and DXT5 textures from Gen3 files will dump to .DDS I'll get around to writing a 
DXT1/5 decoder later. maybe.

Companion python script for importing Dummies into blender:
	Open a text editor in blender, open the python script, hit Alt + p. Go back to the 3d view, and look at the 
	tool shelf. you should see a new tab that says ".hack". There you'll see the panel for importing the dummies.
	This is usefull for working with towns and stuff.	
	
Now, the part I'm sure you're really wanting to know.
How to dump a character/monster/anything with bendy parts:
	Load up the model.
	For Generation 1 files, you'll see it collapsed into the center.
		Right click on an animation, and click "Set Pose"
		I suggest you chose one with the word "nut" in the name. These are idle animations,
		which should have the least amount of movement, which is important because the animations
		do much more than just rotate the bones.
		This will set the model into a weird pose, but that's okay, because what we care about
		is the positions of the bones right now.
		
	For Generation 2 & 3 you'll see it in some weird ass pose. This is (usually) fine.
		Apply an animation pose at your own risk. I've seen it stetch the shit out of some characters.
	
	Right click on the clump in the Preview Mode Tree, and select "Edit Bones".
	
	The Bone Edit window will popup. Click the Edit menu, and select "Clear Rotation Values"
	This will give you the infamous .hack//Lawn Chair pose. This is much easier to work with than whatever random 
	pose you had before.
	
	From there, you just have to work your way through the tree, typing in rotation values and hitting the Update 
	button. I had originally intended to use spin box controls for adjusting the values, but C# doesn't appear to 
	have one that handles floating point values, and I feel like it would be a bit of a time suck to code one myself. 
	I may add sliders in the next version.
	
	You may notice that you set a bone rotation axis to a nice round "90" and go back to it later and see "90.0085" 
	or something like that. This has to do with the conversion between degrees and radians. You can keep trying to 
	fix it, but it's going to keep happening. Blame whoever taught us to think in degrees.
	
	You may have noticed two options in the Edit menu of the Bone Edit window.
	"Save Pose" will save the current Position, Rotation and Scale values for all bones in the clump to a binary file. 
	This is literally just a flat dump of the raw values stored in the order of each bone in it's parent clump. 
	There's no identifying information like names, or any advanced matching algorithm involved.
	
	"Load Pose" will load these values back. Again, this is just a flat dump of the values, so attempting to load the 
	pose from one model onto another will probably not work how you want it to. 
	
	I implemented these two for debugging some stuff, but it's kind of nice if you export a model and later realize 
	you messed something up and decide to go back and fix it.
	
	Once you've got the model posed how you like, in the main window you click "Export to .OBJ" in the Scene menu 
	and it'll dump the model out in that pose to .OBJ format. It *SHOULD* export the model exactly how you see it.
	
	Well, aside from the also famous flipped triangle problem. so far I haven't found anything that works 100% of 
	the time to fix this. IMOQ/F and GU both render everything double sided, so it may be that there's just no way 
	to fix it. I'll take a look at this again later, but for now, you're just going to have to manually flip them 
	yourself.

	Some final notes on ripping Gen1 models:
	Cutscenes in Generation 1 are going to be a massive pain to rip from right now. Their animation is stored 
	differently, and there is currently no support for loading that data. When I get Animations and Rigged exporting 
	working, I'll work on at least supporting enough of that data to make dumping those models easier. For right now, 
	there's only going to be one way to get those models out, and it requires you to manually note the bone positions 
	for the in-game model, and then set it for the cutscene model. Otherwise you'll just eyeball it. Either way, it's
	going to be a massive pain.
	
Known Bugs & Limitations:
	Collision Meshes:
		There's an odd bug with Collision Mesh rendering that was causing a Null Reference Exception. This never 
		happens when run through the debugger, so I haven't been able to fix it yet. as a result, only some of the 
		collision sub meshes may render in Scene mode.
		
		Currently, the Collision Meshes do not render in Preview Mode. Haven't gotten around to adding that code in 
		yet.
	
	Models:
		Vertices are stored in local bone space. Speculation is that this is because of the half-float format, 
		which combined with a Model's Scaling value, allows them to somewhat overcome the loss of precision.
		
		Generation 1:
			Deformable Models will render, however, due to the way the format works, everything will be collapsed into 
			the center by default. This is due to the bind pose for the skeletons being Non-Existent. 
		
		Generation 2 & 3: The parts may or may not be in the correct position/orientation.
	
		Shadow models do not render. There's really no need to render them, so for now, this will probably
		remain unimplemented.
		
		All models will probably have the infamous Flipped Triangles issue. The games render everything double sided,
		so there may not actually be a reliable way to fix this. 
	
	Textures:
		The texture preview just renders to a plane that can be rotated in the preview viewport. Not sure yet if I'll 
		change this to be a view aligned plane or not, but it seems to work fine as it is for now.
		
	Dummies cannot be currently previewed, nor is there currently a way to tell which dummy is which in Scene mode. 
	They can be exported to a simple ascii format for importing into blender with a companion python script that has 
	been included.
	
	Bounding boxes are currently not visualized in Preview or Scene mode.
	
	Animations cannot currently be played. There's some weird things happening with the rotations, so until I get
	that sorted, no real animation support.
	
	Lights:
		Due to the way lights are implemented in the files, Animation Playback support is needed to utilize them.
	
	Rendering:
		Only some Model types actually have Vertex Colors. In the games, the colors are utilized for baked 
		lighting. For models where no vertex colors were provided, they're set to rgba(0.5, 0.5, 0.5, 1.0), 
		this may appear to make some models look funny or dark. May need to change the rendering to deal with this
		in the future.
	
		I believe that model normals are correctly read, however there's currently no suport for using them 
		in either Preview or Scene mode. There is an option to export them when exporting to OBJ, but they mess with
		the lighting in blender. More investigation is needed on this. Probably doesn't help that I'm not 
		re-calculating them with bone transforms. Will work on that later.
		
		Also, I had no idea dealing with transparency was such a pain in the ass. Texture transparency is all jacked 
		up, and I'm not writing a super advanced rendering engine just to make it look right. I mean, ya'll are just
		going to export this stuff to import it into other game engines, seems like more work than it's worth right 
		now.
	
	Exporting:
		If the exported vertices all end up with coordinates of 0, 0, 0, try going into scene mode before exporting.
		There was a bug where the matrices weren't getting updated properly, but it should be gone now.
		
	Viewports:
		In both Preview and Scene mode, the mousewheel may not work for zooming in and out. I have no idea what 
		causes this, as it only happens on the one machine I have access for testing, but not development on. 
		for this reason I have added the zoom keys. 
		Also, zooming is not very smooth right now. Will work on that in coming builds.
	
Final Notes:
	There's probably some files that will either fail to load, or crash the program. I haven't checked compatibility 
	with every single CCS file in existence. If you come across a file that doesn't load, let me know so I can look 
	at it.
	
	Currently, this thing will only read the files. it cannot edit/replace/inject things in them. Support for those 
	features is planned, but the current priority is reading the files properly.
	
Questions I'm only answering once:
	Q: 'X or Y file doesn't load!'
	A: Not all files will currently load, though a vast majority of them will. Again, if you come across a file that 
	doesn't read, let me know so I can look into it.
	   
	Q: 'When is feature X or Y going to be added in? When will there be source code released?'
	A: When I get around to it.
	
	Q: 'Do you have a patreon? Paypal donation?'
	A: No, all that sounds like too much work. The more time I have to spend messing with all that, the less time 
	I get to spend working on anything else. If you absolutely must spend your money, go donate to a local no kill 
	animal shelter.

	Q: 'Why do some of the bones look so messed up?'
	A: Long story short, visualizing the bones is a complex process, and we don't have all the data we need to do it 
	properly. If a bone looks messed up, it's because it's not rotated in the direction of it's first child. If it's 
	that big of an issue I'll make a setting to switch the bone rendering to nub mode.
	   