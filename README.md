# HConfig
This is a hierarchical config framework . The idea being to have multiple levels or planes of config . Each one maps onto a type of config .E.g User, Pc , workgroup,customer , product etc . When requesting a config value the highest level plane is searched first , if that fails to find a value then the next is searched etc .Until at some point we either have the context value or return config not found . 

Within each plane we can set a config value for a specific context e.g Specific PC and we can set a default value . By arranging these planes in turn we can search for a context value based on a hierarchy . 

This is clearly not the only approach to a hierarchical config . We could use SQL or Linq and impose a sort order to achieve a similar result . But Config priority is normally known before run time , so it seems to make sense to impose this early and not waste time at runtime using sorts . Also HConfig is independant of storage , so the next stage will be to have a storage interface that allows it to be populated from a variety of stores .

<B>Example Use</B>
 
