# HConfig
This is a hierarchical config framework . The idea being to have multiple levels or planes of config . Each one maps onto a type of config .E.g User, Pc , workgroup,customer , product etc . When requesting a config value the highest level plane is searched first , if that fails to find a value then the next is searched etc .Until at some point we either have the context value or return config not found . 

Within each plane we can set a config value for a specific context e.g Specific PC and we can set a default value . By arranging these planes in turn we can search for a context value based on a hierarchy . 

This is clearly not the only approach to a hierarchical config . We could use SQL or Linq and impose a sort order to achieve a similar result . Both could be used , and there is an argument for doing so . The approach used then would be to simply cache the values . However config is often held in XML or Text files , in which case we would have to manipulate the config into a queryable format prior to running Linq queries . In which case it probably makes more sense to store the data in the hierarchy ( which is known prior to runtime) rather than impose an order on each and every lookup. Also it smells a little to have the storage mechanism of the config data influence how that data is searched. At least with this design we hide the config behind an Interface so that implementation is abstracted. 

<B>Example Use</B>

Imagine we have a config value of ReportEmailAddress . 
Initially we could have a single plane called (say) BaseConfig
In there we could set a Default Value for ReportEmailAddress.

Then we get a new requirment that for people in a IT department we want the report to go to a different recipient
We add a new plane called Departments . In there we add a Config Value for ReportEmailAddress and we set this plane
to be a higher priority than the BaseConfig. 

So when we search for ReportEmail Address here is what happens.

The Departments plane is searched first , for non IT people we find no departmental override so we search the defaults in the department plane . That has no values so we go to the next plane down which is the BaseConfig , that has no overrides and so we search the Defaults . There we find the ReportEmailAddress. Now to save time on the next search , every returned  value is cached . The cache will remain in use as long as no new data is added or the priority order of the planes set .

If however the context is for someone in the IT department then it picks up the override from the departmental plane , and that is cached and returned .

Assume that works fine until we get a new requirment . That is the network team and the server team within the IT department want to use their own address , then all we have to do is create a new plane called Team , and within that set the override for the Network and Server teams and change the priority. Then people in that team get the new address , the rest of it stay on the IT address and non it people stay with the default .

<B>Not Yet Implemented</B>

This design is all well and good for most simple configuration scenarios . What it currently does not deal with though is combinations of context values . Say for example we have a requirment that if a member of the network team is also a member of the Server team we want yet another ReportEmailAddress used then we do not have an elegant  way of copeing with that .I am currently thinking about how to cope with logical combinations of context values . 




 
