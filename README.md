# Power Runner
Power Runner plugin for SportTracks (Desktop). Used to estimate power across a running activity. 

This plugin is for runners who would like the advantage of training with power.  Power data has been around for quite some time in the cycling community, and this plugin brings that to runners.

This software was open-sourced after SportTracks desktop was discontinued, and all restrictions have been removed.

### Getting Started
All you need is GPS data (with elevation) to use this plugin.  Simply select your activity, then
1) From the settings page, select the categories that contain running activities.
2) Go to the activity, select Edit > Power > Calculate Power Track

![pr_editmenu](https://mechgt.com/st/images/pr_editmenu.png)


**Additional things to improve the calculations**:  
Set your weight in the Athlete info page.  If no weight is set, default assumptions are made.

A new power track will be calculated from your GPS data (speed and elevation) and saved to the activity.  Use the power data for anything you like, including Training Load.

The menu is accessible from either Daily Activity view, or the Reports view.  You can select many activities in the reports view for an easy way to calculate power tracks for many activities at once.  A warning message will be displayed if ever you're about to overwrite an existing Power Track.

The calculations done by this plugin are specifically intended for RUNNING (or jogging, etc.) activities.

### Settings
See below for a description of the settings available.  Disable all non-running categories, and enable auto calculate for any categories in which you'd like the plugin to generate and store a power track automatically after import.

![pr_settings](https://mechgt.com/st/images/pr_settings.png)
