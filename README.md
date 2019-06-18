# CalibrateTooltip
Simple Unity demo of calibrating the tip of a tool on a VR controller.

This version tested in Unity 2018.4.2f1. You will need to install the SteamVR plugin with its usual settings. Tested with SteamVR Plugin, 2.3.2 (June 18th 2019).

The repository includes the [MathNet.Numerics DLL](https://numerics.mathdotnet.com/) for its PseudoInverse function.

## Instructions

The announcement of [Logitech's VR Ink Pilot Edition](https://www.logitech.com/en-roeu/promo/vr-ink.html) prompted me to dig out this piece of Unity code for calibrating the tip of a tool attached to the tracker. This is an example of a piece of code that I have written at least five times in different VR toolkits over the years. We often end up using it when constructing demos that have any sort of mixed-reality component where a VR object needs aligning with a real object, or we need to get precise input on a controller or you want to find the tip of the physical sword prop that you embedded your controller into 

The use is very straightforward. Make a stylus of your choosing and rigid attach it to a VR controller. I taped a mechanical pencil to a Vive Pro controller. Run the Unity project, hold the tip stationary (e.g. presss it firmly into a spongy mouse mat) and rotate the the wand around the tip. Click the trigger four or more times in different rotations. A small green ball will appear at the tip. If you keep the tip in the same position and rotate some more, the green ball should stay stationary.

## Maths

The maths of this is straightforward. The tooltip is stationary but in an unknown location. The translation offset between VR controller coordinates and the tooltip is fixed but unknown. The translation and rotations of the VR controller are all known. This makes a simple relationship that can be solved with three or more example rotations. I use four or more in the code as this gives some robustness to jitter. Markdown doesn't lend itself to maths, but you can [see the equation to solve on p17 (Section 4.3) of Tuceryan et al. 1995 (freely available on ResearchGate)](
https://www.researchgate.net/profile/Mihran_Tuceryan/publication/3410747_Calibration_Requirements_and_Procedures_for_a_Monitor-Based_Augmented_Reality_System). 

## Python

There is a Jupyter notebook with a Python/Numpy implementation of the same calibration. You can use this to validate another implementation, or check through the working. 

## Uses

* Make a simple modelling tool with a wand that gives you more precise input.
* Use it to get a fine position for drawing on a flat surface.
* Find a fixed point on a controller.
* Make a tool for measuring real objects around the user (in MR mode).
* Make a more precise virtual distance measuring tool inside your VR.






