# Component Development Portfolio

### Context
This repo serves as the content developed during SIT758 workshops and submitted as XR/VR Components for the portfolio assessment.

Note that not all workshop items are used as final submission items. 

## Portfolio submission item overview
The below items are submitted for final portfolio. Note that the naming convention of the portfolio submissions are of the form "Week X Prototype" however they are not required to be workshop items. This format is used for alignment with the submission guide but in effect they are "Portfolio Item N".


### "Week 1 Prototype"

This is an extension of the week 1 workshop which consisted of displaying a mobile phone camera on a Webcam Texture. 

#### Extensions:

- Camera -> Image pixels -> Array of image pixels -> "Flipbook" visualisation by cycling applying pixels to texture, iterating through stored images over time.

 - Additional sensors (GPS, Gyro) + Image pixels -> Array of frames with metadata (pixels, location, location accuracy, boresight angles) to support location identification (simple "where was this taken" through to CV object detection and location estimation through altitude and boresight ray calculations)




### "Week 3 Prototype"

The week 3 workshop consisted of the introduction to Photon Networking. The prototype for the week 3 submission is instead the Range Invariant Markers component developed separately. *Note: The project for this submission is *MickM/RangeInvariantMarkers* and not */MickM/Week3* - the latter is just the workshop endstate and maintained for future reference.

The intent of this component is to have physical markers in 3D space indicating a position at some (longer) range that will allow the user to look at them; in this case they are removed so the user can see the point of interest and an information box appears with contextual information.

Full feature development repo:
https://github.com/MickWPM/RangeInvariantMarkers

#### Features:
- Uses double as datatype for marker position for precision
- Contains static conversion helper to confirm if they should be rendered and at what position
- Rendering is based on markers being within an (optional) min and max range (ie. only render if more than 10m away and dont render if more than 10km away)
- Rendering position is in line with object location but at a customisable render distance (eg. render all markers 15m from observer)
- Rendering position respects real world position over derived rendering position if object is inside render distance
- Marker rendering fades out over time as user is looking at the location, fading in over time as the user looks away
- Fade in/out time customisable, as is the gaze warmup time, ie. how long a user needs to be looking at the marker before the fade out starts (and opposide for looking away for fade back in)
- Gaze collision based on collider for marker visuals, separate text box collider considered when text box is visible in order to stop marker fading back in as user "looks away" to the info box.


### "Week 4 Prototype"

This workshop introduced hand pose detection and implemented Pinch Detection by calculating the distance between thumb and forefinger.

#### Extensions:

This was extended to become a general hand pose matcher:
- User saved hand positions -> Array of stored positions
- Sum of squared errors to match hand positions -> Iterate current pose over saved poses to find likely match based off sum of square errors (vector maths between points)
- Implemented a delegate matching method to allow dynamic matching approach changes.
- Add rotation invariance -> Initial approach only works with near identical orientation. Calculated hand rotation to update matching approach and allow different hand rotation (primarily in the frame plane). 
- Added a 3D visualisation of the pose plane to highlight relative strengths and weaknesses (ie. primarily effective at "rotation in the frame plane" and broad "facing towards or away", rotation on non frame plane axis more limited)

A third matching approach to test AI optimisations of the rotation invariance maths was implemented but resulted in negligible performance change.



### "Week 5 Prototype"

The week 5 submission is a custom implementation of C/D ratio for weight emulation in VR. This is based off a paper I did for SIT755 which looked at how we can emulate weight using best practices from both VR/AR development lessons learned and broader literature including psychology research findings.

