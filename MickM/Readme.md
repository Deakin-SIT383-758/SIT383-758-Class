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

The week 3 workshop consisted of the introduction to Photon Networking. The prototype for the week 3 submission is instead the Range Invariant Markers component developed separately. 

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

...

#### Extensions:
...



### "Week 6 Prototype"

...

#### Extensions:
...



### "Week 7 Prototype"

...

#### Extensions:
...



### "Week 8 Prototype"

...

#### Extensions:
...
