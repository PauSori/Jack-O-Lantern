*upon opening the project you might have blank scripts with name "nothing selected" in the Main Camera under whichever scene you open and post processing scripts missing. 
This is normal because some additional assets are not included in the package:

Note: These additional assets are not included in the project due to the Asset Store Guidelines. Dont worry its an easy fix.

To get the same effects I had, kindly download these assets first.
KEEP IN MIND: make sure you import the Standard Assets first, else it will not update properly and you will have to do the additional instructions below.
Standard Assets: https://www.assetstore.unity3d.com/en/#!/content/32351 (make sure you only import "image effects" folder, its under effects folder. Thats all you need from Standard Assets. )
Post Processing Stack: https://www.assetstore.unity3d.com/en/#!/content/83912

After those Assets are imported in the project, the post processing profiles and scripts should replace the missing ones. (This should happen in the Unity Latest Update 2017.1.1f1)
If not, just follow along below:

Assuming you're done downloading, open the following scene and do the following.
*Scene: AllAssets
-Go to Main Camera under hierarchy and add the following component:
-Add Post Processing behaviour Script by clicking Add Component and drag the "Portal" profile under the PP Profiles Folder (YN-Processing Profile Folder)

*Scene: Hell
-Go to Main Camera under hierarchy and add the following component:
-Add Post Processing behaviour Script by clicking Add Component and drag the "Hell" profile under the PP Profiles Folder (YN-Processing Profile Folder)
-Add Bloom Optimized Script (Threshold: .12 Intensity: .14)
-Add Depth Of Field Script (Aperture .26)

*Scene: Portal
-Go to Main Camera under hierarchy and add the following component:
-Add Post Processing behaviour Script by clicking Add Component and drag the "Portal" profile under the PP Profiles Folder (YN-Processing Profile Folder)
-Add Bloom Script (Intensity: .63)
-Add Screen Space Ambient Occlusion (Radius: .29 Occlusion Intensity: 4)

*Scene: Swamp
-Go to Main Camera under hierarchy and add the following component:
-Add Post Processing behaviour Script by clicking Add Component and drag the "Swamp" profile under the PP Profiles Folder (YN-Processing Profile Folder)
-Add Bloom Script (Intensity: .6)
-Add Screen Space Ambient Obscurance (Intensity: .53 Radius: .1 Blur Iterations: 3 Blur Filter Distance: 5)
-Motion Blur (Blur Amount: .7)

If you have any suggestions, ideas about how to improve this asset & more, feel free to message me: 
-yumeofficial24@gmail.com
-https://discordapp.com/invite/etX2zJB [@LeFluff]
-https://connect.unity.com/u/581f502b32b306001b44485a
