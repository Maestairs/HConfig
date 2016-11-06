# HConfig
This is a hierarchical config framework . The idea being to have multiple levels or planes of config . Each one maps onto a type of config .E.g User, Pc , workgroup,customer , product etc . When requesting a config value the highest level plane is searched first , if that fails to find a value then the next is searched etc .Until at some point we either have the context value or return config not found . 

Each plane has one or two main components . A set of default values and an optional set of overrides for a particular context.E.g we may set a set of default values for users , and override them for specific users . The metaphor used here is that of a bicycle wheel . The plane has a hub which holds the default values and spokes for each potential override.So the plane searches the spokes first , and if that fails it searches the hub . If the hub search fails then the next plane down is searched.

HConfig is initially set to be independant of config storage 

6-11-2016 At this point I am not sure if I will add parent child planes to each plane and manage the relationship that way or have a seperate config controller that manages that . The advantage of the latter being that it opens up the possibility of programatically changing the order of the levels .