The link to the repo is [here](https://github.com/MickWPM/VRWeightEmulationPrototype). 

The component itself consists of an individual item script which manages the C/D ratio and item specific parameters and a global weight emulation manager which implements the C/D ratio proposed in the paper. The resulting impact is a psycologically grounded emulation of weighty objects in VR which is easy to drag and drop into any project, flexibly managing physics implemenetations both with and without rigidbodies.


### "Week 6 Prototype"

The week 6 submission is the integration of Cesium within Unity. This allows full global scale heights and terrain shading within Unity. The Cesium package streams both heightmap and textures based on the georeference origin (lat/long).

The integration in this prototype consists of:
- Integrating VR into Cesium
- Integrating zoom in/out using VR triggers, scaling zoom speed based on zoom level
- Integrating VR scale changes as zoomout occurs to change interpupillary distance and make the terrain look "smaller" (rather than just being high up)
- Creating "movement" control by reading controller stick position and updating georeference origin lat/long (this helps Cesium render correctly and avoids floating point issues)
- Scaling movement speed based off zoom level.
- Integrating terrain texture changes through Cesium Raster Overlay asset ID changes at runtime
- Integrating terrain height detection to offset user ground position to prevent being stuck below ground (eg. when entering mountain areas)

_Note for replication: Cesium requires a user key which has been excluded from git for security reasons. To use this project:_
- [Create a Cesium Ion Account](https://ion.cesium.com/signup)
- [Connect to Cesium inside Unity](https://cesium.com/learn/unity/unity-quickstart/#step-2-connect-to-cesium-ion)

### "Week 7 Prototype"

The week 7 and 8 prototypes are in a single repo: _ARVRTransition_.

The week 7 prototype is the transition iteself. The transition involves movement from a location in AR to a VR player location (located inside an AR castle object)

The user can move the AR castle around and the transition always brings them to the VR player location.
This compontent includes:
- Smooth (time adjustable) transition from AR to VR and VR to AR (Trigger squeeze to commence, release to return)
- AR scene opens only at the point that we are almost in AR; this avoids sickness from AR objects moving and scaling while our environment remains static
- VR and AR only elements; AR only including the obejct selection box. VR only including skybox, additional castle decorations and ambeint sound


### "Week 8 Prototype"

The week 7 and 8 prototypes are in a single repo: _ARVRTransition_.

The week 8 component maps real world pixels to texture within the game world. As the user moves around an AR object, when they release the object:
- The pixel real world camera frame is recorded.
- Four positions, aligned to the corners of a quad in unity space, are translated into VR screen space. This is done with the support of camera intrinsics to avoid FOV issues.
- A custom shader maps real world pixel colours to texture colour using linear interpolation between the real world-image coordinates of the ground plane.

This component took significant planning and testing to understand the camera component capturing and developing the approach to map edges of the ground plane to image location; the end result was an incredibly rewarding bit of immersion.

_Generative AI disclaimer_: The camera intrisics mapping and shader (complete) were generated using Gemini AI. 
- The latter was done as I am unfamiliar with HLSL; I described the maths I want ("Linear interpolation between real world points mapped to image locations that correspond with edges of the plane") and reviewed the generated code to confirm the maths was correct.
- The former was asked as "how do I apply the camera intrinsics to offset the camera element image warping" as the initial image when mapped using the shader was very zoomed in due to the difference between eye rendered images and the fully captured image.


### "Overflow Prototype"

This component consists of a wrist mounted quick menu with the ability to pull out detailed panels. The broad intent is a simple "quick menu" that also has the ability to easily gain detailed information or advanced interactions (beginner vs power user). The context as presented is for a VR Lecturer looking through lecture control tabs (dashboard, scrollable lecture notes, attendees and lecture controls) however the system is generic for any case where you have a "quick menu" of common actions and less common but more detailed actions (or reference requirements) that you can access separately. 

The functionality developed is:
- The wrist menu is activated by looking at your wrist, similar to looking at a watch. The functionality is via angle comparison between the wrist menu and the look direction. This facilitates menu interaction with low cognitive overhead.
- The wrist menu is interactable with clear buttons and hover effect feedback. Each button opens a panel on the wrist menu with some commonly required quick actions.
- When additional information is required, the user can grab one of the "handles" (top and right sides of the wrist menu) and pull them out to create a larger detailed screen. This is a separate screen designed with additional detail. The screen can be freely positioned in virtual space and referenced /interacted with as required.
- When the larger screen is no longer required, the user simply grabs the panel and "puts it back in" the wrist menu. The user can pull it out again later as required.

The code for this is available at [VRDesignDemo](https://github.com/MickWPM/VRDesignDemo)
