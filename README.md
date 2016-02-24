# ResourceFitness (*Resfit*)

Resfit is a multipurpose tool for working with and mangling your resources (*.resx) in .NET. 

## For example,
In large projects, even renaming resource keys can be a time-consuming chore. At its heart, *Resfit* allows you to load resources from your source files as `ResourceList` objects and then attach one or more "transforms" (`ITranform`) to any `Resource` within the `ResourceList` - For instance, a `ResourceReplacementTransform` will replace the resource it's attached to with a brand new resource (key and value) of your making. After attaching one or more of these "transforms" to your resources, simply execute `TransformFolder` on the ResourceList, and all of the resources within the supplied folder path will be transformed. Voila!

## Build it / Use it
Resfit is still a wobbling toddler and is only a code library. As always, use it at your own risk. (Remember, "toddler".) To build it, just clone the repo and run `.\build` from PowerShell. (You will need nuget and msbuild in your path.) Everything will be built in the `.build` folder. From there you can grab the `*.dll`s you need and drop them into your project.

### Coming soon
 - A PowerShell driver
 - A basic GUI (?)
 - More Core functionality

## Philosophy
*Resfit* is a ATDD project, largely because I wanted to experiment with the technique. So when I envision a new feature, I first write the requirements as a Gherkin (Cucumber) specification using the *Given-When-Then* syntax. Then I use SpecFlow to create a failing acceptance test for the new feature. Then I gradually make the acceptance test pass, one new class at a time. Each class is built using TDD, which requires a failing unit test for each new behavior. I get these tests passing immediately, refactor to improve the design, then move to the next feature needed to flesh out the specification, all the while making sure that none of the other tests have turned "Red". The flow goes like this: Think -> Red -> Green -> Refactor -> `Goto Think`

Every time you run `.\build` from PowerShell, the entire solution is built from the ground up and all of the tests are run, both the acceptance tests and the unit tests. Code coverage metrics are output to `.build\TestCoverage\index.html`, courtesy of OpenCover and ReportGenerator. The goal here is not to achieve 100% coverage. The goal is a sensible and pragmatic level of coverage. I started the project using NCrunch, which was *amazing* ("TDD Crack" is not an unfair designation!), but the trial expired and I can't justify the expense right now.
