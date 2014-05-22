NuSet
=====

An open sourced Visual Studio 2013 Extension used to easily create NuGet packages out of class libraries.

The goal for this extension is to fill the gap between setting up a NuGet package and finalizing the package out to either the NuGet repository online, local, or company hosted repository. 

I have been using PSake and building the packages to the specification from http://docs.nuget.org/docs/creating-packages/creating-and-publishing-a-package

Any code checkins should have a unit test, and should be added into the integration testing for the Visual Studio Extension.

List of things I hate when I see code
1. Hungarian notation in C# (It's 2014... it's useless)
2. Use var if you can plainly see what the object is, you don't need to be reminded every time you look at an object of what type it is
3. No comments for the summary
4. Comments inside of methods trying to explain what the hell it is doing... Just write smaller pieces that explain what the method is by the name 
5. Spacing all over the place, why have spaces and code everywhere. Clean it up!
 
There are tons more, but this is a start...
Use StyleCop and ReSharper or some other refactoring tool 
